using GameStore.Core.Entities;

namespace GameStore.Core.Interfaces
{
    public interface IGameService
    {
        OperationResult<Game> Create(Game game);
        OperationResult<Game> GetById(int id);
        OperationResult<IReadOnlyList<Game>> GetByCategory(int categoryId);
        OperationResult<IReadOnlyList<Game>> GetByGameMode(Entities.GameMode gamemode);
        OperationResult<IReadOnlyList<Game>> GetRentedGames();
        OperationResult<Game> RentGame(int id);
        OperationResult<Game> ReturnGame(int id);
    }
}
