using Godot;
using System;

namespace Game.UI
{

    public class ModeSelectButton : ButtonBase
    {

        private Label _uiLabel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            _uiLabel = GetNode<Label>("%ModeSelect");
        }

        private void OnModeSelectButtonPressed()
        {
            switch (GameData.Mode)
            {
                case GameMode.ONE_PLAYER_GAME:
                    _uiLabel.Text = "2P";
                    GameData.Mode = GameMode.TWO_PLAYER_GAME;
                    break;
                case GameMode.TWO_PLAYER_GAME:
                    _uiLabel.Text = "1P";
                    GameData.Mode = GameMode.ONE_PLAYER_GAME;
                    break;
            }
        }

    }
}