using System;
using BrainfuckEmit;
using NUnit.Framework;

namespace InterpreterTests
{
	[TestFixture]
	public class OptimizerTests
	{
		[Test]
		public void OptimizeS0()
		{
			var code = "+[+]<->[-]";
			var emit = new Optimizer();
			var optimized = emit.Optimize(code);
			Assert.True("+s0<->s0"==optimized);
		}

		[Test]
		public void TestOptimizeHelloWorld()
		{
			var code = "+++[-]++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++"
						+ " .>+.+++++++..+++.>++.<<+++++++++++++++.>.+++."
						+ " ------.--------.>+.>.";
			var emit = new Optimizer();
			var optimized = emit.Optimize(code);
			Assert.True("i3s0i10[>i7>i10>i3>+<<<<-]>i2.>+.i7..i3.>i2.<<i15.>.i3.d6.d8.>+.>." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizePlus()
		{
			var code = "+++++ +++++.";
			var emit = new Optimizer();
			var optimized = emit.Optimize(code);
			Assert.True("i10." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizePlus1()
		{
			var code = "+++++<+++++.";
			var emit = new Optimizer();
			var optimized = emit.Optimize(code);
			Assert.True("i5<i5." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizePlus2()
		{
			var code = "+++++ +++++ +++++.";
			var emit = new Optimizer();
			var optimized = emit.Optimize(code);
			Assert.True("i15." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizeMinus()
		{
			var code = "----- ----- -----.";
			var emit = new Optimizer();
			var optimized = emit.Optimize(code);
			Assert.True("d15." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizeLeft()
		{
			var code = "lllll lllll lllll.";
			var emit = new Optimizer();
			var optimized = emit.Optimize(code);
			Assert.True("l15." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizeRight()
		{
			var code = "rrrrr rrrrr rrrrr.";
			var emit = new Optimizer();
			var optimized = emit.Optimize(code);
			Assert.True("r15." == optimized, $"{optimized}");
		}
	}
}