using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Core.DTO
{
	public class BoardDto
	{
		public string Name { get; set; }
		public List<CellDto> Cells { get; set; }
	}
}
