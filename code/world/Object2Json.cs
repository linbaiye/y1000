using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace y1000.code.world
{
	public struct Object2Json
	{
		public byte Version => 2;

		public int Width { get; set; }

		public int Height { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		public static Object2Json FromJsonString(string jsonString)
		{
			return JsonSerializer.Deserialize<Object2Json>(jsonString);
		}
	}
}