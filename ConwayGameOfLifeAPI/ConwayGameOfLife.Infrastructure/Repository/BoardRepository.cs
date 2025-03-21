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

		public async Task<Guid> AddBoardAsync(Board board)
		{
			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					_context.Boards.Add(board);
					await _context.SaveChangesAsync();

					await transaction.CommitAsync();
					return board.Id;
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					Console.WriteLine("AddBoardAsync: An error occurred - {ex.Message}");
					throw;
				}
			}
		}

		public async Task<Board> GetBoardByIdAsync(Guid id)
		{
			return await _context.Boards.Include(b => b.Cells).FirstOrDefaultAsync(b => b.Id == id);
		}
	}
}
