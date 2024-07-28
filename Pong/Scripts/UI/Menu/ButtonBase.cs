using Godot;
using System;

namespace Game.UI
{

    public abstract class ButtonBase : TextureButton
    {
        PackedScene _menuMoveSFX;
        PackedScene _menuToggleSFX;
        Node _root;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Connect("focus_exited", this, nameof(OnFocusExit));
            Connect("pressed", this, nameof(OnButtonPressed));
            _menuMoveSFX = GD.Load<PackedScene>("res://Pong/Scenes/FX/SFX_MenuMove.tscn");
            _menuToggleSFX = GD.Load<PackedScene>("res://Pong/Scenes/FX/SFX_MenuToggle.tscn");
            _root = GetNode("/root");
        }

        public void SetButton()
        {
            GrabFocus();
        }

        public void UnsetButton()
        {
            ReleaseFocus();
        }

        protected void OnFocusExit()
        {
            if (IsVisibleInTree())
            {
                _root.CallDeferred("add_child", _menuMoveSFX.Instance());
            }
        }

        protected void OnButtonPressed()
        {
            _root.CallDeferred("add_child", _menuToggleSFX.Instance());
        }
    }
}