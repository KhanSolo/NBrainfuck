﻿using System;
using System.Diagnostics;
using NUnit.Framework;

namespace InterpreterTests
{
	public class PerformanceTests : TestsBase
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
			sw.Stop();
			Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds + " ms");
		}
	}
}