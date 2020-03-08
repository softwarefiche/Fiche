using System;
using System.Collections;

using Fiche.Collections.Extensions;

using NUnit.Framework;

namespace Fiche.Tests.Collections.Extensions
{
    [TestFixture]
    public class EnumerableExtensions_Tests
    {
        private const string NonGeneric_SequenceItem0 = "Item0";
        private const int NonGeneric_SequenceItem1 = 1;

        [Test(Description = "Tested on 2-items sequence")]
        [TestCase(-1, Description = "Below range", ExpectedResult = typeof(ArgumentOutOfRangeException))]
        [TestCase(0, Description = "First", ExpectedResult = null)]
        [TestCase(1, Description = "Second (middle)", ExpectedResult = null)]
        [TestCase(2, Description = "Last", ExpectedResult = null)]
        [TestCase(4, Description = "Above range", ExpectedResult = typeof(ArgumentOutOfRangeException))]
        public object EnumerableInsert_InsertAtIndex(int index)
        {
            //Arrange
            const bool insertedItem = false;
            IEnumerable createSequence()
            {
                yield return NonGeneric_SequenceItem0;
                yield return NonGeneric_SequenceItem1;
            };

            //Act
            Type exceptionType = null;
            IEnumerable actual = createSequence();
            try
            { actual = actual.EnumerableInsert(index, insertedItem); }
            catch (Exception ex)
            { exceptionType = ex.GetType(); }

            //Assert
            try
            {
                int currentIndex = -1;
                foreach (object item in actual)
                {
                    ++currentIndex;
                    if (index == currentIndex)
                    {
                        Assert.AreEqual(insertedItem, item);
                        break;
                    }
                }
                if (currentIndex < index - 1)
                    exceptionType = typeof(ArgumentOutOfRangeException);
            }
            catch (Exception ex)
            { exceptionType = ex.GetType(); }
            return exceptionType;
        }

    }
}
