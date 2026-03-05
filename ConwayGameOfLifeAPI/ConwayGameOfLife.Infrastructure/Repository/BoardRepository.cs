using ConwayGameOfLife.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Infrastructure.Repository
{
	public class BoardRepository : IBoardRepository
	{
		private readonly GameOfLifeDBContext _context;

		public BoardRepository(GameOfLifeDBContext context)
		{
			_context = context;
		}

		#region Public Methods
		public async Task<Board> GetBoardAsync(Guid id)
		{
			return await _context.Boards.Include(b => b.Cells).FirstOrDefaultAsync(b => b.Id == id);
		}

		public async Task AddBoardAsync(Board board)
		{
			try
			{
				await _context.Boards.AddAsync(board);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
		#endregion
	}
}
