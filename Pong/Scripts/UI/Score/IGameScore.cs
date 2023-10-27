namespace Game.UI
{
    /// <summary>
    /// Interface for classes that deal with storing and keeping track of the game's score
    /// </summary>
    public interface IGameScore
    {
        /// <summary>
        /// Resets the game's score as defined by the class that implements this interface.
        /// </summary>
        void ResetScore();
    }
}