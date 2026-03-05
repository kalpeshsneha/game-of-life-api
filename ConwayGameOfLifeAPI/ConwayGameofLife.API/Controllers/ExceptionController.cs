using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConwayGameofLife.API.Controllers
{
	// This controller handles exceptions and errors in the application.
	[Route("api/[controller]")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)] 
	public class ExceptionController : ControllerBase
	{
		// This method handles error reporting.
		// The route for this method is "api/Exception/report".
		[Route("report")]
		public IActionResult HandleError() =>
			// Returns a standardized error response using the Problem method.
			Problem();
	}
}