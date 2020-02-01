using System;
using System.Collections.Generic;
using System.Text;

namespace Bifrost.Brainfuck
{
    public class Parser
    {
        public ASTRoot parse(string source)
        {
            /* Brainfuck syntax:
             * List of valid symbols: > < + - . , [ ]
             * anything else is omitted.
             * */
            ASTRoot root = new ASTRoot();
            foreach (var ch in source)
            {
                ASTCommand command;
                switch (ch)
                {
                    case '>': command = new ASTCommand(ASTCommandType.IncrementPointer); break;
                    case '<': command = new ASTCommand(ASTCommandType.DecrementPointer); break;
                    case '+': command = new ASTCommand(ASTCommandType.Increment); break;
                    case '-': command = new ASTCommand(ASTCommandType.Decrement); break;
                    case '.': command = new ASTCommand(ASTCommandType.WriteByte); break;
                    case ',': command = new ASTCommand(ASTCommandType.ReadByte); break;
                    case '[': command = new ASTCommand(ASTCommandType.JumpForward); break;
                    case ']': command = new ASTCommand(ASTCommandType.JumpBack); break;
                    default:
                        continue;
                }
                root.Commands.Add(command);
            }
            return root;
        }
    }
}
