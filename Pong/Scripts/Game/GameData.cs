
using System.Windows.Markup;

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
    /// <summary>
    /// Static class holding data to be used throughout the Pong game
    /// Note: I don't really like using globals, especially since only a select few classes need access to this data specifically,
    /// but this is also quick and shouldn't change at all.
    /// </summary>
    public static class GameData
    {
        private static GameMode _gameMode = GameMode.TWO_PLAYER_GAME;
        public static GameMode Mode
        {
            get => _gameMode;
            set { _gameMode = value; }
        }
    }
}