namespace Fiche.Tests.Stubs
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct ComplexStruct
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public ComplexStruct(string str, SimpleStruct simpleStruct)
        {
            Str = str;
            SimpleStruct = simpleStruct;
        }
        public string Str { get; }
        public SimpleStruct SimpleStruct { get; }
    }
}
