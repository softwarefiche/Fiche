using System;

using Fiche.Tests.Stubs;

using NUnit.Framework;

namespace Fiche.Tests
{
    [TestFixture]
    public class DisposableEnumerator_Tests
    {
        [Test]
        public void DisposedEnumerator_IsDisposed()
        {
            //Arrange
            DisposableEnumerator enumerator = new DisposableEnumerator(new DisposedEnumerator<SimpleObject>());
            enumerator.Dispose();

            //Act
            void actual()
            {
                enumerator.Reset();
            }

            //Assert
            Assert.Throws<ObjectDisposedException>(actual);
        }
        [Test]
        public void UndisposableEnumerator_IsDisposalIgnored()
        {
            //Arrange
            DisposableEnumerator enumerator = new DisposableEnumerator(new UndisposableEnumerator());
            enumerator.Dispose();

            //Act
            void actual()
            {
                enumerator.Reset();
            }

            //Assert
            Assert.DoesNotThrow(actual);
        }
    }
}
