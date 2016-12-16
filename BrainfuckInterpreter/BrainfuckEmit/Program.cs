﻿using System;
using System.IO;

namespace BrainfuckEmit
{
	class Program
	{
		static void Main(string[] args)
		{
			//var code = File.ReadAllText("bizzfuzz.txt");

			/*
			var code =
	"+++++++++++++++++++++++++++++++++++++++++++++" 
	+ " +++++++++++++++++++++++++++.+++++++++++++++++\r\n ++++++++++++.+++++++..+++.-------------------\r\n ---------------------------------------------\r\n ---------------.+++++++++++++++++++++++++++++\r\n ++++++++++++++++++++++++++.++++++++++++++++++\r\n ++++++.+++.------.--------.------------------\r\n ---------------------------------------------\r\n ----.-----------------------.";*/

			/*var code =
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
			+ "]<<]>.";*/

			var code = "+++[-]++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++"
						+ " .>+.+++++++..+++.>++.<<+++++++++++++++.>.+++."
						+ " ------.--------.>+.>.";

			var interpreter = new BrainfuckEmitter(new Validator(), new Optimizer());
			interpreter.Run(code);
			Console.ReadKey();
		}
	}
}