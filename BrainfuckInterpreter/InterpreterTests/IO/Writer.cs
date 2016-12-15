using System.IO;
using System.Text;

namespace InterpreterTests
{
	public class Writer : TextWriter
	{
		private readonly StringBuilder sb = new StringBuilder();
		public string Out => sb.ToString();
		public override Encoding Encoding { get; }
		public override void Write(char value)
		{
			sb.Append(value);
		}
	}
}