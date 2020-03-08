using Fiche.Collections;

using NUnit.Framework;

namespace Fiche.Tests.Collections
{
    [TestFixture]
    public class CompositeKey_Tests
    {
        private const string STRING_KEY = "abc";
        private const int INT_KEY = 123;
        private const bool BOOL_KEY = true;
        private const byte BYTE_KEY = 28;

        [Test]
        public void CompositeKey_IsEqualToKey1()
        {
            //Arrange
            CompositeKey ck = new CompositeKey(STRING_KEY, INT_KEY);

            //Act
            bool actual = ck == ck.Key1 && ck.Equals(ck.Key1) && ck == STRING_KEY && ck.Equals(STRING_KEY);

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void CompositeKey_IsEqualToKey2()
        {
            //Arrange
            CompositeKey ck = new CompositeKey(STRING_KEY, INT_KEY);

            //Act
            bool actual = ck == ck.Key2 && ck.Equals(ck.Key2) && ck == INT_KEY && ck.Equals(INT_KEY);

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void TwoCompositeKeys_SameKeys_AreEqual()
        {
            //Arrange
            CompositeKey ck1 = new CompositeKey(STRING_KEY, INT_KEY);
            CompositeKey ck2 = new CompositeKey(STRING_KEY, INT_KEY);

            //Act
            bool actual = ck1 == ck2 && ck1.Equals(ck2);

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void TwoCompositeKeys_OneKeyDifferent_AreNotEqual()
        {
            //Arrange
            CompositeKey ck1 = new CompositeKey(BOOL_KEY, INT_KEY);
            CompositeKey ck2 = new CompositeKey(INT_KEY, STRING_KEY);

            //Act
            bool actual = ck1 != ck2 && !ck1.Equals(ck2);

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void TwoCompositeKeys_DifferentKeys_AreNotEqual()
        {
            //Arrange
            CompositeKey ck1 = new CompositeKey(BYTE_KEY, STRING_KEY);
            CompositeKey ck2 = new CompositeKey(INT_KEY, BOOL_KEY);

            //Act
            bool actual = ck1 != ck2 && !ck1.Equals(ck2);

            //Assert
            Assert.IsTrue(actual);
        }
    }
}
