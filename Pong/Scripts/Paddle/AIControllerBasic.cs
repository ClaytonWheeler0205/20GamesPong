using Game.Ball;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Paddle
{

    /// <summary>
    /// A basic AI controller to play Pong. The AI will compare its Y position with the Y position of the pong ball, and move up and down
    /// accordingly. The AI controller will not have a speed of 1 in order to have a degree of imperfection.
    /// </summary>
    public class AIControllerBasic : PaddleController
    {
        /// <summary>
        /// Reference to the ball to track and follow
        /// </summary>
        private BallBase _ballRef = null;

        /// <summary>
        /// The maximum margin of error the paddle the AI controlls can have from the center of the paddle to be considered in the same Y
        /// position as the ball it's tracking.
        /// </summary>
        private const float MAX_BALL_MARGIN = 30.0f;
        
        /// <summary>
        /// The distance from the center of the paddle that the AI will currently aim for. This changes before the paddle starts moving again.
        /// </summary>
        private float _ballMargin = 0.0f;

        /// <summary>
        /// The max speed the paddle controlled by the AI will move. It should never be greater than or equal to 1.0
        /// </summary>
        private const float MAX_SPEED = 0.85f;

        /// <summary>
        /// The vector used to tell the paddle which direction to move. Set determined by the paddle the AI is controlling and the ball's
        /// Y position.
        /// </summary>
        private Vector2 _direction = Vector2.Zero;

        private Timer _mistakeTimer;
        private bool _makeMistake = false;

        public override void _Ready()
        {
            _mistakeTimer = GetNode<Timer>("%MistakeTimer");
            foreach(Node node in GetTree().GetNodesInGroup("Ball"))
            {
                if(node is BallBase @base)
                {
                    _ballRef = @base;
                    break;
                }
            }
            if(!_mistakeTimer.IsValid()) { return; }

            _mistakeTimer.Start();
            GD.Randomize();
        }

        public override void _Process(float delta)
        {
            // Only set the direction if we have a ball to track and a paddle to move.
            if (_ballRef.IsValid() && PaddleToControl.IsValid() && !_makeMistake) { SetDirection(); }

            if (_makeMistake)
            {
                _makeMistake = false;
                _mistakeTimer.Start();
            }
        }

        /// <summary>
        /// Compare the paddle this controller is controlling's Y position with the ball it's tracking's Y position. If the y position of the
        /// ball is above the y position of the paddle - the margin of error, the paddle should move up. If the y position of the ball is
        /// below the Y position of the paddle + the margin of error, the paddle should move down. Otherwise, the paddle shouldn't move at all.
        /// </summary>
        private void SetDirection()
        {
            // Only set the direction if the ball is moving toward the paddle.
            if (_ballRef.IsMoving && (PaddleToControl.Position.x * _ballRef.Direction.x) > 0)
            {
                float paddleYPosition = PaddleToControl.Position.y;
                float ballYPosition = _ballRef.Position.y;
                // Case 1: The ball is above the paddle. Move up.
                if(paddleYPosition - _ballMargin > ballYPosition)
                {
                    _direction = new Vector2(0.0f, -MAX_SPEED);
                }
                // Case 2: The ball is below the paddle. Move down.
                else if(paddleYPosition + _ballMargin < ballYPosition)
                {
                    _direction = new Vector2(0.0f, MAX_SPEED);
                }
                else
                {
                    _direction = Vector2.Zero;
                }
                PaddleToControl.SetDirection(_direction);
            }
            else
            {
                _ballMargin = (float)GD.RandRange(0.0, MAX_BALL_MARGIN);
                _direction = Vector2.Zero;
                PaddleToControl.SetDirection(_direction);
            }
        }

        public void OnMistakerTimerTimeout()
        {
            _makeMistake = true;
        }
    }
}