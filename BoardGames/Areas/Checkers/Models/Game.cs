﻿using System;
using System.Collections.Generic;
using System.Web.WebPages;

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

            Player1.Piece = "b";
            Player2.Piece = "w";

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
            if (IsFirstPlayersTurn)
            {
                Player1.MovePiece(row, col, endRow, endCol);
            }
            else
            {
                Player2.MovePiece(row, col, endRow, endCol);
            }

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
            if (IsFirstPlayersTurn)
            {
                Player1.MovePiece(row, col, endRow, endCol);
                Player2.PieceEaten(row > endRow ? row - 1 : row + 1, col > endCol ? col - 1 : col + 1);
            }
            else
            {
                Player2.MovePiece(row, col, endRow, endCol);
                Player1.PieceEaten(row > endRow ? row - 1 : row + 1, col > endCol ? col - 1 : col + 1);
            }

            Board.EatPiece(row, col, endRow, endCol);
        }

        private List<IPiece> SetPlayerPieces(string color)
        {
            var pieces = new List<IPiece>();
            if (color.Equals("Whites"))
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if ((row + col) % 2 == 1)
                        {
                            pieces.Add(new White(row, col));
                        }
                    }
                }
            }
            else
            {
                for (int row = 5; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if ((row + col) % 2 == 1)
                        {
                            pieces.Add(new Black(row, col));
                        }
                    }
                }
            }

            return pieces;
        }

        /// <summary>
        /// Checks valid moves for the piece in a certain position
        /// </summary>
        /// <param name="row">row of piece to check</param>
        /// <param name="col">column of piece to check</param>
        /// <returns>List of valid moves for a certain piece</returns>
        public List<int[]> ShowValidMovesForPiece(int row, int col)
        {
            var moves = new List<int[]>();

            string piece = Board.Pieces[row, col];

            switch (piece)
            {
                case "b":
                    moves.Add(new []{1, -1});
                    moves.Add(new []{1, 1});
                    break;
                case "B":
                case "W":
                    moves.Add(new[] {-1, -1});
                    moves.Add(new[] {-1, 1});
                    moves.Add(new[] {1, -1});
                    moves.Add(new[] {1, 1});
                    break;
                case "w":
                    moves.Add(new[] {-1, -1 });
                    moves.Add(new[] {-1, 1 });
                    break;
            }

            var validMoves = new List<int[]>();

            foreach (var move in moves)
            {
                if ((row + move[0] < 0 || row + move[0] > 8) || (col + move[1] < 0 || col + move[1] > 0))
                {
                    continue;
                }

                string otherPiece = Board.Pieces[row + move[0], row + move[1]].ToLower();

                if (otherPiece.IsEmpty())
                {
                    validMoves.Add(new []{row+move[0], col+move[1]});
                }
                else if (otherPiece.Equals(piece.ToLower()))
                {
                    // Do Nothing
                }
                else
                {
                    if ((row + move[0] * 2 < 0 || row + move[0] * 2 > 8) ||
                        (col + move[1] * 2 < 0 || col + move[1] * 2 > 0))
                    {
                        continue;
                    }

                    var thirdPiece = Board.Pieces[row + move[0] * 2, col + move[1] * 2];
                    if (thirdPiece.IsEmpty())
                    {
                        validMoves.Add(new []{row+move[0], col+move[1]});
                    }
                }
            }

            return validMoves;
        }

        public bool IsCurrentPlayersPiece(int row, int col)
        {
            if(IsFirstPlayersTurn && Board.Pieces[row, col].ToLower().Equals(Player1.Piece))
            {
                return true;
            }
            if (!IsFirstPlayersTurn && Board.Pieces[row, col].ToLower().Equals(Player2.Piece))
            {
                return true;
            }

            return false;
        }
    }
}