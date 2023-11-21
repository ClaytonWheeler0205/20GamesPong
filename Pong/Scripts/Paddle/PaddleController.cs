using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Paddle
{
    /// <summary>
    /// Abstract base class of the two paddle controllers in pong. This class stores the information about the paddle it is controlling.
    /// Concrete implementations will have their own logic on how exactly they make the paddle move.
    /// </summary>
    public abstract class PaddleController : Node
    {
        // Member Variables
        private PaddleBase _paddleToControl;
        protected PaddleBase PaddleToControl => _paddleToControl;

        public void SetPaddleToControl(PaddleBase Paddle)
        {
            _paddleToControl = Paddle;
        }

        public void Destroy()
        {
            this.SafeQueueFree();
            _paddleToControl = null;
        }
    }
}