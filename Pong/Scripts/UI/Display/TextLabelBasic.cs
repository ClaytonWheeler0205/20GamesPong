using Godot;
using System;

namespace Game.UI
{
    public class TextLabelBasic : CanvasLayer, ITextLabel
    {
        // Child node references
        private Label _textLabelRef = null;
        private Timer _displayTimer = null;

        [Export]
        private float _displayDuration = 5.0f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _textLabelRef = GetNode<Label>("%WinText");
            _displayTimer = GetNode<Timer>("%DisplayTimer");
        }

        public void SetText(string text)
        {
            _textLabelRef.Text = text;
        }

        public void DisplayText()
        {
            Visible = true;
            if(_displayTimer != null)
            {
                _displayTimer.Start(_displayDuration);
            }
        }

        public void HideText()
        {
            Visible = false;
            if(_displayTimer != null)
            {
                _displayTimer.Stop();
            }
        }
        
    }
}