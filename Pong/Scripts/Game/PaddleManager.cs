using Game.Paddle;
using Godot;

namespace Game
{

    /// <summary>
    /// Manager class that handles the pong paddles for use in the game. This class will create and assign PaddleControllers on the start of a
    /// game and destroy the controllers at the end of a game of Pong.
    /// </summary>
    public class PaddleManager : Node, IGameManager
    {
        // Node paths and node references to the two paddles
        private PaddleBase _leftPaddleRef = null;
        private PaddleBase _rightPaddleRef = null;

        // node references to the paddle controllers
        private PaddleController _leftPaddleController = null;
        private PaddleController _rightPaddleController = null;

        public override void _Ready()
        {
            _leftPaddleRef = GetNode<PaddleBase>("LeftPaddle");
            _rightPaddleRef = GetNode<PaddleBase>("RightPaddle");
        }

        public bool StartGame()
        {
            CreateControllers();
            if (_leftPaddleController == null || _rightPaddleController == null)
            {
                return false;
            }
            _rightPaddleController.SetPaddleToControl(_rightPaddleRef);
            _leftPaddleController.SetPaddleToControl(_leftPaddleRef);
            return true;
        }

        public bool EndGame()
        {
            bool isSuccessful = true;
            if(_leftPaddleController == null)
            {
                isSuccessful = false;
            }
            else
            {
                _leftPaddleController.Destroy();
                _leftPaddleController = null;
            }

            if(_rightPaddleController == null)
            {
                isSuccessful = false;
            }
            else
            {
                _rightPaddleController.Destroy();
                _rightPaddleController = null;
            }

            return isSuccessful;
        }

        private void CreateControllers()
        {
            PackedScene playerPaddle = GD.Load<PackedScene>("res://Pong/Scenes/Paddle/PlayerController.tscn");
            _rightPaddleController = playerPaddle.Instance() as PaddleController;
            AddChild(_rightPaddleController);
            switch(GameData.Mode)
            {
                case GameMode.ONE_PLAYER_GAME:
                    break;
                case GameMode.TWO_PLAYER_GAME:
                    _leftPaddleController = playerPaddle.Instance() as PaddleController;
                    AddChild( _leftPaddleController);
                    break;
            }
        }
    }
}