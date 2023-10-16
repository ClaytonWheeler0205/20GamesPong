using Godot;
using System;

namespace Game.Paddle
{
    /// <summary>
    /// Class representing a Pong paddle in game. Controls the paddle's movement based on input from a controller class.
    /// </summary>
    public class Paddle : KinematicBody2D
    {
        // Member variables
        [Export]
        private float _speed = 25000.0f;
        private Vector2 _velocity;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Set the velocity vector's initial state to the zero vector.
            _velocity = Vector2.Zero;
        }

        /// <summary>
        /// Sets the direction vector to a new value, specified by the controller class
        /// </summary>
        /// <param name="newDirection">The new vector the paddle shall move along.</param>
        public void SetDirection(Vector2 newDirection)
        {
            _velocity = newDirection;
        }

        public override void _PhysicsProcess(float delta)
        {
            MoveAndSlide(_velocity * _speed * delta);
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}