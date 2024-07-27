using Godot;
using System;

namespace Game.UI
{



    public class PlayButton : ButtonBase
    {
        [Signal]
        delegate void PlayGame();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            SetButton();
        }

        private void OnPlayButtonPressed()
        {
            EmitSignal(nameof(PlayGame));
        }
    }
}