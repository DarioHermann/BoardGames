namespace BoardGames.Areas.TicTacToe.Models
{
    public class Board
    {
        /// <summary>
        /// Number of pieces placed on the board
        /// </summary>
        private int TotalPiecesPlaced { get; set; }

        /// <summary>
        /// Pieces on the board
        /// </summary>
        public string[,] Pieces { get; }

        public Board()
        {
            Pieces = new string[3, 3];
        }

        /// <summary>
        /// Checks if there's a three in a row (winning play)
        /// If so, returns a true
        /// </summary>
        public bool IsThreeInRow
        {
            get
            {
                for (int row = 0; row < Pieces.GetLength(0); row++)
                {
                    if (!string.IsNullOrEmpty(Pieces[row, 0]) &&
                        Pieces[row, 0] == Pieces[row, 1] &&
                        Pieces[row, 0] == Pieces[row, 2])
                    {
                        return true;
                    }
                }

                for (int col = 0; col < Pieces.GetLength(1); col++)
                {
                    if (!string.IsNullOrEmpty(Pieces[0, col]) &&
                        Pieces[0, col] == Pieces[1, col] &&
                        Pieces[0, col] == Pieces[2, col])
                    {
                        return true;
                    }
                }

                if (!string.IsNullOrEmpty(Pieces[0, 0]) &&
                    Pieces[0, 0] == Pieces[1, 1] &&
                    Pieces[0, 0] == Pieces[2, 2])
                {
                    return true;
                }

                if (!string.IsNullOrEmpty(Pieces[0, 2]) &&
                    Pieces[0, 2] == Pieces[1, 1] &&
                    Pieces[0, 2] == Pieces[2, 0])
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if there are still spaces left to play
        /// </summary>
        public bool AreSpacesLeft => TotalPiecesPlaced < Pieces.Length;

        /// <summary>
        /// Method to place a Piece in the board
        /// </summary>
        /// <param name="row">row of piece placed</param>
        /// <param name="col">column of piece placed</param>
        /// <param name="piece">piece placed</param>
        public void PlacePiece(int row, int col, string piece)
        {
            Pieces[row, col] = piece;
            TotalPiecesPlaced++;
        }

        public override string ToString()
        {
            return string.Join(", ", Pieces);
        }
    }
}