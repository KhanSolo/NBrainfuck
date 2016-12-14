using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainfuckInterpreter;
using NUnit.Framework;

namespace InterpreterTests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void TestUnoptimizedHelloWorld()
		{
			var code =
				"+++++++++++++++++++++++++++++++++++++++++++++\r\n +++++++++++++++++++++++++++.+++++++++++++++++\r\n ++++++++++++.+++++++..+++.-------------------\r\n ---------------------------------------------\r\n ---------------.+++++++++++++++++++++++++++++\r\n ++++++++++++++++++++++++++.++++++++++++++++++\r\n ++++++.+++.------.--------.------------------\r\n ---------------------------------------------\r\n ----.-----------------------.";
			var writer = Run(code);
			Assert.True("Hello World!\n" == writer.Out, $"'{writer.Out}'");
		}

		[Test]
		[MaxTime(1000)]
		public void TestOptimizedHelloWorld()
		{
			Writer writer = null;
			try
			{
				var code = "++++++++++[>+++++++>++++++++++>+++>+<<<<-]"
				           + ">++" // состояние памяти после цикла	  [0,70,100,30,10,0,0...]
				           + "." // состояние памяти на первой точке	  [0,72,100,30,10,0,0...]
				           + ">+.+++++++..+++.>++.<<+++++++++++++++.>.+++."
				           + "------.--------.>+.>.";

				writer = Run(code);
				Assert.That(writer.Out, Is.EqualTo("Hello World!\n")
					.After(5000, 50), $"'{writer.Out}'");
			}
			catch (Exception ex)
			{
				Console.WriteLine(writer?.Out ?? "<null>");
				Assert.Fail(ex.ToString());
			}
		}

		[Test]
		public void TestSimpleLoop()
		{
			var code = "+++++ +++++             initialize counter (cell #0) to 10"
			           + "["
			           + "    > +++++ ++    add  7 to cell #1"
			           + "    < -           decrement counter (cell #0)"
			           + "]"
			           + "> ++ .            print \'H\'";
			var writer = Run(code);
			Assert.That(writer.Out, Is.EqualTo("H")
				.After(5000, 50), $"'{writer.Out}'");
		}

		private static Writer Run(string code)
		{
			var writer = new Writer();
			var reader = new Reader();
			var interpreter = new Interpreter(writer, reader);
			interpreter.Run(code);
			return writer;
		}

		[Test]
		public void TestSelectionSorting()
		{
			var code = ">>,[>>,]<<[[-<+<]>[>[>>]<[.[-]<[[>>+<<-]<]>>]>]<<]";
			var writer = new Writer();
			var inputArray = new byte[]
				{
					4, 25, 20, 5, 1, 55, 128, 1, 4
				}.Select(b => (char) b)
				.ToArray();

			var reader = new Reader(new string(inputArray));

			var interpreter = new Interpreter(writer, reader);
			interpreter.Run(code);
			
			var outArray = writer.Out.ToCharArray();

			foreach (var tuple in inputArray.OrderBy(_ => _)
				.Zip(outArray, (a, b) => new Tuple<char, char>(a, b)))
			{
				Assert.True(tuple.Item1 == tuple.Item2);
			}
		}
	}

	internal class Writer : TextWriter
	{
		private readonly StringBuilder sb = new StringBuilder();
		public string Out => sb.ToString();
		public override Encoding Encoding { get; }
		public override void Write(char value)
		{
			sb.Append(value);
		}
	}

	internal class Reader : TextReader
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

	[TestFixture]
	public class InternalTests
	{
		[Test]
		public void TestInput()
		{
			var expect = new []
			{
				'a', 'b', 'c', default(char)
				, default(char)
				, default(char)
				, default(char)
			};
			var input = new Reader("abc");
			foreach (var ch in expect)
			{
				var inp = input.In;
				Assert.True(inp == ch, $"'{inp}' '{ch}'");
			}
		}
	}
}