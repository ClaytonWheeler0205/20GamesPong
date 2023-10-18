using Godot;
using System;
using Game.Paddle;

namespace Game.Ball
{

    public class PongBall : KinematicBody2D
    {
        // Ball movement member variables
        [Export]
        private float _speed = 300.0f; // The starting speed of the ball
        [Export]
        private float _speedIncrease = 10.0f; // The amount the ball speeds up every time the paddle is hit.
        private float _currentSpeed; // The current speed, used to move the ball. Updates on each paddle hit
        private Vector2 _direction = Vector2.Zero; // The direction vector for the ball.
        private bool _isMoving = false; // Vector to let the update function know that the ball should be moved. Set to true on a round start.

        // Node reference member variables
        private Sprite _spriteRef = null; // Reference to the sprite node to make it invisible on start

        // Ball starting position member variables

        // This will be the in center of the arena. This vector will also be used to determine which side the
        // ball is on when is hits a goal for scoring purposes. The center of the arena is defined as the position the ball is placed
        // in the scene by the designer.
        private Vector2 _startPos;

        // We export the min and max Y values to determine the range of our random y position in the arena
        // This values will be used to give the ball a random starting position on the start of a round.
        [Export]
        private float _minY = 0.0f;
        [Export]
        private float _maxY = 0.0f;

        // Collision layers for special collision cases when handling ball colision
        private const int GOAL_COLLISION_LAYER = 2;
        private const int PADDLE_COLLISION_LAYER = 4;

        private const float MAX_BOUNCE_ANGLE = (float)(Math.PI / 3); // 60 degrees
        private bool _hasHitPaddle = false; // bool to make sure we only handle paddle collision only a single time we hit a paddle

        // Signals
        [Signal]
        delegate void PlayerScored(Player scoringPlayer);

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _spriteRef = GetNode<Sprite>("BallSprite");
            if( _spriteRef == null)
            {
                GD.PrintErr("Sprite node not found! Does the sprite node exist? If it does, check the sprite node name.");
            }
            _startPos = Position;
            ResetBall();
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_isMoving)
            {
                MoveBall(delta);
            }
        }

        /// <summary>
        /// Resets the ball's position back to the center of the arena, sets the ball's velocity to zero, and sets the ball to be invisible. 
        /// This occurs at the start of the game, and whenever the ball hits the goal
        /// </summary>
        private void ResetBall()
        {
            Position = _startPos;
            _direction = Vector2.Zero;
            _isMoving = false;
            _spriteRef.Visible = false;
        }

        /// <summary>
        /// Called by the pong game to let the ball know it should start moving and give it a random y position and angle depending on which
        /// player the ball should move towards. At the start of the game, it should face player 1, then every other call to this function
        /// should face the player whose goal was hit by the ball.
        /// </summary>
        /// <param name="playerToFace">Determines the player should face and move towards. Defines the angle the ball direction will have</param>
        public void StartBall(Player playerToFace)
        {
            float xPos = _startPos.x;
            float yPos = (float)GD.RandRange(_minY, _maxY);
            Position = new Vector2(xPos, yPos);
            float angle = 0.0f;
            switch(playerToFace)
            {
                case Player.PLAYER_ONE:
                    angle = (float)GD.RandRange((Math.PI / 18), (17 * Math.PI / 18));
                    break;
                case Player.PLAYER_TWO:
                    angle = (float)GD.RandRange((19 * Math.PI / 18), (35 * Math.PI / 18));
                    break;
            }
            _direction = new Vector2(0, -1).Rotated(angle);
            _spriteRef.Visible = true;
            _currentSpeed = _speed;
            _isMoving = true;
        }

        /// <summary>
        /// Move the ball according to its direction and speed. If the ball hits a physics object, HandleCollision is called
        /// </summary>
        private void MoveBall(float delta)
        {
            KinematicCollision2D collision = MoveAndCollide(_direction * _currentSpeed * delta);
            if(collision != null)
            {
                HandleCollision(collision);
            }
        }

        /// <summary>
        /// Determinds what the ball should do in response to a certain collision. If the collision is either a wall of the paddle, the
        /// ball should simply bounce off the collision. If the collision is a goal, it should send a signal that a player scored, and then
        /// call ResetBall.
        /// </summary>
        /// <param name="collision">The collision that the ball is handling and determining its type</param>
        private void HandleCollision(KinematicCollision2D collision)
        {
            CollisionObject2D collisionObject = collision.Collider as CollisionObject2D;
            if(collisionObject != null)
            {
                if(collisionObject.CollisionLayer == GOAL_COLLISION_LAYER)
                {
                    // Hit a goal physics object! Now we determine who scored a point
                    if(Position.x > _startPos.x) // Player one goal. Player two has scored!
                    {
                        EmitSignal("PlayerScored", Player.PLAYER_TWO);
                        GD.Print("Player two scores!");
                    }
                    else // Player two goal. Player one has scored!
                    {
                        EmitSignal("PlayerScored", Player.PLAYER_ONE);
                        GD.Print("Player one scores!");
                    }
                    ResetBall();
                    _hasHitPaddle = false;
                }
                else if(collisionObject.CollisionLayer == PADDLE_COLLISION_LAYER && !_hasHitPaddle)
                {
                    // Change the angle of the ball depending on how far from the center of the paddle the ball hit, up to a max angle
                    // of 75 degrees.
                    PaddleBase collidingPaddle = collision.Collider as PaddleBase;
                    float bounceAngle = GetBounceAngle(collidingPaddle);
                    // Set the direction vector with trig
                    _direction.x = (float)Math.Cos(bounceAngle);
                    _direction.y = (float)-Math.Sin(bounceAngle);
                    _direction = _direction.Normalized(); // normalize the vector
                    // set this bool to true so we don't handle paddle collision again until the ball has left the paddle's collision
                    _hasHitPaddle = true;
                    // Increase the speed on a ball hit
                    _currentSpeed += _speedIncrease;
                }
                else
                {
                    _direction = _direction.Bounce(collision.Normal);
                    _hasHitPaddle = false;
                }
            }
            else
            {
                _hasHitPaddle = false;
            }
        }

        private float GetBounceAngle(PaddleBase collidingPaddle)
        {
            float relativeIntersectY = collidingPaddle.Position.y - Position.y;
            // This should give us a value between -1.0 and 1.0
            float relativeIntersectYNormalized = (relativeIntersectY / (collidingPaddle.GetPaddleSize() / 2));
            // GD.Print("Relative angle: " + relativeIntersectYNormalized);
            float bounceAngle = relativeIntersectYNormalized * MAX_BOUNCE_ANGLE;
            // Clamp to ensure that our angle is between the values we set it to
            bounceAngle = Mathf.Clamp(bounceAngle, -MAX_BOUNCE_ANGLE, MAX_BOUNCE_ANGLE);
            return bounceAngle;
        }
    }
}