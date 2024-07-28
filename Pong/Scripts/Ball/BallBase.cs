using Godot;

namespace Game.Ball
{

    /// <summary>
    /// The base abstract class for a bouncing ball object for use in various arcade style games (ex. Pong & Breakout)
    /// </summary>
    public abstract class BallBase : KinematicBody2D
    {
        // Movement variables
        /// <summary>
        /// Base speed of the ball
        /// </summary>
        [Export]
        private float _speed = 300.0f;
        protected float Speed => _speed;

        /// <summary>
        /// Vector representing the direction of the ball
        /// </summary>
        private Vector2 _direction;
        public Vector2 Direction => _direction;

        /// <summary>
        /// Boolean to determine if the ball should be moving or should be stationary. Movement code will only be executed if this value
        /// is set to true.
        /// </summary>
        private bool _isMoving;
        public bool IsMoving => _isMoving;

        /// <summary>
        /// Vector representing the starting position of the ball. Used to revert the ball back to a starting position on reset.
        /// </summary>
        private Vector2 _startPos;
        protected Vector2 StartPos => _startPos;

        // Called when the node enters the scene tree for the first time.
        // Set up the starting position, and reset the ball to ensure it isn't active unless called to activate
        public override void _Ready()
        {
            _startPos = Position;
            ResetBall();
        }

        public override void _PhysicsProcess(float delta)
        {
            if(_isMoving)
            {
                MoveBall(delta);
            }
        }

        /// <summary>
        /// Resets the data of the ball object to that it returns to it's starting position and is no longer moving.
        /// </summary>
        public virtual void ResetBall()
        {
            _isMoving = false;
            _direction = Vector2.Zero;
            Position = _startPos;
        }

        /// <summary>
        /// Sets up the ball data to prepare it to be active in the game, and set the boolean _isMoving to true so movement code can be run.
        /// </summary>
        public virtual void StartBall()
        {
            _isMoving = true;
        }

        /// <summary>
        /// Readies the ball for a game's reset
        /// </summary>
        public abstract void ReadyBall();

        protected void SetDirection(Vector2 newDirection)
        {
            _direction = newDirection;
        }

        /// <summary>
        /// Moves the ball around the scene, if a collision with the KinematicBody2D occurs, HandleCollision is called.
        /// </summary>
        /// <param name="delta">Time between last physics frams to perform movement over time. Parameter is given by _PhysicsProcess</param>
        protected abstract void MoveBall(float delta);

        /// <summary>
        /// Checks the information about the collision that the ball collided with and responds depending on the kind of object it
        /// collided with.
        /// </summary>
        /// <param name="collision">The collision object that the ball has collided with</param>
        protected abstract void HandleCollision(KinematicCollision2D collision);
    }
}