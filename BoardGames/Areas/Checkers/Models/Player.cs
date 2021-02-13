namespace BoardGames.Areas.Checkers.Models
{
    public class Player
    {
        public string Name { get; }
        public string Id { get; }
        public string GameId { get; set; }
        public string Piece { get; set; }

        public IPiece[] Pieces { get; set; }

        public Player(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return string.Format($"(Id={Id}, Name={Name}, GameId={GameId}, Piece={Piece})");
        }
    }
}