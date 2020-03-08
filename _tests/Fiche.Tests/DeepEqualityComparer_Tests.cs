using Fiche.Tests.Stubs;

using NUnit.Framework;

namespace Fiche.Tests
{
    [TestFixture]
    public class DeepEqualityComparer_Tests
    {
        [Test]
        public void NonGenericComparer_SimpleObjects_AreEqual()
        {
            //Arrange
            SimpleObject simpleStub1 = Fakers.SimpleObjectFaker.Generate();
            SimpleObject simpleStub2 = new SimpleObject()
            {
                IntProperty = simpleStub1.IntProperty,
                StringProperty = simpleStub1.StringProperty
            };
            DeepEqualityComparer deepEqualityComparer = new DeepEqualityComparer();

            //Act
            bool actual = deepEqualityComparer.Equals(simpleStub1, simpleStub2);

            //Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void GenericComparer_SimpleObjects_AreEqual()
        {
            //Arrange
            SimpleObject simpleStub1 = Fakers.SimpleObjectFaker.Generate();
            SimpleObject simpleStub2 = new SimpleObject()
            {
                IntProperty = simpleStub1.IntProperty,
                StringProperty = simpleStub1.StringProperty
            };
            DeepEqualityComparer<SimpleObject> deepEqualityComparer = new DeepEqualityComparer<SimpleObject>();

            //Act
            bool actual = deepEqualityComparer.Equals(simpleStub1, simpleStub2);

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void NonGenericComparer_SimpleObjects_AreNotEqual()
        {
            //Arrange
            SimpleObject simpleStub1 = Fakers.SimpleObjectFaker.Generate();
            SimpleObject simpleStub2 = Fakers.SimpleObjectFaker.Generate();
            DeepEqualityComparer deepEqualityComparer = new DeepEqualityComparer();

            //Act
            bool actual = deepEqualityComparer.Equals(simpleStub1, simpleStub2);

            //Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void GenericComparer_SimpleObjects_AreNotEqual()
        {
            //Arrange
            SimpleObject simpleStub1 = Fakers.SimpleObjectFaker.Generate();
            SimpleObject simpleStub2 = Fakers.SimpleObjectFaker.Generate();
            DeepEqualityComparer<SimpleObject> deepEqualityComparer = new DeepEqualityComparer<SimpleObject>();

            //Act
            bool actual = deepEqualityComparer.Equals(simpleStub1, simpleStub2);

            //Assert
            Assert.IsFalse(actual);
        }
        [Test]
        public void NonGenericComparer_ComplexObjects_AreEqual()
        {
            //Arrange
            ComplexObject complexStub1 = Fakers.ComplexObjectFaker.Generate();
            ComplexObject complexStub2 = new ComplexObject()
            {
                LongProperty = complexStub1.LongProperty,
                SimpleProperty = new SimpleObject()
                {
                    IntProperty = complexStub1.SimpleProperty.IntProperty,
                    StringProperty = complexStub1.SimpleProperty.StringProperty
                }
            };
            DeepEqualityComparer deepEqualityComparer = new DeepEqualityComparer();

            //Act
            bool actual = deepEqualityComparer.Equals(complexStub1, complexStub2);

            //Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void GenericComparer_ComplexObjects_AreEqual()
        {
            //Arrange
            ComplexObject complexStub1 = Fakers.ComplexObjectFaker.Generate();
            ComplexObject complexStub2 = new ComplexObject()
            {
                LongProperty = complexStub1.LongProperty,
                SimpleProperty = new SimpleObject()
                {
                    IntProperty = complexStub1.SimpleProperty.IntProperty,
                    StringProperty = complexStub1.SimpleProperty.StringProperty
                }
            };
            DeepEqualityComparer<ComplexObject> deepEqualityComparer = new DeepEqualityComparer<ComplexObject>();

            //Act
            bool actual = deepEqualityComparer.Equals(complexStub1, complexStub2);

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void NonGenericComparer_ComplexObjects_AreNotEqual()
        {
            //Arrange
            ComplexObject complexStub1 = Fakers.ComplexObjectFaker.Generate();
            ComplexObject complexStub2 = Fakers.ComplexObjectFaker.Generate();
            DeepEqualityComparer<ComplexObject> deepEqualityComparer = new DeepEqualityComparer<ComplexObject>();

            //Act
            bool actual = deepEqualityComparer.Equals(complexStub1, complexStub2);

            //Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void GenericComparer_ComplexObjects_AreNotEqual()
        {
            //Arrange
            ComplexObject complexStub1 = Fakers.ComplexObjectFaker.Generate();
            ComplexObject complexStub2 = Fakers.ComplexObjectFaker.Generate();
            DeepEqualityComparer<ComplexObject> deepEqualityComparer = new DeepEqualityComparer<ComplexObject>();

            //Act
            bool actual = deepEqualityComparer.Equals(complexStub1, complexStub2);

            //Assert
            Assert.IsFalse(actual);
        }
        [Test]
        public void NonGenericComparer_NullIsNotEqualToAnInstance()
        {
            //Arrange
            ComplexObject complexStub = Fakers.ComplexObjectFaker.Generate();
            DeepEqualityComparer deepEqualityComparer = new DeepEqualityComparer();

            //Act
            bool actual = deepEqualityComparer.Equals(null, complexStub);

            //Assert
            Assert.IsFalse(actual);
        }
        [Test]
        public void GenericComparer_NullIsNotEqualToAnInstance()
        {
            //Arrange
            ComplexObject complexStub = Fakers.ComplexObjectFaker.Generate();
            DeepEqualityComparer<ComplexObject> deepEqualityComparer = new DeepEqualityComparer<ComplexObject>();

            //Act
            bool actual = deepEqualityComparer.Equals(null, complexStub);

            //Assert
            Assert.IsFalse(actual);
        }
    }
}