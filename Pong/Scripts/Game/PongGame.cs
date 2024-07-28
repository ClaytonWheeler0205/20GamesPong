using Game.UI;
using Godot;
using Util.ExtensionMethods;

namespace Game
{
    public class PongGame : Node
    {
        private IGameManager _ballManager;
        private IGameManager _paddleManager;
        private IGameScore _pongScore;
        private ITextLabel _winTextLabel;
        private CanvasItem _mainMenu;
        private ButtonBase _playButton;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _ballManager = GetNode<IGameManager>("%BallManager");
            if (!(_ballManager is Node ballNode) || !ballNode.IsValid())
            {
                GD.PrintErr("Pong ball manager not found! Is it not in the scene?");
            }

            _paddleManager = GetNode<IGameManager>("%PaddleManager");
            if (!(_paddleManager is Node paddleNode) || !paddleNode.IsValid())
            {
                GD.PrintErr("Pong paddle manager not found! Is it not in the scene?");
            }

            _pongScore = GetNode<IGameScore>("%ScoreUI");
            if (!(_pongScore is Node scoreNode) || !scoreNode.IsValid())
            {
                GD.PrintErr("Pong score display not found! Is it not in the scene?");
            }

            _winTextLabel = GetNode<ITextLabel>("%WinLabel");
            if (!(_winTextLabel is Node winNode) || !winNode.IsValid())
            {
                GD.PrintErr("Win text label not found! Is it not in the scene?");
            }

            _mainMenu = GetNode<CanvasItem>("%MainMenu");
            if (!_mainMenu.IsValid())
            {
                GD.PrintErr("Main menu not found! Is it not in the scene?");
            }

            _playButton = GetNode<ButtonBase>("%MainMenu/MenuBorder/MenuBG/VBoxContainer/PlayButton");
            if (!_playButton.IsValid())
            {
                GD.PrintErr("Play Button not found! Is it not in the scene?");
            }
            else
            {
                _playButton.SetButton();
            }


        }

        public void OnGameStartPressed()
        {
            StartGame();
        }

        public void StartGame()
        {
            if ((_ballManager is Node ballNode) && ballNode.IsValid()) { _ballManager.StartGame(); }

            if ((_paddleManager is Node paddleNode) && paddleNode.IsValid()) { _paddleManager.StartGame(); }

            if (_mainMenu.IsValid()) { _mainMenu.Visible = false; }
            if (_playButton.IsValid()) { _playButton.UnsetButton(); }
        }

        public void OnGameOver(Player winningPlayer)
        {
            if (_winTextLabel is Node winNode && winNode.IsValid())
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

            if (_ballManager is Node ballNode && ballNode.IsValid()) { _ballManager.EndGame(); }

            if (_paddleManager is Node paddleNode && paddleNode.IsValid()) { _paddleManager.EndGame(); }
        }

        public void OnWinTextTimeout()
        {
            if (_winTextLabel is Node winNode && winNode.IsValid()) { _winTextLabel.HideText(); }

            if (_pongScore is Node scoreNode && scoreNode.IsValid()) { _pongScore.ResetScore(); }
            // Return to main menu
            if (_mainMenu.IsValid()) { _mainMenu.Visible = true; }
            if (_playButton.IsValid()) { _playButton.SetButton(); }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("reset_game"))
            {
                if (!_mainMenu.Visible)
                {
                    if (_ballManager is Node ballNode && ballNode.IsValid()) { _ballManager.EndGame(); }

                    if (_paddleManager is Node paddleNode && paddleNode.IsValid()) { _paddleManager.EndGame(); }

                    if (_winTextLabel is Node winNode && winNode.IsValid()) { _winTextLabel.HideText(); }

                    if (_pongScore is Node scoreNode && scoreNode.IsValid()) { _pongScore.ResetScore(); }

                    if (_mainMenu.IsValid()) { _mainMenu.Visible = true; }
                    if (_playButton.IsValid()) { _playButton.SetButton(); }
                }
                else
                {
                    GetTree().Quit();
                }
            }
        }
    }
}