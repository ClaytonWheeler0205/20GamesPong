using Game.UI;
using Godot;

namespace Game
{
    public class PongGame : Node
    {
        private IGameManager _ballManager;
        private IGameManager _paddleManager;
        private IGameScore _pongScore;
        private ITextLabel _winTextLabel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _ballManager = GetNode<IGameManager>("%BallManager");
            if (_ballManager != null)
            {
                _ballManager.StartGame();
            }

            _paddleManager = GetNode<IGameManager>("%PaddleManager");
            if (_paddleManager != null)
            {
                _paddleManager.StartGame();
            }

            _pongScore = GetNode<IGameScore>("%ScoreUI");

            _winTextLabel = GetNode<ITextLabel>("%WinLabel");
            if (_winTextLabel != null)
            {
                _winTextLabel.HideText();
            }
        }

        public void OnGameOver(Player winningPlayer)
        {
            if (_winTextLabel != null)
            {
                switch (winningPlayer)
                {
                    case Player.PLAYER_ONE:
                        _winTextLabel.SetText("Player One Wins!");
                        break;
                    case Player.PLAYER_TWO:
                        _winTextLabel.SetText("Player Two Wins!");
                        break;
                }
                _winTextLabel.DisplayText();
            }
            if (_ballManager != null)
            {
                _ballManager.EndGame();
            }
            if (_paddleManager != null)
            {
                _paddleManager.EndGame();
            }
        }

        public void OnWinTextTimeout()
        {
            if( _winTextLabel != null )
            {
                _winTextLabel.HideText();
            }
            if(_pongScore != null)
            {
                _pongScore.ResetScore();
            }
            // Return to main menu
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("reset_game"))
            {
                if (_ballManager != null)
                {
                    _ballManager.EndGame();
                }
                if (_paddleManager != null)
                {
                    _paddleManager.EndGame();
                }
                if(_winTextLabel != null)
                {
                    _winTextLabel.HideText();
                }
                if (_pongScore != null)
                {
                    _pongScore.ResetScore();
                }
            }
        }
    }
}