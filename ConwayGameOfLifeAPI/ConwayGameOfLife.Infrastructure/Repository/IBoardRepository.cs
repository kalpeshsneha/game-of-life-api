using ConwayGameOfLife.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Infrastructure.Repository
{
	public interface IBoardRepository
	{
		Task<Guid> AddBoardAsync(Board board);
		Task<Board> GetBoardByIdAsync(Guid id);
	}
}
