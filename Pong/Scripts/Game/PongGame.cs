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
            if (_ballManager == null)
            {
                GD.PrintErr("Pong ball manager not found! Is it not in the scene?");
            }

            _paddleManager = GetNode<IGameManager>("%PaddleManager");
            if (_paddleManager == null)
            {
                GD.PrintErr("Pong paddle manager not found! Is it not in the scene?");
            }

            _pongScore = GetNode<IGameScore>("%ScoreUI");
            if(_pongScore == null)
            {
                GD.PrintErr("Pong score display not found! Is it not in the scene?");
            }

            _winTextLabel = GetNode<ITextLabel>("%WinLabel");
            if (_winTextLabel == null)
            {
                GD.PrintErr("Win text label not found! Is it not in the scene?");
            }

            StartGame();
        }

        public void StartGame()
        {
            _ballManager?.StartGame();
            _paddleManager?.StartGame();
        }

        public void OnGameOver(Player winningPlayer)
        {
            switch (winningPlayer)
            {
                case Player.PLAYER_ONE:
                    _winTextLabel?.SetText("Player One Wins!");
                    break;
                case Player.PLAYER_TWO:
                    _winTextLabel?.SetText("Player Two Wins!");
                    break;
            }
            _winTextLabel?.DisplayText();

            _ballManager?.EndGame();
            _paddleManager?.EndGame();
        }

        public void OnWinTextTimeout()
        {
            _winTextLabel?.HideText();
            _pongScore?.ResetScore();
            // Return to main menu
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("reset_game"))
            {
                _ballManager?.EndGame();
                _paddleManager?.EndGame();
                _winTextLabel?.HideText();
                _pongScore?.ResetScore();
            }
        }
    }
}