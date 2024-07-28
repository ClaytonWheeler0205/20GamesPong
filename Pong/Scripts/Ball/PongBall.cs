using Godot;
using System;
using Game.Paddle;
using Util.ExtensionMethods;

namespace Game.Ball
{

    public class PongBall : BallBase
    {
        /// <summary>
        /// When the ball touches a paddle, the current speed of the ball will be increased by this amount. This will make the ball
        /// go faster on every paddle hit.
        /// </summary>
        [Export]
        private float _speedIncrease = 10.0f;

        /// <summary>
        /// The true speed of the pong ball. Determined by the base ball's speed plus any speed increases added by hitting the paddle
        /// </summary>
        private float _currentSpeed;

        /// <summary>
        /// Reference to the sprite node in the pong ball scene tree
        /// </summary>
        private Sprite _spriteRef;

        // Min and max Y values representing the top and bottom of the arean to give the pong ball a random starting y position.
        [Export]
        private float _minY = 0.0f;
        [Export]
        private float _maxY = 0.0f;

        // Collision layers for handling the collision of specific objects in the pong game
        private const int GOAL_COLLISION_LAYER = 2;
        private const int PADDLE_COLLISION_LAYER = 4;

        /// <summary>
        /// The maximum angle of the ball that will bounce off the paddle when the ball hits the paddle. The largest angle will occur if the
        /// ball hits the paddle the furthest point away from the center.
        /// </summary>
        private const float MAX_BOUNCE_ANGLE = (float)(Math.PI / 3); // 60 degrees

        /// <summary>
        /// boolean to make sure handling any collision with the paddle occurs only one time the ball touches the paddle
        /// Ensures that bouncing away from the paddle occurs only once. Set to true when the ball collides with a paddle
        /// and false if the ball isn't colliding with the paddle.
        /// </summary>
        private bool _hasHitPaddle;

        /// <summary>
        /// The player the ball should face/move towards at the start of a round.
        /// </summary>
        private Player _playerToFace;

        // AudioStreamPlayer nodes to create and add to the game
        PackedScene _ballHitSFX;
        PackedScene _goalHitSFX;

        /// <summary>
        /// Signal to let the game know that a player has scored a point (i.e. the ball has made contact with a player's goal)
        /// </summary>
        /// <param name="scoringPlayer">The player that scored a point</param>
        [Signal]
        delegate void PlayerScored(Player scoringPlayer);

        /// <summary>
        /// Singnal to let the game know that the ball has hit a goal in the arena.
        /// </summary>
        [Signal]
        delegate void GoalHit();

        public override void _Ready()
        {
            // Create scene instances of the SFX to play
            _ballHitSFX = GD.Load<PackedScene>("res://Pong/Scenes/FX/SFX_PongBallHit.tscn");
            _goalHitSFX = GD.Load<PackedScene>("res://Pong/Scenes/FX/SFX_PongGoalHit.tscn");

            // Player 1 is always the first player the ball should move towards.
            _playerToFace = Player.PLAYER_ONE;
            _spriteRef = GetNode<Sprite>("%BallSprite");
            if (!_spriteRef.IsValid())
            {
                GD.PrintErr("Sprite node not found! Does the sprite node exist? If it does, check the sprite node name.");
            }
            base._Ready();
        }

        public override void ResetBall()
        {
            if (_spriteRef.IsValid())
            {
                _spriteRef.Visible = false;
            }
            base.ResetBall();
        }

        public override void StartBall()
        {
            // set the ball to the center of the arena and give it a random y position. Also reset its angle.
            float xPos = StartPos.x;
            GD.Randomize();
            float yPos = (float)GD.RandRange(_minY, _maxY);
            Position = new Vector2(xPos, yPos);
            float angle = 0.0f;

            // set the angle of the ball determined by which side of the arena the ball should move towards (towards player one of two)
            GD.Randomize();
            switch (_playerToFace)
            {
                case Player.PLAYER_ONE:
                    angle = (float)GD.RandRange((Math.PI / 18), (17 * Math.PI / 18));
                    break;

                case Player.PLAYER_TWO:
                    angle = (float)GD.RandRange((19 * Math.PI / 18), (35 * Math.PI / 18));
                    break;
            }

            // Set the ball's direction, speed, and visibility
            SetDirection(new Vector2(0, -1).Rotated(angle));
            _currentSpeed = Speed;
            if (_spriteRef.IsValid())
            {
                _spriteRef.Visible = true;
            }

            base.StartBall();
        }

        public override void ReadyBall()
        {
            _playerToFace = Player.PLAYER_ONE;
        }

        protected override void MoveBall(float delta)
        {
            // Move the ball and grab a KinematicCollision2D if we have hit something
            KinematicCollision2D collision = MoveAndCollide(Direction * _currentSpeed * delta);
            if (collision.IsValid())
            {
                HandleCollision(collision);
            }
            else
            {
                // If we didn't hit anything, then we definitely didn't hit the paddle!
                _hasHitPaddle = false;
            }
        }

        // Note that collision not being null is a precondition to this method being called.
        protected override void HandleCollision(KinematicCollision2D collision)
        {
            // Get the object we collided with, to check its collision layer
            CollisionObject2D collisionObject = collision.Collider as CollisionObject2D;

            // Case 1: The ball hit a goal.
            if (collisionObject.CollisionLayer == GOAL_COLLISION_LAYER)
            {
                GetNode("/root").AddChild(_goalHitSFX.Instance());

                // Determine who scored a point based on if the x position is to the right or left of center.
                if (Position.x > StartPos.x) // Player one goal. Player two has scored!
                {
                    EmitSignal("PlayerScored", Player.PLAYER_TWO);
                    _playerToFace = Player.PLAYER_ONE;
                }

                else // Player two goal. Player one has scored!
                {
                    EmitSignal("PlayerScored", Player.PLAYER_ONE);
                    _playerToFace = Player.PLAYER_TWO;
                }

                // Reset
                ResetBall();
                _hasHitPaddle = false;
                EmitSignal("GoalHit");
            }

            // Case 2: The ball hit a paddle.
            else if (collisionObject.CollisionLayer == PADDLE_COLLISION_LAYER)
            {
                // Only handle paddle collision if we haven't handled the collision for this paddle yet.
                if (!_hasHitPaddle)
                {
                    _hasHitPaddle = true;

                    // Play a ball hit sound effect
                    GetNode("/root").AddChild(_ballHitSFX.Instance());

                    // Change the angle of the ball depending on how far from the center of the paddle the ball hit, up to a max angle
                    // of 75 degrees.
                    PaddleBase collidingPaddle = collision.Collider as PaddleBase;
                    float bounceAngle = GetBounceAngle(collidingPaddle);

                    // Set the direction vector with trig
                    float newX = (float)Math.Cos(bounceAngle);
                    float newY = (float)-Math.Sin(bounceAngle);

                    // Determine if the x position of the velocity vector should be negative or positive depending on which paddle hit the ball
                    if (Position.x > StartPos.x) // Right paddle
                    {
                        SetDirection(new Vector2(-newX, newY));
                    }

                    else // Left paddle
                    {
                        SetDirection(new Vector2(newX, newY));
                    }

                    // Normalize the vector and increase the ball's speed.
                    SetDirection(Direction.Normalized());
                    _currentSpeed += _speedIncrease;
                }
            }

            // Case 3: The ball hit a wall
            else
            {
                // Play the ball hi sound effect, and bounce off the wall
                GetNode("/root").AddChild(_ballHitSFX.Instance());
                SetDirection(Direction.Bounce(collision.Normal));
                _hasHitPaddle = false;
            }
        }

        private float GetBounceAngle(PaddleBase paddle)
        {
            float relativeIntersectY = paddle.Position.y - Position.y;
            // This should give us a value between -1.0 and 1.0
            float relativeIntersectYNormalized = (relativeIntersectY / (paddle.GetPaddleSize() / 2));
            // GD.Print("Relative angle: " + relativeIntersectYNormalized);
            float bounceAngle = relativeIntersectYNormalized * MAX_BOUNCE_ANGLE;
            // Clamp to ensure that our angle is between the values we set it to
            bounceAngle = Mathf.Clamp(bounceAngle, -MAX_BOUNCE_ANGLE, MAX_BOUNCE_ANGLE);
            return bounceAngle;
        }
    }
}