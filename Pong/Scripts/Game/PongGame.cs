using Game.Score;
using Godot;

namespace Game
{
    public class PongGame : Node
    {
        private IGameManager _ballManager;
        private IGameManager _paddleManager;
        private IGameScore _pongScore;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _ballManager = GetNode<IGameManager>("Managers/BallManager");
            _ballManager.StartGame();
            _paddleManager = GetNode<IGameManager>("Managers/PaddleManager");
            _paddleManager.StartGame();
            _pongScore = GetNode<IGameScore>("ScoreUI");
        }

        public void OnGameOver(Player winningPlayer)
        {
            switch (winningPlayer)
            {
                case Player.PLAYER_ONE:
                    GD.Print("Player One wins!!!");
                    break;
                case Player.PLAYER_TWO:
                    GD.Print("Player Two wins!!!");
                    break;
            }
            _ballManager.EndGame();
            _paddleManager.EndGame();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("reset_game"))
            {
                _ballManager.EndGame();
                _paddleManager.EndGame();
                _pongScore.ResetScore();
            }
        }
    }
}