using ConwayGameOfLife.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Core.Services
{
	public interface IErrorService
	{
		ErrorDto CreateError(int statusCode, string details = null);
	}
}
