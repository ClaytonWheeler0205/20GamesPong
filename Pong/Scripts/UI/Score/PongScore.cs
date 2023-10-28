using Godot;
using System;
using System.Data.SqlTypes;

namespace Game.UI
{
    /// <summary>
    /// Class that stores the scores for both players in a Pong game. The class will also let the game know when a player has won the game.
    /// </summary>
    public class PongScore : Control, IGameScore
    {
        /// <summary>
        /// Reference to the label node that displays player one's score.
        /// </summary>
        private Label _playerOneLabelRef = null;
        /// <summary>
        /// Integer that holds player one's score, used to display the score in the UI and determine if the player has won.
        /// </summary>
        private int _playerOneScore = 0;
        /// <summary>
        /// Reference to the label node that displays player two's score.
        /// </summary>
        private Label _playerTwoLabelRef = null;
        /// <summary>
        /// Integer that holds player two's score, used to display the score in the UI and determine if the player has won.
        /// </summary>
        private int _playerTwoScore = 0;

        /// <summary>
        /// Signal to let the game know that a player has scored the number of points needed to win the game
        /// </summary>
        /// <param name="winningPlayer">The player that has won the game</param>
        [Signal]
        public delegate void GameOver(Player winningPlayer);


        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _playerOneLabelRef = GetNode<Label>("%PlayerOneScore");
            if(_playerOneLabelRef == null)
            {
                GD.PrintErr("Player one score label not found! Is it in the scene tree? If so, is the node path incorrect?");
            }
            _playerTwoLabelRef = GetNode<Label>("%PlayerTwoScore");
            if(_playerTwoLabelRef == null)
            {
                GD.PrintErr("Player two score label not found! Is it in the scene tree? If so, is the node path incorrect?");
            }
            Visible = true;
        }

        public void ResetScore()
        {
            // Reset player one score
            _playerOneScore = 0;
            if (_playerOneLabelRef != null)
            {
                _playerOneLabelRef.Text = _playerOneScore.ToString();
            }

            // Reset player two score
            _playerTwoScore = 0;
            if (_playerTwoLabelRef != null)
            {
                _playerTwoLabelRef.Text = _playerTwoScore.ToString();
            }
        }

        /// <summary>
        /// Updates the player score and score label once a player scores a point. Also checks if a player has gotten enough points
        /// to win the game.
        /// </summary>
        /// <param name="scoringPlayer">The player that has scored the point</param>
        public void OnPlayerScored(Player scoringPlayer)
        {
            switch (scoringPlayer)
            {
                case Player.PLAYER_ONE:
                    _playerOneScore++;
                    if (_playerOneLabelRef != null)
                    {
                        _playerOneLabelRef.Text = _playerOneScore.ToString();
                    }
                    if(_playerOneScore == GameData.ScoreToWin)
                    {
                        EmitSignal("GameOver", Player.PLAYER_ONE);
                    }
                    break;

                case Player.PLAYER_TWO:
                    _playerTwoScore++;
                    if (_playerTwoLabelRef != null)
                    {
                        _playerTwoLabelRef.Text = _playerTwoScore.ToString();
                    }
                    if(_playerTwoScore == GameData.ScoreToWin)
                    {
                        EmitSignal("GameOver", Player.PLAYER_TWO);
                    }
                    break;
            }
        }

    }
}