using Godot;
using Util.ExtensionMethods;

namespace Game.UI
{
    public class TextLabelBasic : Control, ITextLabel
    {
        // Child node references
        private Label _textLabelRef = null;
        private Timer _displayTimer = null;

        [Export]
        private float _displayDuration = 5.0f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Get the text label reference
            _textLabelRef = GetNode<Label>("%WinText");
            if(!_textLabelRef.IsValid())
            {
                GD.PrintErr("Text label not found! Is it not in the scene?");
            }

            // Get the display timer reference
            _displayTimer = GetNode<Timer>("%DisplayTimer");
            if(!_displayTimer.IsValid())
            {
                GD.PrintErr("Display timer not found! Is it not in the scene?");
            }
        }

        public void SetText(string text)
        {
            if (_textLabelRef != null)
            {
                _textLabelRef.Text = text;
            }
        }

        public void DisplayText()
        {
            Visible = true;
            if(_displayTimer.IsValid())
            {
                _displayTimer.Start(_displayDuration);
            }
        }

        public void HideText()
        {
            Visible = false;
            if(_displayTimer.IsValid())
            {
                _displayTimer.Stop();
            }
        }
        
    }
}