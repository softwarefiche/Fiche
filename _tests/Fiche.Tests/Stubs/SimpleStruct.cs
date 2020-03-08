namespace Fiche.Tests.Stubs
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct SimpleStruct
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public SimpleStruct(int @int) => Int = @int;
        public int Int { get; }
    }
}
