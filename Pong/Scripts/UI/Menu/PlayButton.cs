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
            SetPlayButton();
        }

        public void SetPlayButton()
        {
            GrabFocus();
        }

        private void OnPlayButtonPressed()
        {
            GD.Print("Play game!");
            EmitSignal(nameof(PlayGame));
        }
    }
}