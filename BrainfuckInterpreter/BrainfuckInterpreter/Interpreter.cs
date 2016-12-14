using System;
using System.Collections.Generic;
using System.IO;

namespace BrainfuckInterpreter
{
	public class Interpreter
	{
		public Interpreter() : this(Console.Out, Console.In)
		{
		}

		public Interpreter(TextWriter writer, TextReader reader)
		{
			this.writer = writer;
			this.reader = reader;
			this.stack = new Stack<int>();
		}

		private readonly Stack<int> stack;

		private readonly byte[] data = new byte[30000];
		private int dataPointer;
		private readonly TextWriter writer;
		private readonly TextReader reader;

		public void Run(string code)
		{
			for (var codePointer = 0;
				codePointer < code.Length;
				codePointer++)
			{
				var op = code[codePointer];
				switch (op)
				{
					#region simple

					case '>':
					{
						dataPointer++;
						break;
					}
					case '<':
					{
						dataPointer--;
						break;
					}
					case '+':
					{
						data[dataPointer]++;
						break;
					}
					case '-':
					{
						data[dataPointer]--;
						break;
					}
					case '.':
					{
						var printed = (char) data[dataPointer];
						writer.Write(printed);
						break;
					}
					case ',':
					{
						var read = (byte) reader.Read();
						data[dataPointer] = read;
						break;
					}

						#endregion

					case '[':
					{
						/*
						 if the byte at the data pointer is zero, 
						 then instead of moving the instruction pointer 
						 forward to the next command, 
						 jump it forward to the command after the matching ] command.							 
						 */

						var counter = data[dataPointer];
						if (0 == counter)
						{
							int br = 1;
							while (br > 0)
							{
								++codePointer;
								if (code[codePointer] == '[') ++br;
								if (code[codePointer] == ']') --br;
							}
						}
						else
							stack.Push(codePointer);

						break;
					}

					case ']':
					{
						/*
						 if the byte at the data pointer is nonzero, 
						 then instead of moving the instruction pointer 
						 forward to the next command, jump it back to the 
						 command after the matching [ command.
						 */
						var counter = data[dataPointer];
						if (counter > 0)
							codePointer = stack.Peek();
						else
							stack.Pop();
						break;
					}
				}
			}
		}
	}
}