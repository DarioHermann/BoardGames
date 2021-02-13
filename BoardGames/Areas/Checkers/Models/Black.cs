﻿namespace BoardGames.Areas.Checkers.Models
{
    public class Black : IPiece
    {
        private bool _isKing { get; set; }
        private int _row { get; set; }
        private int _column { get; set; }

        public Black(int row, int col)
        {
            _isKing = false;
            _row = row;
            _column = col;
        }

        public string GetColor()
        {
            return "Black";
        }

        public bool IsKing()
        {
            return _isKing;
        }

        public int Row()
        {
            return _row;
        }

        public int Col()
        {
            return _column;
        }

        public void Move(int row, int col)
        {
            _row = row;
            _column = col;
        }

        public void TurnedKing()
        {
            _isKing = true;
        }
    }
}