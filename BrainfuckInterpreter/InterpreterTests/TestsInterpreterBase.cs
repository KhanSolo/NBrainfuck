using BrainfuckInterpreter;
using NUnit.Framework;

namespace InterpreterTests
{
	[TestFixture]
	public abstract class TestsInterpreterBase
	{
		protected static Writer Run(string code)
		{
			var writer = new Writer();
			var reader = new Reader();
			var interpreter = new Interpreter(writer, reader);
			interpreter.Run(code);
			return writer;
		}
	}
}