using System;
using System.Collections.Generic;

namespace BoardGames.Areas.Checkers.Models
{
    public class Player
    {
        public string Name { get; }
        public string Id { get; }
        public string GameId { get; set; }
        public string Piece { get; set; }

        public List<IPiece> Pieces { get; set; }

        public Player(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return string.Format($"(Id={Id}, Name={Name}, GameId={GameId}, Piece={Piece})");
        }

        public bool MovePiece(int row, int col, int endRow, int endCol)
        {
            foreach (var piece in Pieces)
            {
                if (piece.Row() == row && piece.Col() == col)
                {
                    piece.Move(endRow, endCol);
                    return piece.IsKing();
                }
            }

            throw new Exception();
        }

        public void PieceEaten(int row, int col)
        {
            foreach (var piece in Pieces)
            {
                if (piece.Row() == row && piece.Col() == col)
                {
                    Pieces.Remove(piece);
                    return;
                }
            }

            //for (int i = 0; i < Pieces.Count; i++)
            //{
            //    if (Pieces[i].Row() == row && Pieces[i].Col() == col)
            //    {
            //        Pieces.RemoveAt(i);
            //    }
            //}
        }
    }
}