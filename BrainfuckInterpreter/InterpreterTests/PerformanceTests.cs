using System;
using System.Diagnostics;
using NUnit.Framework;

namespace InterpreterTests
{
	public class PerformanceTests : TestsInterpreterBase
	{
		[Test]
		public void TestLongCycles()
		{
			var code =
				">+>+>+>+>++<[>[<+++>-" 
				+ " >>>>>" 
				+ " >+>+>+>+>++<[>[<+++>-" 
				+ "   >>>>>" 
				+ "   >+>+>+>+>++<[>[<+++>-" 
				+ "     >>>>>" 
				+ "     >+>+>+>+>++<[>[<+++>-" 
				+ "       >>>>>" 
				+ "       +++[->+++++<]>[-]<" 
				+ "       <<<<<" 
				+ "     ]<<]>[-]" 
				+ "     <<<<<" 
				+ "   ]<<]>[-]" 
				+ "   <<<<<" 
				+ " ]<<]>[-]" 
				+ " <<<<<" 
				+ "]<<]>.";

			var sw = new Stopwatch();
			sw.Start();
			var writer = Run(code);
			Console.WriteLine(writer.Out);
			sw.Stop();
			Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds + " ms");
			//Elapsed: 204097 ms
			//Elapsed: 129801 ms
		}
	}
}