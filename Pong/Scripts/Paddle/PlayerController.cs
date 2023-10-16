using Godot;
using System;

namespace Game.Paddle
{
    /// <summary>
    /// Implementation of the PaddleController abstract class to be used by the player
    /// Gets keyboard input from the player and sets the movement direction for the paddle this controller is controlling
    /// </summary>
    public class PlayerController : PaddleController
    {
        // Member variables.
        //NOTE: Exporting this variable is for testing purposes only. Normally, the PongGame script should set this value
        // when is creates this node
        [Export]
        private Player _player;
        private Vector2 _direction;

        public override void _Ready()
        {
            //NOTE: call the base here only for testing purposes (see the ready function in PaddleController for context)
            base._Ready();
            _direction = Vector2.Zero;
        }

        // We use the unhandled input method to ensure that menu actions take priority in the game.
        public override void _UnhandledInput(InputEvent @event)
        {
            switch(_player)
            {
                case Player.PLAYER_ONE:
                    _direction.y = Input.GetAxis("player1_move_up", "player1_move_down");
                    break;
                case Player.PLAYER_TWO:
                    _direction.y = Input.GetAxis("player2_move_up", "player2_move_down");
                    break;
            }
            PaddleToControl.SetDirection(_direction);
        }

        public void SetPlayer(Player controllingPlayer)
        {
            _player = controllingPlayer;
        }
    }
}