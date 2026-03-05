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
		Task<Board> GetBoardAsync(Guid id);
		Task AddBoardAsync(Board board);
		Task SaveChangesAsync();
	}
}
