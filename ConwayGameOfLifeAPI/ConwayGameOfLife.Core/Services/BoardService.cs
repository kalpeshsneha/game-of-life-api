using ConwayGameOfLife.Infrastructure.Models;
using ConwayGameOfLife.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Core.Services
{
	public class BoardService : IBoardService
	{
		private readonly IBoardRepository _boardRepository;
		public BoardService(IBoardRepository boardRepository)
		{
			_boardRepository = boardRepository;
		}

		public async Task<Guid> AddBoardAsync(Board board)
		{
			return await _boardRepository.AddBoardAsync(board);
		}

		public async Task<Board> GetBoardByIdAsync(Guid id)
		{
			return await _boardRepository.GetBoardByIdAsync(id);
		}
	}
}
