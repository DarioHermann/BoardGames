using System.Threading.Tasks;
using BoardGames.Areas.TicTacToe.Models;
using Microsoft.AspNet.SignalR;

namespace BoardGames.Areas.TicTacToe
{
    public class GameHub : Hub
    {
        public async Task FindGame(string username)
        {
            if (GameState.Instance.IsUsernameTaken(username))
            {
                Clients.Caller.usernameTakes();
                return;
            }

            Player joiningPlayer = GameState.Instance.CreatePlayer(username, Context.ConnectionId);
            Clients.Caller.playerJoined(joiningPlayer);

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

        public void PlacePiece(int row, int col)
        {
            Player playerMakingTurn = GameState.Instance.GetPlayer(Context.ConnectionId);
            Player opponent;
            Game game = GameState.Instance.GetGame(playerMakingTurn, out opponent);

            if (game == null || !game.WhoseTurn.Equals(playerMakingTurn))
            {
                Clients.Caller.notPlayersTurn();
                return;
            }

            if (!game.IsValidMove(row, col))
            {
                Clients.Caller.notValidMove();
                return;
            }

            game.PlacePiece(row, col);
            Clients.Group(game.Id).piecePlaced(row, col, playerMakingTurn.Piece);

            if (!game.IsOver)
            {
                Clients.Group(game.Id).updateTurn(game);
            }
            else
            {
                if (game.IsTie)
                {
                    Clients.Group(game.Id).tieGame();
                }
                else
                {
                    Clients.Group(game.Id).winner(playerMakingTurn.Name);
                }

                GameState.Instance.RemoveGame(game.Id);
            }
        }

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