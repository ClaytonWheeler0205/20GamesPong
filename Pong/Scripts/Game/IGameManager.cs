
namespace Game
{
    public interface IGameManager
    {
        /// <summary>
        /// Sets up the objects that the manager is responsible to be used in play
        /// </summary>
        /// <returns>True if the manager has successfully started, and false if otherwise</returns>
        bool StartGame();

        /// <summary>
        /// Resets or cleans up objects that the manage is responsible for after a game has ended. (Note, a game ending does not necessarily
        /// mean that the program has terminated.)
        /// </summary>
        /// <returns></returns>
        bool EndGame();
    }
}