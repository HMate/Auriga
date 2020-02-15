using Bifrost.CommonSemanticTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bifrost.Brainfuck
{
    public class CommonSemanticConverter
    {
        public static CSTRoot Convert(ASTRoot ast)
        {
            CSTRoot root = new CSTRoot();
            var nameGenerator = new VariableNameGenerator();
            root.Variables.Add(new CSTVariable(nameGenerator.GenerateName(), CSTType.Number, "0"));
            int pointerPos = 0;
            foreach (var command in ast.Commands)
            {
                switch (command.Type)
                {
                    case ASTCommandType.Decrement:
                        break;
                    case ASTCommandType.Increment:
                        {
                            var variable = root.Variables[pointerPos];
                            variable.Value = (int.Parse(variable.Value) + 1).ToString();
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
                            if(pointerPos >= root.Variables.Count)
                                root.Variables.Add(new CSTVariable(nameGenerator.GenerateName(), CSTType.Number, "0"));
                        }
                        break;
                    case ASTCommandType.WriteByte:
                        break;
                    case ASTCommandType.ReadByte:
                        break;
                    case ASTCommandType.JumpForward:
                        break;
                    case ASTCommandType.JumpBack:
                        break;
                    default:
                        break;
                }
            }
            return root;
        }

        private class VariableNameGenerator
        {
            private int counter = 0;
            public string GenerateName()
            {
                string result = Char.ConvertFromUtf32('a' + counter);
                counter++;
                return result;
            }
        }
    }
}
