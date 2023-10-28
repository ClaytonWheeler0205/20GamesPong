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
            // Get a reference to the left paddle
            _leftPaddleRef = GetNode<PaddleBase>("%LeftPaddle");
            if(_leftPaddleRef == null)
            {
                GD.PrintErr("Left paddle reference not found! Is it not in the scene?");
            }

            // Get a reference to the right paddle
            _rightPaddleRef = GetNode<PaddleBase>("%RightPaddle");
            if(_rightPaddleRef == null)
            {
                GD.PrintErr("Right paddle reference not found! Is it not in the scene?");
            }
        }

        public bool StartGame()
        {
            CreateControllers();
            if (_leftPaddleController == null || _rightPaddleController == null)
            {
                return false;
            }
            
            // Assign our controllers paddles they will be controlling in the game
            _rightPaddleController.SetPaddleToControl(_rightPaddleRef);
            _leftPaddleController.SetPaddleToControl(_leftPaddleRef);
            return true;
        }

        public bool EndGame()
        {
            bool isSuccessful = true;
            // Attempt to destroy and cleanup the left paddle controller
            if(_leftPaddleController == null)
            {
                isSuccessful = false;
            }
            else
            {
                _leftPaddleController.Destroy();
                _leftPaddleController = null;
            }

            // Attempt to destroy and cleanup the right paddle controller
            if(_rightPaddleController == null)
            {
                isSuccessful = false;
            }
            else
            {
                _rightPaddleController.Destroy();
                _rightPaddleController = null;
            }

            // Reset our pong paddles to their starting state
            _rightPaddleRef?.ResetPaddle();
            _leftPaddleRef?.ResetPaddle();

            return isSuccessful;
        }

        /// <summary>
        /// Creates the pong paddle controllers, according to the current game mode. (ie. if it's a one or two player game)
        /// </summary>
        private void CreateControllers()
        {
            // The player one paddle will always be controlled by the player
            // Load the player controller scene, create an instance of it, and add it as a child
            PackedScene playerPaddle = GD.Load<PackedScene>("res://Pong/Scenes/Paddle/PlayerController.tscn");
            _rightPaddleController = playerPaddle.Instance() as PaddleController;
            AddChild(_rightPaddleController);

            // Determine what kind of controller should be created according to the game mode
            switch(GameData.Mode)
            {
                case GameMode.ONE_PLAYER_GAME:
                    PackedScene aiPaddle = GD.Load<PackedScene>("res://Pong/Scenes/Paddle/AIControllerBasic.tscn");
                    _leftPaddleController = aiPaddle.Instance() as PaddleController;
                    AddChild(_leftPaddleController);
                    break;

                case GameMode.TWO_PLAYER_GAME:
                    _leftPaddleController = playerPaddle.Instance() as PaddleController;
                    AddChild( _leftPaddleController);
                    break;
            }
        }
    }
}