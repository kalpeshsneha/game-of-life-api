using ConwayGameOfLife.Core.DTO;
using ConwayGameOfLife.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Core.Services
{
	public interface IBoardService
	{
		Task<Guid> CreateBoardAsync(BoardDto dto);
		Task<BoardDto> GetNextStateAsync(Guid boardId);
		Task<BoardDto> GetXStatesAwayAsync(Guid boardId, int steps);
		Task<BoardDto> GetFinalStateAsync(Guid boardId, int maxNoOfAttempts);

	}
}
