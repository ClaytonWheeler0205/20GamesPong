using Godot;
using System;

namespace Game.Paddle
{
    /// <summary>
    /// Abstract base class of the two paddle controllers in pong. This class stores the information about the paddle it is controlling.
    /// Concrete implementations will have their own logic on how exactly they make the paddle move.
    /// </summary>
    public abstract class PaddleController : Node
    {
        // Member Variables
        [Export] //NOTE: Designer should't have to set the paddle to control in the inspector. This is only for testing purposes.
        private NodePath _paddlePath;
        protected PongPaddle PaddleToControl;

        // NOTE: Using the ready function in this way is for testing putposes only.
        // The paddle controller should not be directly setting the paddle it controls.
        public override void _Ready()
        {
            if(_paddlePath != null)
            {
                PaddleToControl = GetNode<PongPaddle>(_paddlePath);
            }
        }

        public void SetPaddleToControl(PongPaddle Paddle)
        {
            PaddleToControl = Paddle;
        }
    }
}