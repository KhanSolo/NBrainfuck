using System.IO;

namespace InterpreterTests
{
	public class Reader : TextReader
	{
		public Reader() : this(string.Empty) { }
		private readonly string input;
		private int i;
		public Reader(string input)
		{
			this.input = input;
			i = 0;
		}
		public char In => i == input.Length ? default(char) : input[i++];
		public override int Read()
		{
			return In;
		}
	}
}