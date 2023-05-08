using Microsoft.AspNetCore.Mvc;
using GameStore.Core;
using GameStore.Core.Entities;
using GameStore.Core.Interfaces;
using GameStore.Api.DataTransferObjects.CategoryDataTransferObjects;

namespace GameStore.Api.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Crea una nueva categoria.
        /// </summary>
        /// <param name="category">Los datos de la categoria a agregar.</param>
        /// <returns>Los detalles de la categoria agregada.</returns>
        [HttpPost("[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult CreateCategory([FromBody] CategoryCreateDto category)
        {
            var newCategory = new Category
            {
                Name= category.Name,
            };

            var result = this.categoryService.Create(newCategory);
            if (result.Succeeded)
            {
                return new CreatedAtActionResult(nameof(GetCategoryById), "Category", new { categoryId = result.Result.Id }, category);
            }
            var categoryDetails = new CategoryDetailsDto
            {
                Name = category.Name,
            };

            return GetErrorResult<CategoryDetailsDto>(categoryDetails);
        }

        /// <summary>
        /// Busca una categoria por el id de la misma.
        /// </summary>
        /// <param name="categoryId">El id de la categoria a buscar.</param>
        /// <returns>La categoria entrada para el id.</returns>
        [HttpGet("[controller]/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetCategoryById(int categoryId)
        {
            var result = this.categoryService.GetById(categoryId);
            var categoryDetails = new CategoryDetailsDto
            {
                Name = result.Result.Name,
            };

            if (result.Succeeded)
            {
                return Ok(categoryDetails);
            }
            return GetErrorResult<CategoryDetailsDto>(categoryDetails);
        }

        /// <summary>
        /// Busca el error que ocurrio al tratar de ejecutar una accion.
        /// </summary>
        /// <typeparam name="TResult">La interfaz de resultados</typeparam>
        /// <param name="result">El resultado de la operacion realizada</param>
        /// <returns>El error que se genero al tratar de ejecutar la accion.</returns>
        private ActionResult GetErrorResult<TResult>(OperationResult<TResult> result)
        {
            switch (result.Error.Code)
            {
                case Core.ErrorCode.NotFound:
                    return NotFound(result.Error.Message);
                case Core.ErrorCode.Unauthorized:
                    return Unauthorized(result.Error.Message);
                default:
                    return BadRequest(result.Error.Message);
            }
        }
    }
}
