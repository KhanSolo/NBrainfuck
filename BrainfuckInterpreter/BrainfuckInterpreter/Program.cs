using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainfuckInterpreter
{
	class Program
	{


		static void Main(string[] args)
		{
			var code = File.ReadAllText("collatz.txt");
			var interpreter = new Interpreter();
			interpreter.Run(code);
			Console.ReadKey();
		}
	}
}