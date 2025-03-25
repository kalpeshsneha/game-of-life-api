using ConwayGameOfLife.Core.DTO;
using ConwayGameOfLife.Core.DTO.Helper;
using ConwayGameOfLife.Core.Services;
using ConwayGameOfLife.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace ConwayGameofLife.API.Controllers
{
	[ApiController]
	[ApiVersion("1")]
	[Route("/game-of-life/api/v{version:apiVersion}/board")]
	public class GameofLifeAPIController : ControllerBase
	{
		private readonly IBoardService _boardService;
		private readonly IErrorService _errorService;

		public GameofLifeAPIController(IBoardService boardService, IErrorService errorService)
		{
			_boardService = boardService;
			_errorService = errorService;
		}

		#region Controller Public Methods
		/// <summary>
		/// Create a new board state
		/// </summary>
		/// <param name="board">board state object</param>
		/// <returns>id of board state</returns>
		[HttpPost("state/create")]
		public async Task<IActionResult> CreateBoardState([FromBody] BoardDto board)
		{
			try
			{
					if (board == null || board.Cells == null || board.Cells.Count == 0)
						return BadRequest(_errorService.CreateError((int)HttpStatusCode.BadRequest, ErrorsConstants.NullBoardStateMgs));

					var boardId = await _boardService.CreateBoardAsync(board);

				Console.WriteLine("CreateAndUploadBoardState: Board Created Successfully BoardId {boardId}");
				return Ok(boardId);
			}
			catch (Exception ex)
			{
				Console.WriteLine("CreateAndUploadBoardState: Exception {ex.Message}");
				return BadRequest(_errorService.CreateError((int)HttpStatusCode.InternalServerError,ErrorsConstants.InternalServerErrorMgs));
			}
		}
		/// <summary>
		/// gets the next board state
		/// </summary>
		/// <param name="id">id of the existing board in database</param>
		/// <returns>next board state</returns>
		[HttpGet("nextstate/{id:guid}")]
		public async Task<IActionResult> GetNextBoardState(Guid id)
		{
			try
			{
				var nextState = await _boardService.GetNextStateAsync(id);
				if (nextState == null)
					return NotFound(_errorService.CreateError((int)HttpStatusCode.NotFound, ErrorsConstants.BoardNotFoundMgs));
				
				return Ok(nextState);
			}
			catch (Exception ex)
			{
				Console.WriteLine("GetNextBoardState: Exception {ex.Message}");
				return BadRequest(_errorService.CreateError((int) HttpStatusCode.InternalServerError, ErrorsConstants.InternalServerErrorMgs));

			}
			
		}

		/// <summary>
		/// get the board state after x steps
		/// </summary>
		/// <param name="id">id of te existing board in database</param>
		/// <param name="noofSteps">no of steps</param>
		/// <returns>future board state</returns>
		[HttpGet("statesaway/{id:guid}/{noofsteps:int}")]
		public async Task<IActionResult> GetXStatesAway(Guid id, int noofsteps)
		{
			try
			{
				var nextState = await _boardService.GetXStatesAwayAsync(id, noofsteps);
				if (nextState == null)
					return NotFound(_errorService.CreateError((int)HttpStatusCode.NotFound, ErrorsConstants.BoardNotFoundMgs));
				
				return Ok(nextState);
			}
			catch (Exception ex)
			{
				Console.WriteLine("GetXStatesAway: Exception {ex.Message}");
				return BadRequest(_errorService.CreateError((int)HttpStatusCode.InternalServerError, ErrorsConstants.InternalServerErrorMgs));

			}

		}

		[HttpGet("finalstate/{id:guid}/{noofattempts:int}")]
		public async Task<IActionResult> GetFinalState(Guid id, int noofattempts)
		{
			try
			{
				var nextState = await _boardService.GetFinalStateAsync(id, noofattempts);
				if (nextState == null)
					return NotFound(_errorService.CreateError((int)HttpStatusCode.NotFound, ErrorsConstants.BoardNotFoundMgs));

				return Ok(nextState);
			}
			catch (Exception ex)
			{
				Console.WriteLine("GetFinalState: Exception {ex.Message}");
				return BadRequest(_errorService.CreateError((int)HttpStatusCode.InternalServerError, ErrorsConstants.InternalServerErrorMgs));

			}

		}
		#endregion
	}
}
