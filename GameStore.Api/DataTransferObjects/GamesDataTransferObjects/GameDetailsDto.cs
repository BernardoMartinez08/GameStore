using System;

namespace GameStore.Api.DataTransferObjects.GamesDataTransferObjects
{
    public class GameDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Developer { get; set; }
        public  GameMode GameMode { get; set; }
        public int AvailableCopies { get; set; }
    }
}
