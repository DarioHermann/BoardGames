namespace BoardGames.Areas.Checkers.Models
{
    public class Board
    {
        public char[,] Pieces { get; }

        public Board()
        {
            Pieces = CreateNewBoard();
        }

        private static char[,] CreateNewBoard()
        {
            var pieces = new char[8, 8];
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (row + column % 2 == 1)
                    {
                        pieces[row, column] = 'b';
                        continue;
                    }

                    pieces[row, column] = 'E';
                }
            }

            for (int row = 3; row < 5; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (row + column % 2 == 1)
                    {
                        pieces[row, column] = ' ';
                        continue;
                    }

                    pieces[row, column] = 'E';
                }
            }

            for (int row = 5; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (row + column % 2 == 1)
                    {
                        pieces[row, column] = 'w';
                        continue;
                    }

                    pieces[row, column] = 'E';
                }
            }

            return pieces;
        }

        public void MovePiece(int startRow, int startCol, int endRow, int endCol)
        {
            Pieces[endRow, endCol] = Pieces[startRow, startCol];
            Pieces[startRow, startCol] = ' ';
        }

        public void EatPiece(int row, int col, int endRow, int endCol)
        {
            var enemyRow = row > endRow ? row - 1 : row + 1;
            var enemyCol = col > endCol ? col - 1 : col + 1;
            Pieces[enemyRow, enemyCol] = ' ';
            MovePiece(row, col, endRow, endCol);
        }
    }
}