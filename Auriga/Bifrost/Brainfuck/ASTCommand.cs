namespace Bifrost.Brainfuck
{
    public class ASTCommand
    {
        public ASTCommandType Type;

        public ASTCommand(ASTCommandType type)
        {
            Type = type;
        }
    }
}