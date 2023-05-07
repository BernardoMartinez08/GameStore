using GameStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Core.Interfaces
{
    internal interface ICategoryService
    {
        OperationResult<Game> Create(Game game);

        OperationResult<Game> GetById(int id);
    }
}
