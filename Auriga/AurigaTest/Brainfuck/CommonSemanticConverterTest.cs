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
         * 
         * Select 2 points in the code, and show all the possible paths between them? inputs outputs?
         * Some paths in code contribute to some output. Some dont do anything.
         * 
         * metafunctions / functors? function that acts on the type of the input
         */


        [Fact]
        public void SingleIncrement()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("+");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("1", csTree.Variables[0].Value);
            Assert.Equal(CSTType.Number, csTree.Variables[0].Type);
            Assert.Equal(CSTBinding.Constant, csTree.Variables[0].Binding);
        }

        [Fact]
        public void MultipleIncrement()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("++++");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("4", csTree.Variables[0].Value);
        }

        [Fact]
        public void SingleDecrement()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("-");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("-1", csTree.Variables[0].Value);
            Assert.Equal(CSTType.Number, csTree.Variables[0].Type);
        }

        [Fact]
        public void MultipleDecrement()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("+++++-+---");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("2", csTree.Variables[0].Value);
        }

        [Fact]
        public void IncrementPointer()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(">");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("0", csTree.Variables[0].Value);
            Assert.Equal("b", csTree.Variables[1].Name);
            Assert.Equal("0", csTree.Variables[1].Value);
        }

        [Fact]
        public void IncrementPointerAndValue()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(">+");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("0", csTree.Variables[0].Value);
            Assert.Equal("b", csTree.Variables[1].Name);
            Assert.Equal("1", csTree.Variables[1].Value);
        }

        [Fact]
        public void MultipleIncrementPointer()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(">>>");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(4, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("b", csTree.Variables[1].Name);
            Assert.Equal("c", csTree.Variables[2].Name);
            Assert.Equal("d", csTree.Variables[3].Name);
        }

        [Fact]
        public void IncDecPointer()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("><");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("b", csTree.Variables[1].Name);
        }

        [Fact]
        public void IncPointerToSame()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("><>");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("b", csTree.Variables[1].Name);
        }

        [Fact]
        public void LoadInput()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(",");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("input0", csTree.Variables[0].Value);
            Assert.Equal(CSTBinding.Dynamic, csTree.Variables[0].Binding);
            Assert.Equal(CSTType.Number, csTree.Variables[0].Type);
        }

        [Fact]
        public void LoadIncInput()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(",+");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("input0+1", csTree.Variables[0].Value);
        }

        [Fact]
        public void LoadInputIncMany()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(",+++");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("input0+3", csTree.Variables[0].Value);
        }

        [Fact]
        public void LoadInputDecMany()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(",---");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("input0-3", csTree.Variables[0].Value);
        }

        [Fact]
        public void WriteDefaultOutput()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(".");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Outputs);
            Assert.Equal("a", csTree.Outputs[0].Name);
            Assert.Equal("0", csTree.Outputs[0].Value);
        }

        [Fact]
        public void WriteStaticOutput()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(">+++.");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Outputs);
            Assert.Equal("b", csTree.Outputs[0].Name);
            Assert.Equal("3", csTree.Outputs[0].Value);
        }

        [Fact]
        public void WriteDynamicOutput()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse(">,+++.");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Outputs);
            Assert.Equal("b", csTree.Outputs[0].Name);
            Assert.Equal("input0+3", csTree.Outputs[0].Value);
        }

        [Fact]
        public void EmptyLoop()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("[]");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("0", csTree.Variables[0].Value);
        }

        [Fact]
        public void SkippedLoop()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("[>+<+]++");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("2", csTree.Variables[0].Value);
        }

        [Fact]
        public void SkippedNestedLoop()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("[>+<+[-]]++");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Single(csTree.Variables);
            Assert.Equal("a", csTree.Variables[0].Name);
            Assert.Equal("2", csTree.Variables[0].Value);
        }

        [Fact]
        public void StaticLoop()
        {
            Parser parser = new Parser();
            ASTRoot ast = parser.Parse("++++[->+<]");
            CSTDataContext csTree = CommonSemanticConverter.Convert(ast);

            Assert.Equal(2, csTree.Variables.Count);
            assertVariable(new CSTVariable("a", CSTType.Number, "0"), csTree.Variables[0]);
            assertVariable(new CSTVariable("b", CSTType.Number, "4"), csTree.Variables[1]);
        }

        private static void assertVariable(CSTVariable expected, CSTVariable actual)
        {
            Assert.True(expected.Name == actual.Name, $"Variable names differ {expected.Name} != {actual.Name}");
            Assert.True(expected.Value == actual.Value, $"Variable '{actual.Name}' values differ {expected.Value} != {actual.Value}");
        }
    }
}
