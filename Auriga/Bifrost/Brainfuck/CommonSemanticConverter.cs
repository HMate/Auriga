using Bifrost.CommonSemanticTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bifrost.Brainfuck
{
    public class CommonSemanticConverter
    {

        internal class LoopContext
        {
            public CSTVariable EntryVar { get; internal set; }
            public CSTDataContext Context { get; } = new CSTDataContext();

            public void ExtractContextVars(CSTDataContext context)
            {
                Context.Variables.Clear();
                foreach (var v in context.Variables)
                {
                    Context.Variables.Add(new CSTVariable(v.Name, v.Type, "0"));
                }
            }
        }

        public static CSTDataContext Convert(ASTRoot ast)
        {
            CSTDataContext root = new CSTDataContext();
            CSTDataContext currentCtx = root;
            int pointerPos = 0;
            currentCtx.Variables.Add(new CSTVariable(generateName(pointerPos), CSTType.Number, "0"));
            var loops = new Stack<LoopContext>();
            int skipLoop = 0;
            foreach (var command in ast.Commands)
            {
                if (skipLoop > 0 && 
                    !(command.Type==ASTCommandType.JumpForward || command.Type==ASTCommandType.JumpBack))
                {
                    continue;
                }
                switch (command.Type)
                {
                    case ASTCommandType.Decrement:
                        {
                            var variable = currentCtx.Variables[pointerPos];
                            addStaticValueToVariable(variable, -1);
                        }
                        break;
                    case ASTCommandType.Increment:
                        {
                            var variable = currentCtx.Variables[pointerPos];
                            addStaticValueToVariable(variable, 1);
                        }
                        break;
                    case ASTCommandType.DecrementPointer:
                        {
                            pointerPos--;
                        }
                        break;
                    case ASTCommandType.IncrementPointer:
                        {
                            pointerPos++;
                            if (pointerPos >= currentCtx.Variables.Count)
                            {
                                currentCtx.Variables.Add(new CSTVariable(generateName(pointerPos), CSTType.Number, "0"));
                            }
                        }
                        break;
                    case ASTCommandType.WriteByte:
                        {
                            var variable = currentCtx.Variables[pointerPos];
                            var output = new CSTVariable(variable.Name, variable.Type, variable.Value);
                            currentCtx.Outputs.Add(output);
                        }
                        break;
                    case ASTCommandType.ReadByte:
                        {
                            var variable = currentCtx.Variables[pointerPos];
                            variable.Value = variable.Name;
                            variable.Type = CSTType.Number;
                            variable.Binding = CSTBinding.Dynamic;
                        }
                        break;
                    case ASTCommandType.JumpForward:
                        {
                            // current context is the root context, we just check static value
                            // if we are in a loop, we have to create a smart algorithm for loop entry checking
                            // it depends on the value of the currentVar, which can change inside the loop.
                            var entryVar = currentCtx.Variables[pointerPos];
                            if (entryVar.Binding == CSTBinding.Constant && mod8bit(int.Parse(entryVar.Value)) == 0)
                            {
                                skipLoop++;
                                break;
                            }
                            LoopContext ctx = new LoopContext { 
                                EntryVar = entryVar
                            };
                            ctx.ExtractContextVars(currentCtx);
                            loops.Push(ctx);
                            currentCtx = ctx.Context;
                        }
                        break;
                    case ASTCommandType.JumpBack:
                        {
                            if(skipLoop > 0)
                            {
                                skipLoop--;
                                break;
                            }
                            var loop = loops.Pop();
                            currentCtx = (loops.Count > 0) ? loops.Peek().Context : root;

                            // Upon leaving the loop scope, we pass on the variable changes to the parent scope
                            // basic formula for every var: parentScopeValue = parentValueOriginal + loopCount * loopValueChange
                            var loopExitVar = loop.Context.Variables[pointerPos];
                            if (loopExitVar.Name == loop.EntryVar.Name)
                            {
                                if (loopExitVar.Binding == CSTBinding.Constant &&
                                    loop.EntryVar.Binding == CSTBinding.Constant)
                                {
                                    var c0 = int.Parse(loop.EntryVar.Value);
                                    var c1 = int.Parse(loopExitVar.Value);
                                    // c0 + x*c1 = 0 (mod 256) if x not in Z, then infinit loop

                                    var value = c0 + c1;
                                    var count = 1;
                                    while (mod8bit(value) != 0)
                                    {
                                        value += c1;
                                        count++;
                                        if (c0 == value)
                                        {
                                            // TODO: infinite loop
                                            //count = INFINITE;
                                            break;
                                        }
                                    }
                                    // TODO: Do this computation mathematically
                                    //(256 - c0) / c1 (mod 256) in Z 
                                    //(256-c0) % c1 == 0 

                                    foreach (var loopVar in loop.Context.Variables)
                                    {
                                        var scopeVar = currentCtx.GetVariable(loopVar.Name);
                                        if(scopeVar == null)
                                        {
                                            var loopValue = int.Parse(loopVar.Value);
                                            scopeVar = new CSTVariable(loopVar.Name, CSTType.Number, (loopValue * count).ToString());
                                            scopeVar.Binding = loopVar.Binding;
                                            currentCtx.Variables.Add(scopeVar);
                                        }
                                        else if (scopeVar.Binding == CSTBinding.Constant && loopVar?.Binding == CSTBinding.Constant)
                                        {
                                            var loopValue = int.Parse(loopVar.Value);
                                            scopeVar.Value = (int.Parse(scopeVar.Value) + (loopValue * count)).ToString();
                                        }
                                        else
                                        {
                                            // TODO: either outside or inside loop the variable is dynamic
                                        }
                                    }
                                }
                                else if (loopExitVar.Binding == CSTBinding.Dynamic)
                                {
                                    // every touched variable becomes dependent on the dynamic value
                                }
                            }
                            else
                            {
                                // If loop start and end var differs -> its doing a variable search?
                                // sliding the pointer until reaches a 0 variable
                            }
                            
                            // We only leave a loop if the exit var is 0
                            var curVar = currentCtx.Variables[pointerPos];
                            curVar.Value = "0";
                            curVar.Binding = CSTBinding.Constant;
                        }
                        break;
                    default:
                        break;
                }
            }
            return root;
        }

        private static void addStaticValueToVariable(CSTVariable variable, int value)
        {
            var oldVal = variable.Value;
            if (variable.Binding == CSTBinding.Constant)
                variable.Value = (int.Parse(oldVal) + value).ToString();
            else
            {
                string staticValue = value.ToString();
                if (oldVal.Contains("+"))
                {
                    string[] parts = oldVal.Split("+");
                    staticValue = (int.Parse(parts[1]) + value).ToString();
                }
                variable.Value = variable.Name + "+" + staticValue;
            }
        }

        private static int mod8bit(int value)
        {
            var m = 256;
            return (value % m + m) % m;
        }

        private static string generateName(int index)
        {
            string result = Char.ConvertFromUtf32('a' + index);
            return result;
        }
    }
}
