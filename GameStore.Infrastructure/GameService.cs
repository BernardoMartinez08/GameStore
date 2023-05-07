using GameStore.Core.Interfaces;
using GameStore.Core;
using Error = GameStore.Core.Error;
using GameStore.Core.Entities;
using GameStore.Api.DataTransferObjects.GamesDataTransferObjects;
using GameMode = GameStore.Core.Entities.GameMode;
using Microsoft.OpenApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SocialNetwork.Infrastructure;
public class GameService : IGameService
{
    private readonly IRepository<Game> gameRepository;
    private readonly IRepository<Category> categoryRepository;

    public GameService(IRepository<Game> gameRepository, IRepository<Category> categoryRepository)
    {
        this.gameRepository = gameRepository;
        this.categoryRepository = categoryRepository;        
    }

    public OperationResult<Game> Create(GameCreateDto game)
    {
        if (string.IsNullOrEmpty(game.Name))
        {
            return new OperationResult<Game>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = "El juego debe tener un nombre."
            });
        }
        
        if (string.IsNullOrEmpty(game.Developer))
        {
            return new OperationResult<Game>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = "El juego debe tener el nombre del desarrollador."
            });
        }

        if (game.AvailableCopies <= 0)
        {
            return new OperationResult<Game>(new Error
            {
                Code = ErrorCode.NotFound,
                Message = "Debe ingresar un juego con al menos una copia disponible."
            });
        }

        var newGame = new Game
        {
            Name = game.Name,
            PublishDate = game.PublishDate,
            Developer = game.Developer,
            GameMode = (GameMode)game.GameMode,
            AvailableCopies = game.AvailableCopies,

        };

        gameRepository.Add(newGame);
        return new OperationResult<Game>(newGame);
    }

    public OperationResult<GameDetailsDto> GetById(int id)
    {
        var game = this.gameRepository.GetById(id);
        if (game is null)
        {
            return new OperationResult<GameDetailsDto>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = $"No se encontró un juego con el id {id}"
            });
        }

        var gameDetails = new GameDetailsDto
        {
            Id = game.Id,
            Name = game.Name,
            PublishDate = game.PublishDate,
            Developer = game.Developer,
            AvailableCopies = game.AvailableCopies,
            GameMode = (GameStore.Api.DataTransferObjects.GamesDataTransferObjects.GameMode)game.GameMode,
        };

        return gameDetails;
    }

    public OperationResult<IReadOnlyList<GameDetailsDto>> GetByCategory(int categoryId)
    {
        var category = this.categoryRepository.GetById(categoryId);
        if (category is null)
        {
            return new OperationResult<IReadOnlyList<GameDetailsDto>>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = $"No se encontró un post con id {categoryId}"
            });
        }

        var games = this.gameRepository.Filter(x => x.CategoryId == categoryId);
        if (games is null)
        {
            return new OperationResult<IReadOnlyList<GameDetailsDto>>(new Error
            {
                Code = ErrorCode.NotFound,
                Message = $"No se encontró ningun juego con el id de categoria {categoryId}"
            });
        }

        IReadOnlyList<GameDetailsDto> gameDetails = games.Select(game => new GameDetailsDto
        {
            Id = game.Id,
            Name = game.Name,
            PublishDate = game.PublishDate,
            Developer = game.Developer,
            AvailableCopies = game.AvailableCopies,
            GameMode = (GameStore.Api.DataTransferObjects.GamesDataTransferObjects.GameMode)game.GameMode,
        }).ToList();


        return new OperationResult<IReadOnlyList<GameDetailsDto>>(gameDetails);
    }

    public OperationResult<IReadOnlyList<GameDetailsDto>> GetByGameMode(GameMode gamemode)
    {
        var games = this.gameRepository.Filter(x => x.GameMode == gamemode);
        if (games is null)
        {
            return new OperationResult<IReadOnlyList<GameDetailsDto>>(new Error
            {
                Code = ErrorCode.NotFound,
                Message = $"No se encontró ningun juego que tenga modo de juego {gamemode.GetDisplayName()}"
            });
        }

        IReadOnlyList<GameDetailsDto> gameDetails = games.Select(game => new GameDetailsDto
        {
            Id= game.Id,
            Name = game.Name,
            PublishDate = game.PublishDate,
            Developer = game.Developer,
            AvailableCopies = game.AvailableCopies,
            GameMode = (GameStore.Api.DataTransferObjects.GamesDataTransferObjects.GameMode)game.GameMode,
        }).ToList();


        return new OperationResult<IReadOnlyList<GameDetailsDto>>(gameDetails);
    }

    public OperationResult<Game> RentGame(int id)
    {
        var game = this.gameRepository.GetById(id);
        if (game is null)
        {
            return new OperationResult<Game>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = $"No se encontró un juego con el id {id}"
            });
        }

        if (game.AvailableCopies <= 3)
        {
            return new OperationResult<Game>(new Error
            {
                Code = ErrorCode.NotFound,
                Message = "No hay suficientes copias disponibles."
            });
        }

        var newGame = game;
        newGame.AvailableCopies = game.AvailableCopies - 1;

        gameRepository.Update(newGame);
        return new OperationResult<Game>(newGame);
    }

    public OperationResult<Game> ReturnGame(int id)
    {
        var game = this.gameRepository.GetById(id);
        if (game is null)
        {
            return new OperationResult<Game>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = $"No se encontró un juego con el id {id}"
            });
        }

        var newGame = game;
        newGame.AvailableCopies = game.AvailableCopies + 1;

        gameRepository.Update(newGame);
        return new OperationResult<Game>(newGame);
    }

    OperationResult<IReadOnlyList<GameDetailsDto>> IGameService.GetRentedGames()
    {
        throw new NotImplementedException();
    }
}
