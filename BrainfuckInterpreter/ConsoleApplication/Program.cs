using System;

namespace ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Enter a");
			char ch = Char.MinValue;
			while (ch != 'a')
			{
				ch = Console.ReadKey(
					/*true*/
					).KeyChar;
			}
			Console.Write("Thnx!");
			Console.ReadLine();
		}

		public static void PointMain()
		{
			byte[] numArray = new byte[30000];
			int index1 = 0;
			numArray[index1] += (byte)3;
			numArray[index1] = (byte)0;
			numArray[index1] += (byte)10;
			if ((int)numArray[index1] != 0)
			{
				do
				{
					int index2 = index1 + 1;
					numArray[index2] += (byte)7;
					int index3 = index2 + 1;
					numArray[index3] += (byte)10;
					int index4 = index3 + 1;
					numArray[index4] += (byte)3;
					int index5 = index4 + 1;
					++numArray[index5];
					index1 = index5 - 1 - 1 - 1 - 1;
					--numArray[index1];
				}
				while ((int)numArray[index1] != 0);
			}
			int index6 = index1 + 1;
			numArray[index6] += (byte)2;
			Console.Write((char)numArray[index6]);
			int index7 = index6 + 1;
			++numArray[index7];
			Console.Write((char)numArray[index7]);
			numArray[index7] += (byte)7;
			Console.Write((char)numArray[index7]);
			Console.Write((char)numArray[index7]);
			numArray[index7] += (byte)3;
			Console.Write((char)numArray[index7]);
			int index8 = index7 + 1;
			numArray[index8] += (byte)2;
			Console.Write((char)numArray[index8]);
			int index9 = index8 - 1 - 1;
			numArray[index9] += (byte)15;
			Console.Write((char)numArray[index9]);
			int index10 = index9 + 1;
			Console.Write((char)numArray[index10]);
			numArray[index10] += (byte)3;
			Console.Write((char)numArray[index10]);
			numArray[index10] -= (byte)6;
			Console.Write((char)numArray[index10]);
			numArray[index10] -= (byte)8;
			Console.Write((char)numArray[index10]);
			int index11 = index10 + 1;
			++numArray[index11];
			Console.Write((char)numArray[index11]);
			int index12 = index11 + 1;
			Console.Write((char)numArray[index12]);
		}
	}
}
