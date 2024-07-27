using Godot;
using System;

namespace Game.UI
{

    public class ScoreGoalButton : ButtonBase
    {
        private Label _uiLabel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            _uiLabel = GetNode<Label>("%ScoreGoal");
        }

        private void OnScoreGoalButtonPressed()
        {
            GameData.ScoreToWin += 2;
            if (GameData.ScoreToWin > 11)
            {
                GameData.ScoreToWin = 3;
            }
            _uiLabel.Text = $"{GameData.ScoreToWin}";
        }

    }
}