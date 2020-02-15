using Bifrost;
using Bifrost.Brainfuck;
using Bifrost.CommonSemanticTree;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AurigaTest.Brainfuck
{
    public class CommonSemanticConverterTest
    {
        /* What I want is to see that a what the code does
         * If no inputs, I want to see what are the outputs
         * If there are inputs, I want to see what input creates what output, how does it affect them.
         * 
         * Code works on user inputs (dynamic data) + constants (static data).
         * Dynamic data are variables in the algorithm, we cannot know their value, 
         * only maybe their type, or make presumptions on their type from the constraints in the code. (Type = set of possible values).
         * For static inputs we know their value exactly from the code, and can do the computations with them offline, 
         * creating other static inputs.
         * References to static input -> still has static/dynamic distinction of data.
         * I want to see the output as the function of the inputs.
         * like c = a+b
         * 
         * Operators can work on data, or on control flow.
         */


        [Fact]
        public void ConvertSingleIncrement()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.parse("+");
            CSTRoot csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("1", csTree.Variables[0].Value);
            Assert.Equal(CSTType.Number, csTree.Variables[0].Type);
        }

        [Fact]
        public void ConvertMultipleIncrement()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.parse("++++");
            CSTRoot csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
        }

        [Fact]
        public void ConvertIncrementPointer()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.parse(">");
            CSTRoot csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("0", csTree.Variables[0].Value);
            Assert.Equal("b", csTree.Variables[1].Name);
            Assert.Equal("0", csTree.Variables[1].Value);
        }

        [Fact]
        public void ConvertIncrementPointerAndValue()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.parse(">+");
            CSTRoot csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("0", csTree.Variables[0].Value);
            Assert.Equal("b", csTree.Variables[1].Name);
            Assert.Equal("1", csTree.Variables[1].Value);
        }

        [Fact]
        public void ConvertMultipleIncrementPointer()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.parse(">>>");
            CSTRoot csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(4, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("b", csTree.Variables[1].Name);
            Assert.Equal("c", csTree.Variables[2].Name);
            Assert.Equal("d", csTree.Variables[3].Name);
        }

        [Fact]
        public void ConvertIncDecPointer()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.parse("><");
            CSTRoot csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("b", csTree.Variables[1].Name);
        }

        [Fact]
        public void ConvertIncPointerToSame()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.parse("><>");
            CSTRoot csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("b", csTree.Variables[1].Name);
        }
    }
}
