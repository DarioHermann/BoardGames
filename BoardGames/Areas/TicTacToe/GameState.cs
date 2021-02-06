using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using BoardGames.Areas.TicTacToe.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BoardGames.Areas.TicTacToe
{
    public class GameState
    {
        public IHubConnectionContext<dynamic> Clients { get; set; }
        public IGroupManager Groups { get; set; }

        private GameState(IHubContext context)
        {
            Clients = context.Clients;
            Groups = context.Groups;
        }

        private static readonly Lazy<GameState> instance = new Lazy<GameState>(() => new GameState(GlobalHost.ConnectionManager.GetHubContext<GameHub>()));

        private readonly ConcurrentDictionary<string, Player> players = new ConcurrentDictionary<string, Player>(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentDictionary<string, Game> games = new ConcurrentDictionary<string, Game>(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentQueue<Player> waitingPlayers = new ConcurrentQueue<Player>();

        public static GameState Instance => instance.Value;

        public Player CreatePlayer(string username, string connectionId)
        {
            var player = new Player(username, connectionId);
            players[connectionId] = player;

            return player;
        }

        public Player GetPlayer(string playerId)
        {
            if (!players.TryGetValue(playerId, out var foundPlayer))
            {
                return null;
            }

            return foundPlayer;
        }

        public Game GetGame(Player player, out Player opponent)
        {
            opponent = null;
            Game foundGame = games.Values.FirstOrDefault(g => g.Id == player.GameId);

            if (foundGame == null)
            {
                return null;
            }

            opponent = player.Id == foundGame.Player1.Id ? foundGame.Player2 : foundGame.Player1;

            return foundGame;
        }

        public Player GetWaitingOpponent()
        {
            if (!waitingPlayers.TryDequeue(out var foundPlayer))
            {
                return null;
            }

            return foundPlayer;
        }

        public void RemoveGame(string gameId)
        {
            if (!games.TryRemove(gameId, out var foundGame))
            {
                throw new InvalidOperationException("Game not found.");
            }

            // Remove the players
            players.TryRemove(foundGame.Player1.Id, out _);
            players.TryRemove(foundGame.Player2.Id, out _);
        }

        public void AddToWaitingPool(Player player)
        {
            waitingPlayers.Enqueue(player);
        }

        public bool IsUsernameTaken(string username)
        {
            return players.Values.FirstOrDefault(player =>
                player.Name.Equals(username, StringComparison.InvariantCultureIgnoreCase)) != null;
        }

        public async Task<Game> CreatedGame(Player firstPlayer, Player secondPlayer)
        {
            Game game = new Game(firstPlayer, secondPlayer);
            games[game.Id] = game;

            await Groups.Add(firstPlayer.Id, game.Id);
            await Groups.Add(secondPlayer.Id, game.Id);

            return game;
        }
    }
}