using Game.Ball;
using Godot;
using System;

namespace Game.Paddle
{

    public class AIControllerBasic : PaddleController
    {
        private BallBase _ballRef = null;

        private const float BALL_MARGIN = 16.0f;

        private const float MAX_SPEED = 0.75f;

        private Vector2 _direction = Vector2.Zero;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        public void SetBallToTrack(BallBase ball)
        {
            _ballRef = ball;
        }

        public override void _Process(float delta)
        {
            
        }

        private void SetDirection()
        {

        }
    }
}