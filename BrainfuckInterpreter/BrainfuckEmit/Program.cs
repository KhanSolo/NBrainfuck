﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace BrainfuckEmit
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			var code =
	"+++++++++++++++++++++++++++++++++++++++++++++" 
	+ " +++++++++++++++++++++++++++.+++++++++++++++++\r\n ++++++++++++.+++++++..+++.-------------------\r\n ---------------------------------------------\r\n ---------------.+++++++++++++++++++++++++++++\r\n ++++++++++++++++++++++++++.++++++++++++++++++\r\n ++++++.+++.------.--------.------------------\r\n ---------------------------------------------\r\n ----.-----------------------.";*/

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

			//var code = "+++[-]++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++" 
			//            + " .>+.+++++++..+++.>++.<<+++++++++++++++.>.+++." 
			//		    + " ------.--------.>+.>.";

			var interpreter = new BrainfuckEmitter();
			interpreter.Run(code);
			Console.ReadKey();
		}
	}

	public class BrainfuckEmitter
	{
		public BrainfuckEmitter()
		{
			
		}

		private void Validate(string code)
		{
			
		}

		private string Optimize(string code)
		{
			// todo optimizer
			
			// +++++       --> i5 (increment by 5)
			// -------     --> d7 (decrement by 7)
			// >>>		   --> r3 (right shift by 3)
			// <<<<<<	   --> l6 (left shift by 6)
			// [+] или [-] --> s0 (set to 0)
			
			return code;
		}

		public void Run(string code)
		{
			code = Optimize(code);

			MethodInfo writeMI = typeof(Console).GetMethod(
					 "Write",
					 new Type[] { typeof(char) }); // вывод символа

			// todo Read

			Type pointType;

			AppDomain currentDom = Thread.GetDomain();

			//Console.Write("Please enter a name for your new assembly: ");
			StringBuilder asmFileNameBldr = new StringBuilder();
			asmFileNameBldr.Append("bf_executable");
			asmFileNameBldr.Append(".exe");
			string asmFileName = asmFileNameBldr.ToString();

			AssemblyName myAsmName = new AssemblyName();
			myAsmName.Name = "MyDynamicAssembly";

			AssemblyBuilder myAsmBldr = currentDom.DefineDynamicAssembly(
							   myAsmName,
							   AssemblyBuilderAccess.RunAndSave);

			// We've created a dynamic assembly space - now, we need to create a module
			ModuleBuilder myModuleBldr = myAsmBldr.DefineDynamicModule(asmFileName, asmFileName);
			TypeBuilder myTypeBldr = myModuleBldr.DefineType("BfProgram");

			Type objType = Type.GetType("System.Object");
			ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);

			Type[] ctorParams = new Type[] { /*typeof(int), typeof(int)*/ };
			ConstructorBuilder pointCtor = myTypeBldr.DefineConstructor(
									   MethodAttributes.Public,
									  CallingConventions.Standard, ctorParams);

			ILGenerator ctorIL = pointCtor.GetILGenerator();
			ctorIL.Emit(OpCodes.Ret);

			// main exec method
			MethodBuilder pointMainBldr = myTypeBldr.DefineMethod("PointMain",
							MethodAttributes.Public |
							MethodAttributes.Static,
							typeof(void),
							null);
			pointMainBldr.InitLocals = true;
			ILGenerator pmIL = pointMainBldr.GetILGenerator();

			var memory = pmIL.DeclareLocal(typeof(byte[])); // data (memory)
			var dataPointer = pmIL.DeclareLocal(typeof(int)); // dataPointer

			// var memory = new byte[30000];
			pmIL.Emit(OpCodes.Ldc_I4, 30000);
			pmIL.Emit(OpCodes.Newarr, typeof(byte));
			pmIL.Emit(OpCodes.Stloc, memory);

			//	var dataPointer = 0;
			pmIL.Emit(OpCodes.Ldc_I4_0);
			pmIL.Emit(OpCodes.Stloc, dataPointer);

			#region generate body

			// int - codePointer of closing bracket
			// Tuple Item1 - label after opening bracket
			// Tuple Item2 - label after closing bracket
			Dictionary<int, Tuple<Label, Label>> labels = new Dictionary<int, Tuple<Label, Label>>();

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
							//dataPointer++;
							pmIL.Emit(OpCodes.Ldloc, dataPointer);
							pmIL.Emit(OpCodes.Ldc_I4, 1); //
							pmIL.Emit(OpCodes.Add);
							pmIL.Emit(OpCodes.Stloc, dataPointer);

							break;
						}
					case '<':
						{
							//dataPointer--;
							pmIL.Emit(OpCodes.Ldloc, dataPointer);
							pmIL.Emit(OpCodes.Ldc_I4, 1);
							pmIL.Emit(OpCodes.Sub);
							pmIL.Emit(OpCodes.Stloc, dataPointer);

							break;
						}
					case '+':
						{
							//data[dataPointer]++;
							pmIL.Emit(OpCodes.Ldloc, memory);
							pmIL.Emit(OpCodes.Ldloc, dataPointer);
							pmIL.Emit(OpCodes.Ldelema, typeof(byte));
							pmIL.Emit(OpCodes.Dup);
							pmIL.Emit(OpCodes.Ldind_U1);
							pmIL.Emit(OpCodes.Ldc_I4, 1); // 1
							pmIL.Emit(OpCodes.Add);
							pmIL.Emit(OpCodes.Conv_U1);
							pmIL.Emit(OpCodes.Stind_I1);
							/*
							 //000013: 			memory[dataPointer] += 1;
							  IL_000e:  ldloc.0
							  IL_000f:  ldloc.1
							  IL_0010:  ldelema    [mscorlib]System.Byte
							  
							  // This instruction takes the element index (native int) 
							  // and the vector reference (an object reference) 
							  // from the stack and pushes the managed pointer 
							  // to the element on the stack.

							  IL_0015:  dup
							  IL_0016:  ldind.u1 // indirect loading
							  IL_0017:  ldc.i4.1 // <--
							  IL_0018:  add
							  IL_0019:  conv.u1  // conv to byte
							  IL_001a:  stind.i1 // indirect store
							 */

							break;
						}
					case '-':
						{
							//data[dataPointer]--;

							pmIL.Emit(OpCodes.Ldloc, memory);
							pmIL.Emit(OpCodes.Ldloc, dataPointer);
							pmIL.Emit(OpCodes.Ldelema, typeof(byte));
							pmIL.Emit(OpCodes.Dup);
							pmIL.Emit(OpCodes.Ldind_U1);
							pmIL.Emit(OpCodes.Ldc_I4, 1); // 1
							pmIL.Emit(OpCodes.Sub);
							pmIL.Emit(OpCodes.Conv_U1);
							pmIL.Emit(OpCodes.Stind_I1);


							break;
						}
					case '.':
						{
							//writer.Write((char) data[dataPointer]);

							pmIL.Emit(OpCodes.Ldloc, memory);
							pmIL.Emit(OpCodes.Ldloc, dataPointer);
							pmIL.Emit(OpCodes.Ldelem_U1);
							pmIL.EmitCall(OpCodes.Call, writeMI, null);

							break;
						}
					case ',':
						{
							//var read = (byte) reader.Read();
							//data[dataPointer] = read;
							throw new NotImplementedException("Input is not implemented");
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

							// determine codePointer of correspondence closing bracket
						var cp = codePointer;
							
							int br = 1; // current
							while (br > 0)
							{
								++cp;
								if (code[cp] == '[') ++br;
								if (code[cp] == ']') --br;
							}
							 
						var label_after_closing_bracket = /*new Label();*/ pmIL.DefineLabel();
						var label_after_opening_bracket = /*new Label();*/ pmIL.DefineLabel();

						var tuple = new Tuple<Label, Label>(
							label_after_opening_bracket, 
							label_after_closing_bracket);

						labels.Add(cp, tuple);
	
							pmIL.Emit(OpCodes.Ldloc, memory);
							pmIL.Emit(OpCodes.Ldloc, dataPointer);
							pmIL.Emit(OpCodes.Ldelem_U1);
							// branch if value is nonzero
							pmIL.Emit(OpCodes.Brfalse, label_after_closing_bracket);
							
							pmIL.MarkLabel(label_after_opening_bracket);
							
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

							 var tuple = labels[codePointer];
						
							 var label_after_opening_bracket = tuple.Item1;
							 var label_after_closing_bracket = tuple.Item2;
							labels.Remove(codePointer);
							pmIL.Emit(OpCodes.Ldloc, memory);
							pmIL.Emit(OpCodes.Ldloc, dataPointer);
							pmIL.Emit(OpCodes.Ldelem_U1);
							// branch if value is nonzero
							pmIL.Emit(OpCodes.Brtrue, label_after_opening_bracket);
						
							pmIL.MarkLabel(label_after_closing_bracket);
							
							break;
					}
				}
			}

			#endregion

			pmIL.Emit(OpCodes.Ret);
			//

			pointType = myTypeBldr.CreateType();
			Console.WriteLine("Type completed.");

			myAsmBldr.SetEntryPoint(pointMainBldr);

			myAsmBldr.Save(asmFileName);


			// todo transfer control to generated asm
			//pointType.InvokeMember()
		}
	}
}
