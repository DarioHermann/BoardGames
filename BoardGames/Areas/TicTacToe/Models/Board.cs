namespace BoardGames.Areas.TicTacToe.Models
{
    public class Board
    {
        private int TotalPiecesPlaced { get; set; }

        public string[,] Pieces { get; private set; }

        public Board()
        {
            Pieces = new string[3, 3];
        }

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

        public bool AreSpacesLeft => TotalPiecesPlaced < Pieces.Length;

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