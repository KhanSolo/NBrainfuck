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
			var emit = new BrainfuckEmitter();
			var optimized = emit.Optimize(code);
			Assert.True("+s0<->s0"==optimized);
		}

		[Test]
		public void TestOptimizeHelloWorld()
		{
			var code = "+++[-]++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++"
						+ " .>+.+++++++..+++.>++.<<+++++++++++++++.>.+++."
						+ " ------.--------.>+.>.";
			var emit = new BrainfuckEmitter();
			var optimized = emit.Optimize(code);
			Assert.True("i3s0i10[>i7>i10>i3>+<<<<-]>i2.>+.i7..i3.>i2.<<i15.>.i3.------.--------.>+.>." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizePlus()
		{
			var code = "+++++ +++++.";
			var emit = new BrainfuckEmitter();
			var optimized = emit.Optimize(code);
			Assert.True("i10." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizePlus1()
		{
			var code = "+++++<+++++.";
			var emit = new BrainfuckEmitter();
			var optimized = emit.Optimize(code);
			Assert.True("i5<i5." == optimized, $"{optimized}");
		}

		[Test]
		public void OptimizePlus2()
		{
			var code = "+++++ +++++ +++++.";
			var emit = new BrainfuckEmitter();
			var optimized = emit.Optimize(code);
			Assert.True("i15." == optimized, $"{optimized}");
		}
	}
}