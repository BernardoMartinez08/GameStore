using GameStore.Api.DataTransferObjects.GamesDataTransferObjects;

namespace GameStore.Api.DataTransferObjects.CategoryDataTransferObjects
{
    public class CategoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GameDetailsDto> Games { get; set; } = new HashSet<GameDetailsDto>();
    }
}
