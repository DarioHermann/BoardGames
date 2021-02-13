using System;

namespace BoardGames.Areas.Checkers.Models
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

            Player1.Pieces = SetPlayerPieces("Blacks");
            Player2.Pieces = SetPlayerPieces("Whites");
        }

        /// <summary>
        /// Returns which player's turn it currently is
        /// </summary>
        public Player WhoseTurn => IsFirstPlayersTurn ? Player1 : Player2;

        /// <summary>
        /// Players move a piece on the board
        /// </summary>
        /// <param name="row">start row of piece moved</param>
        /// <param name="col">start column of piece moved</param>
        /// <param name="endRow">end row of piece moved</param>
        /// <param name="endCol">end column of piece moved</param>
        public void MovePiece(int row, int col, int endRow, int endCol)
        {


            Board.MovePiece(row, col, endRow, endCol);

            IsFirstPlayersTurn = !IsFirstPlayersTurn;
        }

        /// <summary>
        /// Player eats an opponents piece and jumps one house over
        /// </summary>
        /// <param name="row">start row of piece moved</param>
        /// <param name="col">start column of piece moved</param>
        /// <param name="endRow">end row of piece moved</param>
        /// <param name="endCol">end column of piece moved</param>
        public void EatPiece(int row, int col, int endRow, int endCol)
        {
            Board.EatPiece(row, col, endRow, endCol);
        }

        private IPiece[] SetPlayerPieces(string color)
        {
            if (color.Equals("Whites"))
            {
                var pieces = new White[12];
                int i = 0;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (row + col % 2 == 1)
                        {
                            pieces[i++] = new White(row, col);
                        }
                    }
                }

                return pieces;
            }
            else
            {
                var pieces = new Black[12];
                int i = 0;
                for (int row = 5; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (row + col % 2 == 1)
                        {
                            pieces[i++] = new Black(row, col);
                        }
                    }
                }

                return pieces;
            }
        }
    }
}