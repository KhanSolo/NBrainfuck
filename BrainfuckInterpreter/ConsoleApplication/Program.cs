namespace ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			var data = new byte[30000];
			var dataPointer = 0;

			data[dataPointer] = 7;

label_after_opening_bracket:

			data[dataPointer]--;

			if (data[dataPointer] != 0)
			{
				goto label_after_opening_bracket;
			}
			data[dataPointer]=0;
		}
	}
}
