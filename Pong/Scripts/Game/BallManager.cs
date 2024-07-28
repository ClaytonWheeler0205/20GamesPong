using Godot;
using Game.Ball;
using Util.ExtensionMethods;

namespace Game
{
    public class BallManager : Node, IGameManager
    {
        /// <summary>
        /// Object reference to the ball that this manager will be managing
        /// </summary>
        private BallBase _ballRef = null;

        /// <summary>
        /// Object reference to the timer that will pause the ball movement between rounds.
        /// </summary>
        private Timer _timerRef = null;

        /// <summary>
        /// How long the timer should run until the ball should move once again.
        /// </summary>
        [Export]
        private float _pauseTime = 3.0f;

        private bool _isActive = false;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Get the timer child node reference
            _timerRef = GetNode<Timer>("%PauseTimer");
            if(!_timerRef.IsValid())
            {
                GD.PrintErr("Timer obejct not found! Is it not in the scene?");
            }

            // Get the ball child node reference
            _ballRef = GetNode<BallBase>("%Ball");
            if(!_ballRef.IsValid())
            {
                GD.PrintErr("Ball object not found! Is it not in the scene?");
            }
            
        }
        
        public bool StartGame()
        {
            if(!_ballRef.IsValid()) { return false; }

            _ballRef.StartBall();
            _isActive = true;
            return true;
        }

        public bool EndGame()
        {
            if(!_ballRef.IsValid()) { return false; }

            _ballRef.ResetBall();
            _ballRef.ReadyBall();
            _isActive = false;
            return true;
        }

        public void OnBallGoalHit()
        {
            if(!_timerRef.IsValid()) { return; }

            _timerRef.Start(_pauseTime);
        }

        public void OnPauseTimerTimeout()
        {
            if (!_isActive) { return; }

            if (!_ballRef.IsValid()) { return; }

            _ballRef.StartBall();
        }
    }
}