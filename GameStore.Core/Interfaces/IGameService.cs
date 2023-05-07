using GameStore.Api.DataTransferObjects.GamesDataTransferObjects;
using GameStore.Core.Entities;

namespace GameStore.Core.Interfaces
{
    public interface IGameService
    {
        OperationResult<Game> Create(GameCreateDto game);
        OperationResult<GameDetailsDto> GetById(int id);
        OperationResult<IReadOnlyList<GameDetailsDto>> GetByCategory(int categoryId);
        OperationResult<IReadOnlyList<GameDetailsDto>> GetByGameMode(Entities.GameMode gamemode);
        OperationResult<IReadOnlyList<GameDetailsDto>> GetRentedGames();
        OperationResult<Game> RentGame(int id);
        OperationResult<Game> ReturnGame(int id);
    }
}
