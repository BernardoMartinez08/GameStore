using GameStore.Core.Interfaces;
using GameStore.Core;
using Error = GameStore.Core.Error;
using GameStore.Core.Entities;

namespace GameStore.Infrastructure
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Game> gameRepository;
        private readonly IRepository<Category> categoryRepository;

        public CategoryService(IRepository<Game> gameRepository, IRepository<Category> categoryRepository)
        {
            this.gameRepository = gameRepository;
            this.categoryRepository = categoryRepository;
        }
        
        public OperationResult<Category> Create(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                return new OperationResult<Category>(new Error
                {
                    Code = ErrorCode.BadRequest,
                    Message = "La categoria debe tener un nombre."
                });
            }

            categoryRepository.Add(category);
            return new OperationResult<Category>(category);
        }

        public OperationResult<Category> GetById(int id)
        {
            var category = this.categoryRepository.GetById(id);
            if (category is null)
            {
                return new OperationResult<Category>(new Error
                {
                    Code = ErrorCode.BadRequest,
                    Message = $"No se encontró la categoria con el id {id}"
                });
            }

            return category;
        }
    }
}