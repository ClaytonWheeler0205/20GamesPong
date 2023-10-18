using Godot;
using System;

namespace Game.Paddle
{
    /// <summary>
    /// Class representing a Pong paddle in game. Controls the paddle's movement based on input from a controller class.
    /// </summary>
    public class PongPaddle : PaddleBase
    {
        // Member variables
        private float _paddleHeight = 0.0f;

        public override void _Ready()
        {
            _paddleHeight = PaddleSize * Transform.Scale.y;
        }

        public override float GetPaddleSize()
        {
            return _paddleHeight;
        }
    }
}