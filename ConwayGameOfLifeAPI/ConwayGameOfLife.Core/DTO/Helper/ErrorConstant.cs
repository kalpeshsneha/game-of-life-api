using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Core.DTO.Helper
{
	public static class ErrorsConstants
	{
		public const string NullBoardStateMgs = "Board state can't be null.";
		public const string InvalidBoardStateMgs = "Board state is invalid. All rows should have the same number of columns.";
		public const string BoardNotFoundMgs = "Board state was not found.";
		public const string InternalServerErrorMgs = "An unexpected error occurred. Please try again later.";
	}
}
