using NUnit.Framework;
using System;

namespace Rtr.Rmp.Framework.UnitTests
{
    /// <summary>
    /// Class represents a collection of unit tests for <see cref="StringParameterValidation"/> class.
    /// </summary>
    [TestFixture]
    internal class StringParameterValidationTests
    {
        /// <summary>
        /// Test cases containing <see cref="IParameterValidation"/> instances.
        /// </summary>
        private static readonly object[] TestCases =
        {
            new object[] { null, false },
            new object[] { new StringParameterValidation(0, int.MaxValue, "dummy"), true },
            new object[] { new ValueParameterValidation<int>(0, int.MaxValue, default(int)), false },
            new object[] { new ValueWithItemsParameterValidation<int>(0, int.MaxValue, default(int), new System.Collections.Generic.List<ParameterValidationEntry<int>>()), false },
        };

        /// <summary>
        /// Verifies that instance of <see cref="StringParameterValidation"/> initialized,
        /// provides correct minimum length.
        /// </summary>
        /// <param name="minLength">Minimum length.</param>
        [TestCase(0)]
        [TestCase(4)]
        public void StringParameterValidation_InitializedWithParameter_MinLengthSetCorrectly(int minLength)
        {
            // arrange
            // act
            var validation = new StringParameterValidation(minLength, int.MaxValue, "dummy");

            // assert
            Assert.AreEqual(minLength, validation.MinLength);
        }

        /// <summary>
        /// Verifies that instance of <see cref="StringParameterValidation"/> initialized,
        /// provides correct maximum length.
        /// </summary>
        /// <param name="maxLength">Maximum length.</param>
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        public void StringParameterValidation_InitializedWithParameter_MaxLengthSetCorrectly(int maxLength)
        {
            // arrange
            // act
            var validation = new StringParameterValidation(0, maxLength, "dummy");

            // assert
            Assert.AreEqual(maxLength, validation.MaxLength);
        }

        /// <summary>
        /// Verifies that instance of <see cref="StringParameterValidation"/> initialized,
        /// provides correct default value.
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        [TestCase("dummy")]
        [TestCase("")]
        public void StringParameterValidation_InitializedWithParameter_DefaultValueSetCorrectly(string defaultValue)
        {
            // arrange
            // act
            var validation = new StringParameterValidation(0, int.MaxValue, defaultValue);

            // assert
            Assert.AreEqual(defaultValue, validation.DefaultValue);
        }

        /// <summary>
        /// Verifies that instance of <see cref="StringParameterValidation"/> initialized,
        /// provides correct default value.
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        [TestCase("dummy")]
        [TestCase("")]
        public void StringParameterValidation_GetDefaultValueCalled_ReturnsCorrectDefautValue(string defaultValue)
        {
            // arrange
            // act
            var validation = new StringParameterValidation(new StringParameterValidation(0, int.MaxValue, defaultValue));

            // assert
            Assert.AreEqual(defaultValue, validation.GetDefaultValue());
        }

        /// <summary>
        /// Verifies that instance of <see cref="StringParameterValidation"/> initialized,
        /// provides correct minimum length.
        /// </summary>
        /// <param name="minLength">Minimum length.</param>
        [TestCase(0)]
        [TestCase(4)]
        public void StringParameterValidation_InitializedWithValidInfo_MinLengthSetCorrectly(int minLength)
        {
            // arrange
            // act
            var validation = new StringParameterValidation(new StringParameterValidation(minLength, int.MaxValue, "dummy"));

            // assert
            Assert.AreEqual(minLength, validation.MinLength);
        }

        /// <summary>
        /// Verifies that instance of <see cref="StringParameterValidation"/> initialized,
        /// provides correct maximum length.
        /// </summary>
        /// <param name="maxLength">Maximum length.</param>
        [TestCase(100)]
        [TestCase(int.MaxValue)]
        public void StringParameterValidation_InitializedWithValidInfo_MaxLengthSetCorrectly(int maxLength)
        {
            // arrange
            // act
            var validation = new StringParameterValidation(new StringParameterValidation(0, maxLength, "dummy"));

            // assert
            Assert.AreEqual(maxLength, validation.MaxLength);
        }

        /// <summary>
        /// Verifies that instance of <see cref="StringParameterValidation"/> initialized,
        /// provides correct default value.
        /// </summary>
        /// <param name="defaultValue">Default value.</param>
        [TestCase("dummy")]
        [TestCase("")]
        public void StringParameterValidation_InitializedWithValidInfo_DefaultValueSetCorrectly(string defaultValue)
        {
            // arrange
            // act
            var validation = new StringParameterValidation(new StringParameterValidation(0, int.MaxValue, defaultValue));

            // assert
            Assert.AreEqual(defaultValue, validation.DefaultValue);
        }

        /// <summary>
        /// Verifies that using <see cref="StringParameterValidation(StringParameterValidation)"/>
        /// with null parameter throws exception.
        /// </summary>
        [Test]
        public void StringParameterValidation_InitializedWithInvalidInfo_ThrowException()
        {
            // arrange
            // act
            void Act()
            {
                var validation = new StringParameterValidation(null);
            }

            // assert
            Assert.Catch(typeof(ArgumentNullException), Act);
        }

        /// <summary>
        /// Verifies that using <see cref="StringParameterValidation(StringParameterValidation)"/>
        /// with null parameter throws exception.
        /// </summary>
        /// <param name="minLength">Minimum length.</param>
        /// <param name="maxLength">Maximum length.</param>
        /// <param name="defaultValue">Default value.</param>
        [TestCase(-1, 3, "ab")]
        [TestCase(4, 3, "ab")]
        [TestCase(3, 6, "ab")]
        [TestCase(3, 6, "abcdefg")]
        public void StringParameterValidation_InitializedWithInvalidParameterRange_ThrowException(int minLength, int maxLength, string defaultValue)
        {
            // arrange
            // act
            void Act()
            {
                var validation = new StringParameterValidation(minLength, maxLength, defaultValue);
            }

            // assert
            Assert.Catch(typeof(ArgumentOutOfRangeException), Act);
        }

        /// <summary>
        /// Verifies that <see cref="StringParameterValidation.Update(IParameterValidation)"/>
        /// returns correct value indicating whether update has been performed or not of
        /// <see cref="StringParameterValidation"/>.
        /// </summary>
        /// <param name="validation">New validation information.</param>
        /// <param name="expectedResult">Expected result string.</param>
        [TestCaseSource(nameof(TestCases))]
        public void StringParameterValidation_Update_ReturnsValidValue(IParameterValidation validation, bool expectedResult)
        {
            // arrange
            var stringParameterValidation = new StringParameterValidation(0, 10, "dummy");

            // act
            bool result = stringParameterValidation.Update(validation);

            // assert
            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// Verifies that using <see cref="ParameterValidationEntry{T}"/>
        /// with null default value throws exception.
        /// </summary>
        [Test]
        public void StringParameterValidation_InitializedWithNullDefaultValue_ThrowException()
        {
            // arrange
            // act
            void Act()
            {
                var stringParameterValidation = new StringParameterValidation(default(int), default(int), null);
            }

            // assert
            Assert.Catch(typeof(ArgumentNullException), Act);
        }

        /// <summary>
        /// Verifies that <see cref="StringParameterValidation.ToString"/>
        /// shows valid string representation of <see cref="StringParameterValidation"/>.
        /// </summary>
        /// <param name="minLength">Minimum length.</param>
        /// <param name="maxLength">Maximum length.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="expectedResult">Expected result string.</param>
        [TestCase(0, 0, "", "0 0, ")]
        [TestCase(0, int.MaxValue, "", "0 2147483647, ")]
        [TestCase(2, int.MaxValue, "dummy", "2 2147483647, dummy")]
        public void StringParameterValidation_ToString_ReturnsValidValue(int minLength, int maxLength, string defaultValue, string expectedResult)
        {
            // arrange
            var info = new StringParameterValidation(new StringParameterValidation(minLength, maxLength, defaultValue));

            // act
            string str = info.ToString();

            // assert
            Assert.AreEqual(expectedResult, str);
        }
    }
}
