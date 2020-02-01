namespace Bifrost.Brainfuck
{
    public enum ASTCommandType
    {
        Decrement,
        Increment,
        DecrementPointer,
        IncrementPointer,
        WriteByte,
        ReadByte,
        JumpForward,
        JumpBack
    }
}