﻿using System.Threading.Tasks;
using BoardGames.Areas.Checkers.Models;
using Microsoft.AspNet.SignalR;

namespace BoardGames.Areas.Checkers
{
    public class GameHub : Hub
    {
        /// <summary>
        /// The starting point for a client looking to join a new game.
        /// Player either starts a game with a waiting opponent or joins the waiting pool.
        /// </summary>
        /// <param name="username">username chosen</param>
        /// <returns>A Task to track the asynchronous method execution.</returns>
        public async Task FindGame(string username)
        {
            if (GameState.Instance.IsUsernameTaken(username))
            {
                Clients.Caller.usernameTaken();
                return;
            }

            Player joiningPlayer = GameState.Instance.CreatePlayer(username, Context.ConnectionId);
            Clients.Caller.playerJoined(joiningPlayer);

            // Find pending games if any
            Player opponent = GameState.Instance.GetWaitingOpponent();
            if (opponent == null)
            {
                GameState.Instance.AddToWaitingPool(joiningPlayer);
                Clients.Caller.waitingList();
            }
            else
            {
                Game newGame = await GameState.Instance.CreatedGame(opponent, joiningPlayer);
                Clients.Group(newGame.Id).start(newGame);
            }
        }

        /// <summary>
        /// Client has requested to move a piece down in the following position.
        /// </summary>
        /// <param name="row">The row part of the current position.</param>
        /// <param name="col">The column part of the current position.</param>
        /// <param name="endRow">The row part of the end position.</param>
        /// <param name="endCol">The column part of the end position.</param>
        /// <returns>A Task to track the asynchronous method execution.</returns>
        public void MovePiece(int row, int col, int endRow, int endCol)
        {
            Player playerMakingTurn = GameState.Instance.GetPlayer(Context.ConnectionId);
            Player opponent;
            Game game = GameState.Instance.GetGame(playerMakingTurn, out opponent);

            if (game == null || !game.WhoseTurn.Equals(playerMakingTurn))
            {
                Clients.Caller.notPlayersTurn();
                return;
            }

            //if (!game.IsValidMove(row, col))
            //{
            //    Clients.Caller.notValidMove();
            //    return;
            //}

            //game.MovePiece(row, col);
            //Clients.Group(game.Id).piecePlaced(row, col, playerMakingTurn.Piece);

            //if (!game.IsOver)
            //{
            //    Clients.Group(game.Id).updateTurn(game);
            //}
            //else
            //{
            //    if (game.IsTie)
            //    {
            //        Clients.Group(game.Id).tieGame();
            //    }
            //    else
            //    {
            //        Clients.Group(game.Id).winner(playerMakingTurn.Name);
            //    }

            //    GameState.Instance.RemoveGame(game.Id);
            //}
        }

        /// <summary>
        /// A player that is leaving should end all games and notify the opponent.
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override async Task OnDisconnected(bool stopCalled)
        {
            Player leavingPlayer = GameState.Instance.GetPlayer(Context.ConnectionId);

            if (leavingPlayer != null)
            {
                Player opponent;
                Game ongoingGame = GameState.Instance.GetGame(leavingPlayer, out opponent);
                if (ongoingGame != null)
                {
                    Clients.Group(ongoingGame.Id).opponentLeft();
                    GameState.Instance.RemoveGame(ongoingGame.Id);
                }
            }

            await base.OnDisconnected(stopCalled);
        }
    }
}