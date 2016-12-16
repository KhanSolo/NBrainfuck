using System.Text;

namespace BrainfuckEmit
{
	public class Optimizer : IOptimizer
	{
		public string Optimize(string code)
		{
			// todo optimizer

			var sb = new StringBuilder(code);

			sb = RemoveSpaces(sb);

			for (var i = 0; i < sb.Length; ++i)
				// +++++       --> i5 (increment by 5)   DONE
				// -------     --> d7 (decrement by 7)
				// >>>		   --> r3 (right shift by 3)
				// <<<<<<	   --> l6 (left shift by 6)
				// [+] или [-] --> s0 (set to 0)         DONE
			{
				bool noOp = true;

				if (sb[i] == '+')
				{
					noOp = false;
					i = Compress(i, sb, 'i', '+');
				}
				if (sb[i] == '-')
				{
					noOp = false;
					i = Compress(i, sb, 'd', '-');
				}
				if (sb[i] == '>')
				{
					noOp = false;
					//i = compress(i, sb, 'd', '-');
				}
				if (sb[i] == '<')
				{
					noOp = false;
					//i = compress(i, sb, 'd', '-');
				}
				if (sb[i] == '.') noOp = false;
				if (sb[i] == ',') noOp = false;
				if (sb[i] == ']') noOp = false;
				if (sb[i] == '[')
				{
					noOp = false;
					if (i + 2 < sb.Length)
						if (
							(sb[i + 1] == '+' || sb[i + 1] == '-')
							&&
							(sb[i + 2] == ']'))
						{
							sb[i] = 's';
							sb[i + 1] = '0';
							sb[i + 2] = ' ';
							i++;
						}
				}
				if (noOp) sb[i] = ' ';
			}
			sb = RemoveSpaces(sb);
			return sb.ToString();
		}

		private static int Compress(int i, StringBuilder sb, char newOpcode, char oldOpcode)
		{
			var idx = i; // с какого писать строку
			int incr = 1;
			while ((i + 1 < sb.Length)
			       && (sb[i + 1] == oldOpcode))
			{
				incr++;
				i++;
			}
			if (incr > 1)
			{
				sb[idx] = newOpcode;
				idx++;
				var sIncr = incr.ToString();
				foreach (var c in sIncr)
				{
					sb[idx] = c;
					idx++;
				}
				while (idx <= i)
				{
					sb[idx] = ' ';
					idx++;
				}
			}
			return i;
		}

		private static StringBuilder RemoveSpaces(StringBuilder sb)
		{
			var sb1 = new StringBuilder(sb.Length);
			for (var i = 0; i < sb.Length; ++i)
			{
				var ch = sb[i];
				if (ch != ' ')
					sb1.Append(ch);
			}
			return sb1;
		}
	}
}