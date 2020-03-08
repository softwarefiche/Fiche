using System.Collections;

namespace Fiche.Tests.Stubs
{
    public class UndisposableEnumerator : IEnumerator
    {
        public object Current => default;

        public bool MoveNext() => default;

        public void Reset() { }
    }
}
