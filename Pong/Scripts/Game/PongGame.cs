using Godot;
using System;

namespace Game
{
    public enum Player
    {
        PLAYER_ONE,
        PLAYER_TWO
    }

    public enum GameMode
    {
        ONE_PLAYER_GAME,
        TWO_PLAYER_GAME
    }

    public class PongGame : Node
    {

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }
    }
}