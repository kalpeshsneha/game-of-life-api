using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConwayGameOfLife.Core.DTO;
using ConwayGameOfLife.Core.Services;
using ConwayGameOfLife.Infrastructure.Models;
using ConwayGameOfLife.Infrastructure.Repository;

namespace ConwayGameOfLife.Tests.Service
{
	public class BoardServiceTest
	{
		private readonly Mock<IBoardRepository> _boardRepositoryMock;
		private readonly BoardService _boardService;

		public BoardServiceTest()
		{
			_boardRepositoryMock = new Mock<IBoardRepository>();
			_boardService = new BoardService(_boardRepositoryMock.Object);
		}

		[Fact]
		public async Task CreateBoardAsync_ShouldReturnBoardId()
		{

			var dto = new BoardDto
			{
				Name = "Test Board",
				Cells = new List<CellDto>
			{
				new CellDto { Row = 0, Column = 0, IsAlive = true },
				new CellDto { Row = 0, Column = 1, IsAlive = false }
			}
			};

			var boardId = Guid.NewGuid();
			_boardRepositoryMock.Setup(r => r.AddBoardAsync(It.IsAny<Board>()))
								.Callback<Board>(b => b.Id = boardId)
								.Returns(Task.CompletedTask);

			_boardRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

		
			var result = await _boardService.CreateBoardAsync(dto);

			Assert.Equal(boardId, result);
		}

		[Fact]
		public async Task GetNextStateAsync_ShouldReturnNextState()
		{
		
			var boardId = Guid.NewGuid();
			var board = new Board
			{
				Id = boardId,
				Name = "Blinker",
				Cells = new List<Cell>
			{
				new Cell { RowPosition = 1, ColumnPosition = 0, IsAlive = true },
				new Cell { RowPosition = 1, ColumnPosition = 1, IsAlive = true },
				new Cell { RowPosition = 1, ColumnPosition = 2, IsAlive = true }
			}
			};

			_boardRepositoryMock.Setup(r => r.GetBoardAsync(boardId)).ReturnsAsync(board);

	
			var result = await _boardService.GetNextStateAsync(boardId);

	
			Assert.Equal("Blinker", result.Name);
			Assert.NotEmpty(result.Cells);
		}

		[Fact]
		public async Task GetXStatesAwayAsync_ShouldReturnCorrectState()
		{
	
			var boardId = Guid.NewGuid();
			var board = new Board
			{
				Id = boardId,
				Name = "Blinker",
				Cells = new List<Cell>
			{
				new Cell { RowPosition = 1, ColumnPosition = 0, IsAlive = true },
				new Cell { RowPosition = 1, ColumnPosition = 1, IsAlive = true },
				new Cell { RowPosition = 1, ColumnPosition = 2, IsAlive = true }
			}
			};

			_boardRepositoryMock.Setup(r => r.GetBoardAsync(boardId)).ReturnsAsync(board);

	
			var result = await _boardService.GetXStatesAwayAsync(boardId, 2);

	
			Assert.Contains("2 states away", result.Name);
			Assert.NotNull(result.Cells);
		}

		[Fact]
		public async Task GetFinalStateAsync_ShouldReturnStableState_WhenBoardStabilizes()
		{
	
			var boardId = Guid.NewGuid();
			var stableBoard = new Board
			{
				Id = boardId,
				Name = "Stable Block",
				Cells = new List<Cell>
				{
					new Cell { RowPosition = 0, ColumnPosition = 0, IsAlive = true },
					new Cell { RowPosition = 0, ColumnPosition = 1, IsAlive = true },
					new Cell { RowPosition = 1, ColumnPosition = 0, IsAlive = true },
					new Cell { RowPosition = 1, ColumnPosition = 1, IsAlive = true }
				}
			};

			var boardRepositoryMock = new Mock<IBoardRepository>();
			boardRepositoryMock.Setup(r => r.GetBoardAsync(boardId)).ReturnsAsync(stableBoard);

			var boardService = new BoardService(boardRepositoryMock.Object);

		
			var result = await boardService.GetFinalStateAsync(boardId, maxNoOfAttempts: 10);

	
			Assert.NotNull(result);
			Assert.Contains("Final State", result.Name);
			Assert.Equal(4, result.Cells.Count);
			Assert.All(result.Cells, c => Assert.True(c.IsAlive)); 
		}

		[Fact]
		public async Task GetFinalStateAsync_ShouldThrowInvalidOperationException_WhenBoardDoesNotStabilize()
		{
		
			var boardId = Guid.NewGuid();
			var maxSteps = 1;

			var oscillatorBoard = new Board
			{
				Id = boardId,
				Name = "Blinker",
				Cells = new List<Cell>
		{
			new Cell { RowPosition = 1, ColumnPosition = 0, IsAlive = true },
			new Cell { RowPosition = 1, ColumnPosition = 1, IsAlive = true },
			new Cell { RowPosition = 1, ColumnPosition = 2, IsAlive = true }
		}
			};

			var boardRepositoryMock = new Mock<IBoardRepository>();
			boardRepositoryMock.Setup(r => r.GetBoardAsync(boardId)).ReturnsAsync(oscillatorBoard);

			var boardService = new BoardService(boardRepositoryMock.Object);

			var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
				boardService.GetFinalStateAsync(boardId, maxSteps));

			Assert.NotEmpty(ex.Message);
		}


	}


}