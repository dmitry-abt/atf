using NUnit.Framework;
using System;

namespace Rtr.Rmp.Framework.UnitTests
{
    /// <summary>
    /// Class represents a collection of unit tests for <see cref="ParameterUnit"/> class.
    /// </summary>
    [TestFixture]
    internal class ParameterUnitTests
    {
        /// <summary>
        /// Test cases containing <see cref="IParameter"/> instances.
        /// </summary>
        private static readonly object[] TestCases =
        {
            new object[] { null, string.Empty },
            new object[] { ParameterFactory.Create("dummy"), "dummy" },
        };

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/> initialized,
        /// provides correct device unit type.
        /// </summary>
        [Test]
        public void ParameterUnit_Initialized_DeviceUnitTypeSetCorrectly()
        {
            // arrange
            // act
            var unit = new ParameterUnit();

            // assert
            Assert.AreEqual(ParameterUnitType.Constant, unit.DeviceUnitType);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/> initialized,
        /// provides correct constant unit.
        /// </summary>
        [Test]
        public void ParameterUnit_Initialized_ConstantUnitSetCorrectly()
        {
            // arrange
            // act
            var unit = new ParameterUnit();

            // assert
            Assert.AreEqual(string.Empty, unit.ConstantUnit);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/>
        /// stores valid <see cref="ParameterUnit.UnitType"/>
        /// when <see cref="ParameterUnit.DeviceUnit"/> is not null.
        /// </summary>
        [Test]
        public void ParameterUnit_DeviceUnitSet_ReturnsValidValue()
        {
            // arrange
            IParameter parameter = ParameterFactory.Create("dummy");

            // act
            var unit = new ParameterUnit
            {
                DeviceUnit = parameter,
            };

            // assert
            Assert.AreEqual(parameter, unit.DeviceUnit);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/>
        /// stores valid <see cref="ParameterUnit.UnitType"/>
        /// when <see cref="ParameterUnit.DeviceUnit"/> is set to null.
        /// </summary>
        [Test]
        public void ParameterUnit_DeviceUnitSetNull_DeviceUnitTypeReturnsValidValue()
        {
            // arrange
            IParameter parameter = null;

            // act
            var unit = new ParameterUnit
            {
                DeviceUnit = parameter,
            };

            // assert
            Assert.AreEqual(ParameterUnitType.Constant, unit.DeviceUnitType);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/> initialized,
        /// device unit type updates correctly when parameter type
        /// is <see cref="ParameterType.Enumerated"/>.
        /// </summary>
        [Test]
        public void ParameterUnit_DeviceUnitSetWithEnumeratedUnitType_DeviceUnitTypeReturnsValidValue()
        {
            // arrange
            IParameter parameter = ParameterFactory.Create("dummy", ParameterType.Enumerated);

            // act
            var unit = new ParameterUnit
            {
                DeviceUnit = parameter,
            };

            // assert
            Assert.AreEqual(ParameterUnitType.Enumerated, unit.DeviceUnitType);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/> initialized,
        /// device unit type updates correctly when parameter type
        /// is not <see cref="ParameterType.Enumerated"/>.
        /// </summary>
        /// <param name="parameterType">Parameter type.</param>
        [TestCase(ParameterType.Ascii)]
        [TestCase(ParameterType.Bitfield)]
        [TestCase(ParameterType.Date)]
        [TestCase(ParameterType.Float)]
        [TestCase(ParameterType.PackedAscii)]
        [TestCase(ParameterType.Undefined)]
        [TestCase(ParameterType.UnsignedInteger)]
        public void ParameterUnit_DeviceUnitSetWithNotEnumeratedUnitType_DeviceUnitTypeReturnsValidValue(ParameterType parameterType)
        {
            // arrange
            IParameter parameter = ParameterFactory.Create("dummy", parameterType);

            // act
            var unit = new ParameterUnit
            {
                DeviceUnit = parameter,
            };

            // assert
            Assert.AreEqual(ParameterUnitType.Value, unit.DeviceUnitType);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/> raises
        /// <see cref="ParameterUnit.RaiseDeviceUnitChanged"/> event,
        /// when device unit is updated.
        /// </summary>
        [Test]
        public void ParameterUnit_DeviceUnitIsUpdated_DeviceUnitChangedIsRaised()
        {
            // arrange
            var parameter = new ParameterUnit();
            bool eventRaised = false;
            parameter.DeviceUnitChanged += (s, e) => { eventRaised = true; };

            // act
            parameter.DeviceUnit = ParameterFactory.Create("dummy");

            // assert
            Assert.IsTrue(eventRaised);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/> raises
        /// <see cref="ParameterUnit.RaiseDeviceUnitChanged"/> event
        /// when null device unit is updated.
        /// </summary>
        [Test]
        public void ParameterUnit_DeviceUnitWithNullValueIsUpdated_DeviceUnitChangedIsRaised()
        {
            // arrange
            var parameter = new ParameterUnit
            {
                DeviceUnit = null,
            };

            bool eventRaised = false;
            parameter.DeviceUnitChanged += (s, e) => { eventRaised = true; };

            // act
            parameter.DeviceUnit = ParameterFactory.Create("dummy");

            // assert
            Assert.IsTrue(eventRaised);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/> raises
        /// <see cref="ParameterUnit.RaiseDeviceUnitChanged"/> event
        /// when <see cref="ParameterUnit.ConstantUnit"/> is updated.
        /// </summary>
        [Test]
        public void ParameterUnit_ConstantUnitUpdated_DeviceUnitChangedIsRaised()
        {
            // arrange
            var parameter = new ParameterUnit();

            bool eventRaised = false;
            parameter.DeviceUnitChanged += (s, e) => { eventRaised = true; };

            // act
            parameter.ConstantUnit = "dummy";

            // assert
            Assert.IsTrue(eventRaised);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/>
        /// does not raise <see cref="ParameterUnit.DeviceUnitChanged"/> event,
        /// when device unit is updated on a disposed instance.
        /// </summary>
        [Test]
        public void ParameterUnit_DeviceUnitUpdatedWhenDisposed_DeviceUnitChangedIsNotRaised()
        {
            // arrange
            var parameter = new ParameterUnit();
            bool eventRaised = false;
            parameter.DeviceUnitChanged += (s, e) => { eventRaised = true; };
            parameter.Dispose();

            // act
            parameter.DeviceUnit = ParameterFactory.Create("dummy");

            // assert
            Assert.IsFalse(eventRaised);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/>
        /// does not raise <see cref="ParameterUnit.DeviceUnitChanged"/> event,
        /// when device unit type is <see cref="ParameterUnitType.Constant"/> and device unit is updated on a disposed instance.
        /// </summary>
        [Test]
        public void ParameterUnit_DeviceUnitWithNullValueUpdatedWhenDisposed_DeviceUnitChangedIsNotRaised()
        {
            // arrange
            var parameter = new ParameterUnit
            {
                DeviceUnit = null,
            };

            bool eventRaised = false;
            parameter.DeviceUnitChanged += (s, e) => { eventRaised = true; };
            parameter.Dispose();

            // act
            parameter.DeviceUnit = ParameterFactory.Create("dummy");

            // assert
            Assert.IsFalse(eventRaised);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/>
        /// does not raise <see cref="ParameterUnit.DeviceUnitChanged"/> event
        /// when <see cref="ParameterUnit.DeviceUnit"/> is set and <see cref="ParameterUnit.ConstantUnit"/> is updated.
        /// </summary>
        [Test]
        public void ParameterUnit_ConstantUnitUpdatedWhenDeviceUnitSet_DeviceUnitChangedIsNotRaised()
        {
            // arrange
            var parameter = new ParameterUnit
            {
                DeviceUnit = ParameterFactory.Create("dummy"),
            };

            bool eventRaised = false;
            parameter.DeviceUnitChanged += (s, e) => { eventRaised = true; };

            // act
            parameter.ConstantUnit = "dummy";

            // assert
            Assert.IsFalse(eventRaised);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/>
        /// throws exception when <see cref="ParameterUnit.ConstantUnit"/> is set to null.
        /// </summary>
        [Test]
        public void ParameterUnit_ConstantUnitSetNull_ThrowException()
        {
            // arrange
            // act
            void Act()
            {
                var parameter = new ParameterUnit
                {
                    ConstantUnit = null,
                };
            }

            // assert
            Assert.Catch(typeof(ArgumentNullException), Act);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterUnit"/> raises
        /// <see cref="ParameterUnit.RaiseDeviceUnitChanged"/> event,
        /// that contains valid sender object.
        /// </summary>
        [Test]
        public void ParameterUnit_DeviceUnitIsUpdated_DeviceUnitChangedContainsValidSender()
        {
            // arrange
            var parameter = new ParameterUnit();
            object sender = null;
            parameter.DeviceUnitChanged += (s, e) => { sender = s; };

            // act
            parameter.DeviceUnit = ParameterFactory.Create("dummy");

            // assert
            Assert.AreEqual(sender, parameter);
        }

        /// <summary>
        /// Verifies that <see cref="ParameterUnit.ToString"/>
        /// shows valid string representation of <see cref="ParameterUnit"/>.
        /// </summary>
        /// <param name="parameter">Device unit parameter.</param>
        /// <param name="expectedResult">Expected result string.</param>
        [TestCaseSource(nameof(TestCases))]
        public void ParameterUnit_ToString_ReturnsValidValue(IParameter parameter, string expectedResult)
        {
            // arrange
            var parameterUnit = new ParameterUnit
            {
                DeviceUnit = parameter,
            };

            // act
            string str = parameterUnit.ToString();

            // assert
            Assert.AreEqual(expectedResult, str);
        }
    }
}
