using Godot;
using System;

namespace Game.Paddle
{

    public abstract class PaddleBase : KinematicBody2D
    {
        // Member variables
        [Export]
        private float _speed = 25000.0f;
        private Vector2 _velocity = Vector2.Zero;
        private const int PADDLE_SPRITE_SIZE = 16;
        protected int PaddleSize => PADDLE_SPRITE_SIZE;

        // abstract methods
        public abstract float GetPaddleSize();

        public void SetDirection(Vector2 newDirection)
        {
            _velocity = newDirection;
        }

        public override void _PhysicsProcess(float delta)
        {
            MoveAndSlide(_velocity * _speed * delta);
        }
    }
}