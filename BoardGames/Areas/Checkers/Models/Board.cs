namespace BoardGames.Areas.Checkers.Models
{
    public class Board
    {
        public string[,] Pieces { get; }

        public Board()
        {
            Pieces = CreateNewBoard();
        }

        private static string[,] CreateNewBoard()
        {
            var pieces = new string[8, 8];
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if ((row + column) % 2 == 1)
                    {
                        pieces[row, column] = "b";
                        continue;
                    }

                    pieces[row, column] = " ";
                }
            }

            for (int row = 3; row < 5; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if ((row + column) % 2 == 1)
                    {
                        pieces[row, column] = "";
                        continue;
                    }

                    pieces[row, column] = " ";
                }
            }

            for (int row = 5; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if ((row + column) % 2 == 1)
                    {
                        pieces[row, column] = "w";
                        continue;
                    }

                    pieces[row, column] = " ";
                }
            }

            return pieces;
        }

        public string MovePiece(int startRow, int startCol, int endRow, int endCol, bool isKing)
        {
            var piece = isKing ? Pieces[startRow, startCol].ToUpper() : Pieces[startRow, startCol];
            Pieces[endRow, endCol] = piece;
            Pieces[startRow, startCol] = "";

            return piece;
        }

        public string EatPiece(int row, int col, int endRow, int endCol, bool isKing)
        {
            var enemyRow = row > endRow ? row - 1 : row + 1;
            var enemyCol = col > endCol ? col - 1 : col + 1;
            Pieces[enemyRow, enemyCol] = "";
            return MovePiece(row, col, endRow, endCol, isKing);
        }
    }
}