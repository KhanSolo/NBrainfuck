using System;
using System.Collections.Generic;
using System.Linq;
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
			var code =
	"+++++++++++++++++++++++++++++++++++++++++++++" 
	+ " +++++++++++++++++++++++++++.+++++++++++++++++\r\n ++++++++++++.+++++++..+++.-------------------\r\n ---------------------------------------------\r\n ---------------.+++++++++++++++++++++++++++++\r\n ++++++++++++++++++++++++++.++++++++++++++++++\r\n ++++++.+++.------.--------.------------------\r\n ---------------------------------------------\r\n ----.-----------------------.";
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

		private string Optimize(string code)
		{
			// todo optimizer
			return code;
		}

		public void Run(string code)
		{
			code = Optimize(code);

			MethodInfo writeMI = typeof(Console).GetMethod(
					 "Write",
					 new Type[] { typeof(char) }); // вывод символа


			Type pointType;

			AppDomain currentDom = Thread.GetDomain();

			//Console.Write("Please enter a name for your new assembly: ");
			StringBuilder asmFileNameBldr = new StringBuilder();
			asmFileNameBldr.Append(/*Console.ReadLine()*/"bf_executable");
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
			pointType.InvokeMember()
		}
	}
}
