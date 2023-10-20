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
        [Export]
        private NodePath _leftPaddlePath = null;
        private PaddleBase _leftPaddleRef = null;
        [Export]
        private NodePath _rightPaddlePath = null;
        private PaddleBase _rightPaddleRef = null;

        // node references to the paddle controllers
        private PaddleController _leftPaddleController = null;
        private PaddleController _rightPaddleController = null;
                    
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

        public void SetupLeftPaddle()
        {
            if(_leftPaddlePath == null)
            {
                GD.PrintErr("Left paddle node path not found! Has it not been assigned in the inspector?");
            }
            else
            {
                _leftPaddleRef = GetNode<PaddleBase>(_leftPaddlePath);
                if(_leftPaddleRef == null)
                {
                    GD.PrintErr("Left paddle node not found! Has it been moved or deleted?");
                }
            }
        }

        public void SetupRightPaddle()
        {
            if (_rightPaddlePath == null)
            {
                GD.PrintErr("Left paddle node path not found! Has it not been assigned in the inspector?");
            }
            else
            {
                _rightPaddleRef = GetNode<PaddleBase>(_rightPaddlePath);
                if (_rightPaddleRef == null)
                {
                    GD.PrintErr("Left paddle node not found! Has it been moved or deleted?");
                }
            }
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