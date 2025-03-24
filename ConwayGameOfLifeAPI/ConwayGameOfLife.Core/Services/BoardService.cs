using ConwayGameOfLife.Core.DTO;
using ConwayGameOfLife.Infrastructure.Models;
using ConwayGameOfLife.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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

		#region Public Methods
		public async Task<Guid> CreateBoardAsync(BoardDto dto)
		{
			var board = new Board
			{
				Name = dto.Name,
				Cells = dto.Cells.Select(c => new Cell
				{
					RowPosition = c.Row,
					ColumnPosition = c.Column,
					IsAlive = c.IsAlive
				}).ToList()
			};

			await _boardRepository.AddBoardAsync(board);
			await _boardRepository.SaveChangesAsync();
			return board.Id;
		}

		public async Task<BoardDto> GetNextStateAsync(Guid boardId)
		{
			var board = await _boardRepository.GetBoardAsync(boardId);
			if (board == null)
				throw new ArgumentException("Invalid board id", nameof(boardId));

			var nextState = CalculateNextState(board);
			return new BoardDto { Name = nextState.Name, Cells = nextState.Cells.Select(x => new CellDto { Row = x.RowPosition, Column = x.ColumnPosition, IsAlive = x.IsAlive  }).ToList() };
		}

		

		public async Task<BoardDto> GetXStatesAwayAsync(Guid boardId, int steps)
		{
			var board = await _boardRepository.GetBoardAsync(boardId);
			if(board == null)
				throw new ArgumentException("Invalid board id", nameof(boardId));

			var currentBoardState = board;
			for (var i = 0; i < steps; i++)
			{
				currentBoardState = CalculateNextState(currentBoardState);
			}

			var newXStatesAway = new BoardDto { Name = $"{currentBoardState.Name} {steps} states away", Cells = currentBoardState.Cells.Select(x => new CellDto { Row = x.RowPosition, Column = x.ColumnPosition, IsAlive = x.IsAlive }).ToList() };

			return newXStatesAway;
		}

		public async Task<BoardDto> GetFinalStateAsync(Guid boardId, int maxNoOfAttempts)
		{
			var board = await _boardRepository.GetBoardAsync(boardId);
			if (board == null)
				throw new ArgumentException("Invalid board id", nameof(boardId));
			
			var previousState = board;

			for (int i = 0; i < maxNoOfAttempts; i++)
			{
				var nextState = CalculateNextState(previousState);
				{
					if (BoardsAreEqual(previousState.Cells, nextState.Cells))
					{
						// Stable state reached
						return new BoardDto
						{
							Name = $"{nextState.Name} - Final State after {i + 1} steps",
							Cells = nextState.Cells.Select(c => new CellDto
							{
								Row = c.RowPosition,
								Column = c.ColumnPosition,
								IsAlive = c.IsAlive
							}).ToList()
						};
					}

					previousState = nextState;

				}
			}
			throw new InvalidOperationException("Board did not reach a stable state after {maxNoOfAttempts} steps.");
		}
		#endregion 

		#region Private Methods 
		private Board  CalculateNextState(Board board)
		{
			if (board == null)
				throw new ArgumentNullException(nameof(board));

			var rows = board.Cells.Max(c => c.RowPosition) + 1;
			var cols = board.Cells.Max(c => c.ColumnPosition) + 1;
			var currentBoard = new int[rows, cols];

			foreach (var cell in board.Cells)
			{
				currentBoard[cell.RowPosition, cell.ColumnPosition] = cell.IsAlive ? 1 : 0;
			}

			var nextBoard = new int[rows, cols];

			for (var i = 0; i < rows; i++)
			{
				for (var j = 0; j < cols; j++)
				{
					var aliveNeighbors = CountAliveNeighbors(currentBoard, i, j);
					if (currentBoard[i, j] == 1 && (aliveNeighbors < 2 || aliveNeighbors > 3))
					{
						//cell dies
						nextBoard[i, j] = 0; 
					}
					else if (currentBoard[i, j] == 0 && aliveNeighbors == 3)
					{
						// cell becomes alive
						nextBoard[i, j] = 1; 
					}
					else
					{
						// cell remains the same
						nextBoard[i, j] = currentBoard[i, j]; 
					}
				}
			}

			var nextCells = new List<Cell>();
			for (var i = 0; i < rows; i++)
			{
				for (var j = 0; j < cols; j++)
				{
					nextCells.Add(new Cell
					{
						RowPosition = i,
						ColumnPosition = j,
						IsAlive = nextBoard[i, j] == 1,
						BoardId = board.Id
					});
				}
			}

			return new Board
			{
				Id = board.Id,
				Name = board.Name,
				Cells = nextCells
			};
		}

		private int CountAliveNeighbors(int[,] board, int rowIndex, int colIndex)
		{
			var aliveNeighbors = 0;
			var rows = board.GetLength(0);
			var cols = board.GetLength(1);

			var rowStart = Math.Max(0, rowIndex - 1);
			var rowEnd = Math.Min(rows - 1, rowIndex + 1);
			var colStart = Math.Max(0, colIndex - 1);
			var colEnd = Math.Min(cols - 1, colIndex + 1);

			for (var i = rowStart; i <= rowEnd; i++)
			{
				for (var j = colStart; j <= colEnd; j++)
				{
					if (i == rowIndex && j == colIndex)
					{
						continue;
					}
					aliveNeighbors += board[i, j];
				}
			}

			return aliveNeighbors;
		}

		private bool BoardsAreEqual(IEnumerable<Cell> previousStateCells, IEnumerable<Cell> nextStateCells)
		{
			if (previousStateCells == null || nextStateCells == null)
				throw new ArgumentNullException(previousStateCells == null ? nameof(previousStateCells) : nameof(nextStateCells));

			var listP= previousStateCells.OrderBy(c => c.RowPosition).ThenBy(c => c.ColumnPosition).ToList();
			var listN = nextStateCells.OrderBy(c => c.RowPosition).ThenBy(c => c.ColumnPosition).ToList();

			if (listP.Count != listN.Count)
				return false;

			for (int i = 0; i < listP.Count; i++)
			{
				if (listP[i].RowPosition != listN[i].RowPosition ||
					listP[i].ColumnPosition != listN[i].ColumnPosition ||
					listP[i].IsAlive != listN[i].IsAlive)
				{
					return false;
				}
			}

			return true;
		}


		#endregion
	}
}