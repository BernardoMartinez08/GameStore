using Microsoft.AspNetCore.Mvc;
using GameStore.Core;
using GameStore.Core.Entities;
using GameStore.Core.Interfaces;
using GameStore.Api.DataTransferObjects.CategoryDataTransferObjects;
using GameStore.Api.DataTransferObjects.GamesDataTransferObjects;
using GameMode = GameStore.Api.DataTransferObjects.GamesDataTransferObjects.GameMode;

namespace GameStore.Api.Controllers
{
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;
        private readonly ICategoryService categoryService;

        public GameController(ICategoryService categoryService, IGameService gameService)
        {
            this.categoryService = categoryService;
            this.gameService = gameService;
        }

        /// <summary>
        /// Agregá un nuevo video juego para una categoria.
        /// </summary>
        /// <param name="categoryId">El id de categoria a la que sera agregado el juego.</param>
        /// <param name="game">El video juego que sera agregado.</param>
        /// <returns>Los detalles del juego agregado.</returns>
        [HttpPost("/category/{categoryId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult CreateGame([FromRoute] int categoryId,[FromBody] GameCreateDto game)
        {
            var category = categoryService.GetById(categoryId);
            if(!category.Succeeded)
            {
                return GetErrorResult<Category>(category);
            }

            var newGame = new Game
            {
                Name = game.Name,
                PublishDate = game.PublishDate,
                Developer = game.Developer,
                GameMode = (Core.Entities.GameMode)game.GameMode,
                AvailableCopies = game.AvailableCopies,
                CategoryId = categoryId,
            };

            var gameDetails = new GameDetailsDto
            {
                Name = game.Name,
                PublishDate = game.PublishDate,
                Developer = game.Developer,
                GameMode = (GameMode)game.GameMode,
                AvailableCopies = game.AvailableCopies,
            };

            var result = this.gameService.Create(newGame);
            if (result.Succeeded)
            {
                return new CreatedAtActionResult(nameof(GetGameById), "GameDetailsDto", new { gameId = result.Result.Id }, game);
            }
            return GetErrorResult<GameDetailsDto>(gameDetails);
        }

        /// <summary>
        /// Busca un juego filtado por el id del mismo.
        /// </summary>
        /// <param name="gameId">El id del juego a buscar.</param>
        /// <returns>El juego con el id enviado si existe, si no retorna un mensaje de error.</returns>
        [HttpGet("[controller]/{gameId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetGameById([FromRoute] int gameId)
        {
            var result = this.gameService.GetById(gameId);
            if (result.Succeeded)
            {
                return Ok(result.Result);
            }

            var gameDetails = new GameDetailsDto
            {
                Id = gameId,
                Name = result.Result.Name,
                PublishDate = result.Result.PublishDate,
                Developer = result.Result.Developer,
                GameMode = (GameMode)result.Result.GameMode,
                AvailableCopies = result.Result.AvailableCopies,

            };
            return GetErrorResult<GameDetailsDto>(gameDetails);
        }

        /// <summary>
        /// Busca los juegos para una categoria.
        /// </summary>
        /// <param name="categoryId">EL id de la categoria a buscar.</param>
        /// <returns>Una lista de juegos con el id de categoria enviado si existe, si no retorna un mensaje de error.</returns>
        [HttpGet("category/{categoryId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetGameByCategory([FromRoute] int categoryId)
        {
            var result = this.gameService.GetByCategory(categoryId);
            if (result.Succeeded)
            {
                return Ok(result.Result);
            }

            IReadOnlyList<GameDetailsDto> gamesDetails = result.Result.Select(game => new GameDetailsDto
            {
                Id = game.Id,
                Name = game.Name,
                PublishDate = game.PublishDate,
                Developer = game.Developer,
                AvailableCopies = game.AvailableCopies,
                GameMode = (GameMode)game.GameMode,
            }).ToList();

            return GetErrorResult<GameDetailsDto>(gamesDetails.First());
        }

        /// <summary>
        /// Buscar los juegos para un Modo de Juego.
        /// </summary>
        /// <param name="gameMode">El modo de juego para filtrar la busqueda</param>
        /// <returns>Una lista de juegos con el Modo de Juego enviado si existe alguno, si no retorna un mensaje de error.</returns>
        [HttpGet("/gamemodes/{gameMode}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetGameByGameMode([FromQuery] GameMode gameMode)
        {
            var result = this.gameService.GetByGameMode((Core.Entities.GameMode)gameMode);
            if (result.Succeeded)
            {
                return Ok(result.Result);
            }

            IReadOnlyList<GameDetailsDto> gamesDetails = result.Result.Select(game => new GameDetailsDto
            {
                Id = game.Id,
                Name = game.Name,
                PublishDate = game.PublishDate,
                Developer = game.Developer,
                AvailableCopies = game.AvailableCopies,
                GameMode = (GameMode)game.GameMode,
            }).ToList();

            return GetErrorResult<GameDetailsDto>(gamesDetails.First());
        }

        /// <summary>
        /// Presta un video juego.
        /// </summary>
        /// <param name="gameId">El id del juego a prestar o rentar.</param>
        /// <returns>Los detalles del juego que se rento</returns>
        [HttpPut("[controller]/{gameId}/rent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RentGame([FromRoute] int gameId)
        {
            var result = this.gameService.RentGame(gameId);
            if (result.Succeeded)
            {
                return Ok(result.Result);
            }
            var gameDetails = new GameDetailsDto
            {
                Id = gameId,
                Name = result.Result.Name,
                PublishDate = result.Result.PublishDate,
                Developer = result.Result.Developer,
                GameMode = (GameMode)result.Result.GameMode,
                AvailableCopies = result.Result.AvailableCopies,
            };
            return GetErrorResult<GameDetailsDto>(gameDetails);
        }

        /// <summary>
        /// Retorna un video juego a la libreria.
        /// </summary>
        /// <param name="gameId">El juego que esta siendo devuelto.</param>
        /// <returns>Los detalles del video juego que fue devuelto.</returns>
        [HttpPut("[controller]/{gameId}/return")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult ReturnGame([FromRoute] int gameId)
        {
            var result = this.gameService.ReturnGame(gameId);
            if (result.Succeeded)
            {
                return Ok(result.Result);
            }
            var gameDetails = new GameDetailsDto
            {
                Id = gameId,
                Name = result.Result.Name,
                PublishDate = result.Result.PublishDate,
                Developer = result.Result.Developer,
                GameMode = (GameMode)result.Result.GameMode,
                AvailableCopies = result.Result.AvailableCopies,
            };
            return GetErrorResult<GameDetailsDto>(gameDetails);
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
