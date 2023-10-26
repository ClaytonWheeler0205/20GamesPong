using Godot;
using System;
using Game.Paddle;

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
        PackedScene _ballHitScene;
        AudioStreamPlayer _ballHitSFX;
        PackedScene _goalHitScene;
        AudioStreamPlayer _goalHitSFX;

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
            _ballHitScene = GD.Load<PackedScene>("res://Pong/Scenes/FX/SFX_PongBallHit.tscn");
            _goalHitScene = GD.Load<PackedScene>("res://Pong/Scenes/FX/SFX_PongGoalHit.tscn");
            // Player 1 is always the first player the ball should move towards.
            _playerToFace = Player.PLAYER_ONE;
            _spriteRef = GetNode<Sprite>("%BallSprite");
            if(_spriteRef == null)
            {
                GD.PrintErr("Sprite node not found! Does the sprite node exist? If it does, check the sprite node name.");
            }
            base._Ready();
        }

        public override void ResetBall()
        {
            _spriteRef.Visible = false;
            base.ResetBall();
        }

        public override void StartBall()
        {
            // set the ball to the center of the arena and give it a random y position
            float xPos = StartPos.x;
            GD.Randomize();
            float yPos = (float)GD.RandRange(_minY, _maxY);
            Position = new Vector2(xPos, yPos);
            float angle = 0.0f;
            // set the angle of the ball determined by which side of the arean the ball should move towards
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
            SetDirection(new Vector2(0, -1).Rotated(angle));
            _currentSpeed = Speed;
            _spriteRef.Visible = true;
            base.StartBall();
        }

        protected override void MoveBall(float delta)
        {
            KinematicCollision2D collision = MoveAndCollide(Direction * _currentSpeed * delta);
            if (collision != null)
            {
                HandleCollision(collision);
            }
            else
            {
                _hasHitPaddle = false;
            }
        }

        protected override void HandleCollision(KinematicCollision2D collision)
        {
            CollisionObject2D collisionObject = collision.Collider as CollisionObject2D;
            if (collisionObject != null)
            {
                if (collisionObject.CollisionLayer == GOAL_COLLISION_LAYER)
                {
                    _goalHitSFX = _goalHitScene.Instance() as AudioStreamPlayer;
                    GetNode("/root").AddChild(_goalHitSFX);
                    // Hit a goal physics object! Now we determine who scored a point
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
                    ResetBall();
                    _hasHitPaddle = false;
                    EmitSignal("GoalHit");
                }
                else if (collisionObject.CollisionLayer == PADDLE_COLLISION_LAYER)
                {
                    if (!_hasHitPaddle)
                    {
                        // set this bool to true so we don't handle paddle collision again until the ball has left the paddle's collision
                        _hasHitPaddle = true;
                        _ballHitSFX = _ballHitScene.Instance() as AudioStreamPlayer;
                        GetNode("/root").AddChild(_ballHitSFX);
                        // Change the angle of the ball depending on how far from the center of the paddle the ball hit, up to a max angle
                        // of 75 degrees.
                        PaddleBase collidingPaddle = collision.Collider as PaddleBase;
                        float bounceAngle = GetBounceAngle(collidingPaddle);
                        // Set the direction vector with trig
                        float newX = (float)Math.Cos(bounceAngle);
                        float newY = (float)-Math.Sin(bounceAngle);
                        // Determine if the x position of the velocity vector should be negative or positive depending on which paddle hit the ball
                        if (Position.x > StartPos.x)
                        {
                            SetDirection(new Vector2(-newX, newY));
                        }
                        else
                        {
                            SetDirection(new Vector2(newX, newY));
                        }
                        SetDirection(Direction.Normalized()); // normalize the vector
                                                            // Increase the speed on a ball hit
                        _currentSpeed += _speedIncrease;
                    }
                }
                else
                {
                    _ballHitSFX = _ballHitScene.Instance() as AudioStreamPlayer;
                    GetNode("/root").AddChild(_ballHitSFX);
                    SetDirection(Direction.Bounce(collision.Normal));
                    _hasHitPaddle = false;
                }
            }
            else
            {
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