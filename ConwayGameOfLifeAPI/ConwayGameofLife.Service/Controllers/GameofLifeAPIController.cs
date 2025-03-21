using ConwayGameOfLife.Core.Services;
using ConwayGameOfLife.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ConwayGameofLife.Service.Controllers
{
	[ApiController]
	[ApiVersion("1")]
	[Route("/game-of-life/api/v{version:apiVersion}/board")]
	public class GameofLifeAPIController : ControllerBase
	{
		private readonly IBoardService _boardService;

		public GameofLifeAPIController(IBoardService boardService)
		{
			_boardService = boardService;
		}


		[HttpPost("state/create")]
		public async Task<IActionResult> CreateAndUploadBoardState([FromBody] Board board)
		{

			try
			{
				if (board == null || board.Cells == null || board.Cells.Count == 0)
				{
					return BadRequest("Board and Cells cannot be null or empty");
				}

				var boardId = await _boardService.AddBoardAsync(board);

				Console.WriteLine("CreateAndUploadBoardState: Board Created Successfully BoardId {boardId}");
				return Ok(boardId);
			}
			catch (Exception ex)
			{
				Console.WriteLine("CreateAndUploadBoardState: Exception {ex.Message}");
				return StatusCode(500, "Internal Server Error: An unexpected error occurred. Please try again later");
			}
		}

		[HttpGet("/state/next/{id}")]
		public async Task<IActionResult> GetNextBoardState(Guid id)
		{
			try
			{
				if (await _boardService.GetBoardByIdAsync(id) is not Board board)
				{
					return NotFound($"Board with Id {id} not found");
				}

				return Ok(null);
				
			}
			catch (Exception ex)
			{
				Console.WriteLine($"GetNextBoardState: Exception {ex.Message}");
				return StatusCode(500, "Internal Server Error: An unexpected error occurred. Please try again later");
			}
		}
	}
}
