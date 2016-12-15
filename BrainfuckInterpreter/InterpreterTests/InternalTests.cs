using NUnit.Framework;

namespace InterpreterTests
{
	public class InternalTests : TestsInterpreterBase
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