using System.Collections.Generic;

namespace Bifrost.Brainfuck
{
    public class ASTRoot
    {
        public List<ASTCommand> Commands { get; } = new List<ASTCommand>();
    }
}