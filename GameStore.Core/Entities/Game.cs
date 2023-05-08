namespace GameStore.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Developer { get; set; }
        public GameMode GameMode { get; set; }
        public int AvailableCopies { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
    public enum GameMode
    {
        SinglePlayer,
        MultiPlayer,
    }
}
