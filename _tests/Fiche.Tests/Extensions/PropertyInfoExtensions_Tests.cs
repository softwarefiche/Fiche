using System;
using System.Reflection;

using Fiche.Extensions;
using Fiche.Tests.Stubs;

using NUnit.Framework;

namespace Fiche.Tests.Extensions
{
    [TestFixture]
    public class PropertyInfoExtensions_Tests
    {
        [Test]
        public void IsAutoProperty_ForAutoProperties_ReturnsTrue()
        {
            //Arrange
            Type stubType = typeof(AutoPropertyStub);
            PropertyInfo autoProperty = stubType.GetProperty(nameof(AutoPropertyStub.AutoProperty));

            //Act
            bool actual = autoProperty.IsAutoProperty();

            //Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void IsAutoProperty_ForBodyProperties_ReturnsFalse()
        {
            //Arrange
            Type stubType = typeof(AutoPropertyStub);
            PropertyInfo autoProperty = stubType.GetProperty(nameof(AutoPropertyStub.BodyProperty));

            //Act
            bool actual = autoProperty.IsAutoProperty();

            //Assert
            Assert.IsFalse(actual);
        }
    }
}
