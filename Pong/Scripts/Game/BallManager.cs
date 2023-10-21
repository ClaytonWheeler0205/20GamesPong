using Godot;
using Game.Ball;

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

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _timerRef = GetNode<Timer>("PauseTimer");
            if(_timerRef == null)
            {
                GD.PrintErr("Timer obejct not found! Is it not in the scene?");
            }
            _ballRef = GetNode<BallBase>("Ball");
            if(_ballRef == null)
            {
                GD.PrintErr("Ball object not found! Is it not in the scene?");
            }
            
        }
        
        public bool StartGame()
        {
            if(_ballRef != null)
            {
                _ballRef.StartBall();
                return true;
            }
            return false;
        }

        public bool EndGame()
        {
            if(_ballRef != null)
            {
                _ballRef.ResetBall();
                return true;
            }
            return false;
        }

        public void OnBallGoalHit()
        {
            _timerRef.Start(_pauseTime);
        }

        public void OnPauseTimerTimeout()
        {
            _ballRef.StartBall();
        }
    }
}