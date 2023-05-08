using GameStore.Core.Entities;

namespace GameStore.Core.Interfaces
{
    public interface ICategoryService
    {
        OperationResult<Category> Create(Category category);

        OperationResult<Category> GetById(int id);
    }
}
