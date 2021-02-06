namespace BoardGames.Areas.TicTacToe.Models
{
    public class Player
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public string GameId { get; set; }
        public string Piece { get; set; }

        public Player(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return string.Format($"(Id={Id}, Name={Name}, GameId={GameId}, Piece={Piece})");
        }

        public override bool Equals(object obj)
        {
            Player other = obj as Player;

            if (other == null)
            {
                return false;
            }

            return Id.Equals(other.Id) && Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * Name.GetHashCode();
        }
    }
}