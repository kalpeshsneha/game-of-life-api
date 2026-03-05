using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Core.DTO
{
	public class CellDto
	{
		public int Row { get; set; }
		public int Column { get; set; }
		public bool IsAlive { get; set; }
	}
}
