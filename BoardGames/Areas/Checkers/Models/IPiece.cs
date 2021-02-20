namespace BoardGames.Areas.Checkers.Models
{
    public interface IPiece
    {
        string GetColor();
        bool IsKing();
        int Row();
        int Col();
        void Move(int row, int col);
        void TurnedKing();
    }
}