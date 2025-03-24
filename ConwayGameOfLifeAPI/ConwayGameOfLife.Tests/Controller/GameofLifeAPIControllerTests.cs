using ConwayGameOfLife.Core.Services;
using ConwayGameofLife.Service.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayGameOfLife.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ConwayGameOfLife.Core.DTO.Helper;

namespace ConwayGameOfLife.Tests.Controller
{
	public class GameofLifeAPIControllerTests
	{
		private readonly Mock<IBoardService> _boardServiceMock;
		private readonly Mock<IErrorService> _errorServiceMock;
		private readonly GameofLifeAPIController _controller;

		public GameofLifeAPIControllerTests()
		{
			_boardServiceMock = new Mock<IBoardService>();
			_errorServiceMock = new Mock<IErrorService>();
			_controller = new GameofLifeAPIController(_boardServiceMock.Object, _errorServiceMock.Object);
		}

		[Fact]
		public async Task CreateBoardState_ShouldReturnOk_WhenBoardIsValid()
		{
			
			var board = new BoardDto
			{
				Name = "Test",
				Cells = new List<CellDto> { new CellDto { Row = 0, Column = 0, IsAlive = true } }
			};

			_boardServiceMock.Setup(s => s.CreateBoardAsync(board)).ReturnsAsync(Guid.NewGuid());

			
			var result = await _controller.CreateBoardState(board);

		
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.IsType<Guid>(okResult.Value);
		}

		[Fact]
		public async Task CreateBoardState_ShouldReturnBadRequest_WhenBoardIsNull()
		{
		
			_errorServiceMock.Setup(e => e.CreateError((int)HttpStatusCode.BadRequest, It.IsAny<string>()))
							 .Returns(new ErrorDto { StatusCode = 400, Details = ErrorsConstants.NullBoardStateMgs});

			
			var result = await _controller.CreateBoardState(null);

			
			var badRequest = Assert.IsType<BadRequestObjectResult>(result);
			var error = Assert.IsType<ErrorDto>(badRequest.Value);
			Assert.Equal(400, error.StatusCode) ;
		}

		[Fact]
		public async Task GetNextBoardState_ShouldReturnOk_WhenBoardExists()
		{

			var boardId = Guid.NewGuid();
			var board = new BoardDto { Name = "Next", Cells = new List<CellDto>() };

			_boardServiceMock.Setup(s => s.GetNextStateAsync(boardId)).ReturnsAsync(board);

		
			var result = await _controller.GetNextBoardState(boardId);

		
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(board, okResult.Value);
		}

		[Fact]
		public async Task GetNextBoardState_ShouldReturnNotFound_WhenBoardIsNull()
		{
		
			var boardId = Guid.NewGuid();
			_boardServiceMock.Setup(s => s.GetNextStateAsync(boardId)).ReturnsAsync((BoardDto)null);

			_errorServiceMock.Setup(e => e.CreateError((int)HttpStatusCode.NotFound, It.IsAny<string>()))
							 .Returns(new ErrorDto { StatusCode = 404, Details = ErrorsConstants.BoardNotFoundMgs });

		
			var result = await _controller.GetNextBoardState(boardId);

			// Assert
			var notFound = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal(404, notFound.StatusCode);
		}

		[Fact]
		public async Task GetXStatesAway_ShouldReturnOk()
		{
			
			var boardId = Guid.NewGuid();
			var board = new BoardDto { Name = "Future", Cells = new List<CellDto>() };

			_boardServiceMock.Setup(s => s.GetXStatesAwayAsync(boardId, 3)).ReturnsAsync(board);

			
			var result = await _controller.GetXStatesAway(boardId, 3);

		
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(board, okResult.Value);
		}

		[Fact]
		public async Task GetFinalState_ShouldReturnOk_WhenStable()
		{
			
			var boardId = Guid.NewGuid();
			var board = new BoardDto { Name = "Final", Cells = new List<CellDto>() };

			_boardServiceMock.Setup(s => s.GetFinalStateAsync(boardId, 10)).ReturnsAsync(board);

		
			var result = await _controller.GetFinalState(boardId, 10);

		
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(board, okResult.Value);
		}

		[Fact]
		public async Task GetFinalState_ShouldReturnBadRequest_WhenExceptionThrown()
		{
			// Arrange
			var boardId = Guid.NewGuid();

			_boardServiceMock.Setup(s => s.GetFinalStateAsync(boardId, 1))
							 .ThrowsAsync(new InvalidOperationException());

			_errorServiceMock.Setup(e => e.CreateError((int)HttpStatusCode.InternalServerError, It.IsAny<string>()))
							 .Returns(new ErrorDto { StatusCode = 500, Details = ErrorsConstants.InternalServerErrorMgs });

			// Act
			var result = await _controller.GetFinalState(boardId, 1);

			// Assert
			var badRequest = Assert.IsType<BadRequestObjectResult>(result);
			var error = Assert.IsType<ErrorDto>(badRequest.Value);
			Assert.Equal(500, error.StatusCode);
		}

	}
}
