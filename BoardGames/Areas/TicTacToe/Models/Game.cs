using System;

namespace BoardGames.Areas.TicTacToe.Models
{
    public class Game
    {
        private bool IsFirstPlayersTurn { get; set; }

        public string Id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Board Board { get; set; }


        public Game(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
            Id = Guid.NewGuid().ToString("d");
            Board = new Board();

            IsFirstPlayersTurn = true;

            Player1.GameId = Id;
            Player2.GameId = Id;

            Player1.Piece = "X";
            Player2.Piece = "O";
        }

        /// <summary>
        /// Returns which player's turn it currently is
        /// </summary>
        public Player WhoseTurn => IsFirstPlayersTurn ? Player1 : Player2;

        /// <summary>
        /// Checks if there's a Tie
        /// </summary>
        public bool IsTie => !Board.AreSpacesLeft && !Board.IsThreeInRow;

        /// <summary>
        /// Checks if the game is over
        /// </summary>
        public bool IsOver => IsTie || Board.IsThreeInRow;

        /// <summary>
        /// Players places a piece of the board
        /// </summary>
        /// <param name="row">row of piece placed</param>
        /// <param name="col">column of piece placed</param>
        public void PlacePiece(int row, int col)
        {
            var piece = IsFirstPlayersTurn ? Player1.Piece : Player2.Piece;
            Board.PlacePiece(row, col, piece);

            IsFirstPlayersTurn = !IsFirstPlayersTurn;
        }

        /// <summary>
        /// Checks if a move is valid
        /// </summary>
        /// <param name="row">row of tried move</param>
        /// <param name="col">column of tired move</param>
        /// <returns></returns>
        public bool IsValidMove(int row, int col)
        {
            return row < Board.Pieces.GetLength(0) &&
                   col < Board.Pieces.GetLength(1) &&
                   string.IsNullOrWhiteSpace(Board.Pieces[row, col]);
        }

        public override string ToString()
        {
            return string.Format($"(Id={Id}, Player1={Player1}, Player2={Player2}, Board={Board}");
        }
    }
}