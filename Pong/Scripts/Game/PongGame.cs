using Godot;

namespace Game
{
    public class PongGame : Node
    {

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            IGameManager ballManager = GetNode<IGameManager>("Managers/BallManager");
            ballManager.StartGame();
            IGameManager paddleManager = GetNode<IGameManager>("Managers/PaddleManager");
            paddleManager.StartGame();
        }
    }
}