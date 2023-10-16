using Godot;
using System;

namespace Game.Ball
{

    public class Ball : KinematicBody2D
    {
        // Member variables
        [Export]
        private float _speed = 10000.0f;
        private Vector2 _direction = Vector2.Zero;
        private bool _isMoving = false;
        private Sprite _spriteRef = null;
        [Export]
        private Vector2 _centerPos;

        // Signals
        [Signal]
        delegate void PlayerScored(Player scoringPlayer);

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        public override void _PhysicsProcess(float delta)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetBall()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void StartBall()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveBall()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collision"></param>
        private void HandleCollision(KinematicCollision2D collision)
        {

        }
    }
}