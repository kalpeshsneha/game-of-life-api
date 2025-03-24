using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Core.DTO
{
	public class ErrorDto
	{
		public int StatusCode { get; set; }
		public string Details { get; set; }
	}
}
