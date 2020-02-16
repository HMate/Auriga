using Bifrost.Brainfuck;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AurigaTest.Brainfuck
{
    public class BrainfuckParserTest
    {
        [Fact]
        public void ParseBrainfuckSymbols()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("<>+-.,[]");
            Assert.Equal(8, ast.Commands.Count);
            Assert.Equal(ASTCommandType.DecrementPointer, ast.Commands[0].Type);
            Assert.Equal(ASTCommandType.IncrementPointer, ast.Commands[1].Type);
            Assert.Equal(ASTCommandType.Increment, ast.Commands[2].Type);
            Assert.Equal(ASTCommandType.Decrement, ast.Commands[3].Type);
            Assert.Equal(ASTCommandType.WriteByte, ast.Commands[4].Type);
            Assert.Equal(ASTCommandType.ReadByte, ast.Commands[5].Type);
            Assert.Equal(ASTCommandType.JumpForward, ast.Commands[6].Type);
            Assert.Equal(ASTCommandType.JumpBack, ast.Commands[7].Type);
        }

        [Fact]
        public void IgnoreOtherSymbols()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(",+++. This program increments is input by 3");
            Assert.Equal(5, ast.Commands.Count);
            Assert.Equal(ASTCommandType.ReadByte, ast.Commands[0].Type);
            Assert.Equal(ASTCommandType.Increment, ast.Commands[1].Type);
            Assert.Equal(ASTCommandType.Increment, ast.Commands[2].Type);
            Assert.Equal(ASTCommandType.Increment, ast.Commands[3].Type);
            Assert.Equal(ASTCommandType.WriteByte, ast.Commands[4].Type);
        }
    }
}
