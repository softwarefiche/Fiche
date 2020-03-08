using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

using Fiche.Extensions;
using Fiche.Tests.Stubs;

using NUnit.Framework;

namespace Fiche.Tests.Extensions
{
    [TestFixture]
    public class ObjectExtensions_Tests
    {
        private const string MAGIC_STRING = "MAGIC";
        private const int MAGIC_INT = 10;
        private const string NULL_STRING = default;
        private const int NOT_NULL_INT = 0;
        private readonly int? NULLABLE_NULL_INT = default;
        private readonly int? NULLABLE_NOT_NULL_INT = 0;

        #region IsNull Tests
        [Test]
        [TestCase(null, ExpectedResult = true, Description = "Explicit null")]
        [TestCase(NULL_STRING, ExpectedResult = true, Description = "Null reference-type")]
        [TestCase(NOT_NULL_INT, ExpectedResult = false, Description = "Value-type")]
        public bool IsNull(object value)
        {
            //Act
            bool actual = value.IsNull();

            //Assert
            return actual;
        }

        [Test]
        public void IsNull_NullableValueType_NullValueIsNull()
        {
            //Act
            bool actual = this.NULLABLE_NULL_INT.IsNull();

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void IsNull_NullableValueType_NotNullValueIsNotNull()
        {
            //Act
            bool actual = this.NULLABLE_NOT_NULL_INT.IsNull();

            //Assert
            Assert.IsFalse(actual);
        }
        [Test]
        public void IsNull_NullType()
        {
            //Arrange
            ObjectExtensions.NullTypes.Add(typeof(DBNull));

            //Act
            bool actual = DBNull.Value.IsNull();

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void IsNull_NullValue()
        {
            //Arrange
            ObjectExtensions.NullValues.Add(NOT_NULL_INT);

            //Act
            bool actual = NOT_NULL_INT.IsNull();

            //Assert
            Assert.IsTrue(actual);
        }
        #endregion

        #region DeepClone Tests
        [Test]
        public void DeepClone_ShallowObjects_AreEqual()
        {
            //Arrange
            SimpleObject obj = Fakers.SimpleObjectFaker.Generate();

            //Act
            SimpleObject cloned = obj.DeepClone();

            //Assert
            Assert.IsTrue(obj.DeepEquals(cloned));
        }
        [Test]
        public void DeepClone_DeepObjects_AreEqual()
        {
            //Arrange
            ComplexObject obj = Fakers.ComplexObjectFaker.Generate();

            //Act
            ComplexObject cloned = obj.DeepClone();

            //Assert
            Assert.IsTrue(obj.DeepEquals(cloned));
        }
        [Test]
        public void DeepClone_IEnumerable_AreEqual()
        {
            //Arrange
            object item0 = Fakers.ComplexObjectFaker.Generate();
            object item1 = Fakers.SimpleObjectFaker.Generate();
            IEnumerable createSequence()
            {
                yield return item0;
                yield return item1;
            };
            IEnumerable sequence = createSequence();

            //Act
            IEnumerable cloned = sequence.DeepClone();

            //Assert
            Assert.IsTrue(sequence.DeepEquals(cloned));
        }
        [Test]
        public void DeepClone_GenericLists_AreEqual()
        {
            //Arrange
            List<object> list = new List<object>
            {
                Fakers.ComplexObjectFaker.Generate(),
                Fakers.SimpleObjectFaker.Generate()
            };

            //Act
            List<object> cloned = list.DeepClone();

            //Assert
            Assert.IsTrue(list.DeepEquals(cloned));
        }
        [Test]
        public void DeepClone_GenericCollections_AreEqual()
        {
            //Arrange
            Collection<object> collection = new Collection<object>
            {
                Fakers.ComplexObjectFaker.Generate(),
                Fakers.SimpleObjectFaker.Generate()
            };

            //Act
            Collection<object> cloned = collection.DeepClone();

            //Assert
            Assert.IsTrue(collection.DeepEquals(cloned));
        }
        [Test]
        public void DeepClone_Array_AreEqual()
        {
            //Arrange
            object[] arr = new object[2]
            {
                 Fakers.ComplexObjectFaker.Generate(),
                 Fakers.SimpleObjectFaker.Generate()
            };

            //Act
            object[] cloned = arr.DeepClone();

            //Assert
            Assert.IsTrue(arr.DeepEquals(cloned));
        }
        [Test]
        public void DeepClone_PrimitiveObjects_AreEqual()
        {
            //Act
            int cloned = NOT_NULL_INT.DeepClone();

            //Assert
            NOT_NULL_INT.DeepEquals(cloned);
        }
        #endregion

        #region Copy Tests
        private static SimpleStruct SimpleStructFactory()
            => new SimpleStruct(Fakers.Faker.Random.Int(min: 0, max: 9));
        private static ComplexStruct ComplexStructFactory()
            => new ComplexStruct(Fakers.Faker.Random.String2(3), SimpleStructFactory());

        [Test]
        public void Copy_NullExpression_Succeeds()
        {
            //Arrange
            ComplexStruct complexStructStub = ComplexStructFactory();

            //Act
            ComplexStruct copy = complexStructStub.Copy(null);

            //Assert
            Assert.IsTrue(complexStructStub.DeepEquals(copy));
        }
        [Test]
        public void Copy_ShallowExpression_SetsValue()
        {
            //Arrange
            ComplexStruct complexStructStub = ComplexStructFactory();

            //Act
            ComplexStruct copy = complexStructStub.Copy(cs => cs.Str, MAGIC_STRING);

            //Assert
            Assert.IsFalse(complexStructStub.DeepEquals(copy));
            Assert.AreNotEqual(complexStructStub.Str, copy.Str);
            Assert.AreEqual(MAGIC_STRING, copy.Str);
        }

        [Test]
        public void Copy_DeepExpression_SetsValue()
        {
            //Arrange
            ComplexStruct complexStructStub = ComplexStructFactory();

            //Act
            ComplexStruct copy = complexStructStub.Copy(cs => cs.SimpleStruct.Int, MAGIC_INT);

            //Assert
            Assert.IsFalse(complexStructStub.DeepEquals(copy));
            Assert.AreNotEqual(complexStructStub.SimpleStruct.Int, copy.SimpleStruct.Int);
            Assert.AreEqual(MAGIC_INT, copy.SimpleStruct.Int);
        }
        [Test]
        public void Copy_ConstantExpression_ThrowsException()
        {
            //Arrange
            ComplexStruct complexStructStub = ComplexStructFactory();

            //Act, Assert
            Exception ex = Assert.Catch(() => complexStructStub.Copy(cs => MAGIC_INT, 20));
        }
        [Test]
        public void Copy_UnaryExpressions_AreIgnoredAndValueIsSet()
        {
            //Arrange
            ComplexStruct complexStructStub = ComplexStructFactory();

            //Act
            ComplexStruct copy = complexStructStub.Copy(cs => (short)(((ComplexStruct)(object)cs).SimpleStruct.Int), MAGIC_INT);

            //Assert
            Assert.IsFalse(complexStructStub.DeepEquals(copy));
            Assert.AreNotEqual(complexStructStub.SimpleStruct.Int, copy.SimpleStruct.Int);
            Assert.AreEqual(MAGIC_INT, copy.SimpleStruct.Int);
        }
        [Test]
        public void Copy_MultipleExpressions_SetValues()
        {
            //Arrange
            ComplexStruct complexStructStub = ComplexStructFactory();

            //Act
            ComplexStruct copy = complexStructStub.Copy(new Dictionary<Expression<Func<ComplexStruct, object>>, object>()
            {
                { cs => cs.Str, MAGIC_STRING },
                { cs => cs.SimpleStruct.Int, MAGIC_INT }
            });

            //Assert
            Assert.AreEqual(MAGIC_STRING, copy.Str);
            Assert.AreNotEqual(MAGIC_STRING, complexStructStub.Str);
            Assert.AreEqual(MAGIC_INT, copy.SimpleStruct.Int);
            Assert.AreNotEqual(MAGIC_INT, complexStructStub.SimpleStruct.Int);
        }
        [Test]
        public void Copy_IdenticalMultipleExpressions_Fails()
        {
            //Arrange
            ComplexStruct complexStructStub = ComplexStructFactory();

            //Act, Assert
            Assert.Catch(() => complexStructStub.Copy(new Dictionary<Expression<Func<ComplexStruct, object>>, object>()
            {
                { cs => cs.Str, MAGIC_STRING },
                { cs => cs.Str, MAGIC_STRING }
            }));
        }
        #endregion
    }
}
