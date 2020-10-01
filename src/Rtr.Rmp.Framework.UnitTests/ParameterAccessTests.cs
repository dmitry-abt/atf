using NUnit.Framework;
using Rtr.Rmp.UnitTests.Common;

namespace Rtr.Rmp.Framework.UnitTests
{
    /// <summary>
    /// Class represents a collection of unit tests for <see cref="ParameterAccess"/> class.
    /// </summary>
    [TestFixture]
    internal class ParameterAccessTests
    {
        /// <summary>
        /// Verifies that instance of <see cref="ParameterAccess"/> initialized,
        /// provides correct handling.
        /// </summary>
        [Test]
        public void ParameterAccess_Initialized_HandlingIsReadWrite()
        {
            var access = new ParameterAccess();
            Assert.AreEqual(access.Handling, ParameterHandling.ReadWrite);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterAccess"/> initialized,
        /// provides correct write protection.
        /// </summary>
        [Test]
        public void ParameterAccess_Initialized_WriteProtectionIsFalse()
        {
            var access = new ParameterAccess();
            Assert.AreEqual(access.IsWriteProtected, false);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterAccess"/> raises <see cref="ParameterAccess.WriteProtectionChanged"/> event,
        /// when <see cref="ParameterAccess.IsWriteProtected"/> is updated.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        /// <param name="updatedValue">Updated value that should trigger event.</param>
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void ParameterAccess_WriteProtectionIsUpdated_WriteProtectionChangedIsRaised(bool initialValue, bool updatedValue)
        {
            // arrange
            var access = new ParameterAccess
            {
                IsWriteProtected = initialValue,
            };

            // act
            void Act()
            {
                access.IsWriteProtected = updatedValue;
            }

            // assert
            Event.Monitor<ParameterWriteProtectionChangedEventArgs>(access, nameof(ParameterAccess.WriteProtectionChanged)).
                When(Act).AssertEventRaised().WithSender(access).WithArgument(x => x.IsWriteProtected, updatedValue);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterAccess"/> does not raise <see cref="ParameterAccess.WriteProtectionChanged"/> event,
        /// when <see cref="ParameterAccess.IsWriteProtected"/> is set to the same value.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        /// <param name="updatedValue">Updated value that should trigger event.</param>
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void ParameterAccess_WriteProtectionIsSetToSameValue_WriteProtectionChangedIsNotRaised(bool initialValue, bool updatedValue)
        {
            // arrange
            var access = new ParameterAccess
            {
                IsWriteProtected = initialValue,
            };

            // act
            void Act()
            {
                access.IsWriteProtected = updatedValue;
            }

            // assert
            Event.Monitor<ParameterWriteProtectionChangedEventArgs>(access, nameof(ParameterAccess.WriteProtectionChanged)).
                When(Act).AssertEventNotRaised();
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterAccess"/> does not raise <see cref="ParameterAccess.WriteProtectionChanged"/> event,
        /// when <see cref="ParameterAccess.IsWriteProtected"/> is updated on a disposed <see cref="ParameterAccess"/> instance.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        /// <param name="updatedValue">Updated value that should trigger event.</param>
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void ParameterAccess_WriteProtectionUpdatedWhenDisposed_WriteProtectionChangedIsNotRaised(bool initialValue, bool updatedValue)
        {
            // arrange
            var access = new ParameterAccess
            {
                IsWriteProtected = initialValue,
            };
            access.Dispose();

            // act
            void Act()
            {
                access.IsWriteProtected = updatedValue;
            }

            // assert
            Event.Monitor<ParameterWriteProtectionChangedEventArgs>(access, nameof(ParameterAccess.WriteProtectionChanged)).
                When(Act).AssertEventNotRaised();
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterAccess"/> raises <see cref="ParameterAccess.HandlingChanged"/> event,
        /// when <see cref="ParameterAccess.Handling"/> is updated.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        /// <param name="updatedValue">Updated value that should trigger event.</param>
        [TestCase(ParameterHandling.ReadWrite, ParameterHandling.Read)]
        [TestCase(ParameterHandling.Read, ParameterHandling.Write)]
        [TestCase(ParameterHandling.Write, ParameterHandling.NoAccess)]
        [TestCase(ParameterHandling.NoAccess, ParameterHandling.ReadWrite)]
        public void ParameterAccess_HandlingIsUpdated_HandlingChangedIsRaised(ParameterHandling initialValue, ParameterHandling updatedValue)
        {
            // arrange
            var access = new ParameterAccess
            {
                Handling = initialValue,
            };

            // act
            void Act()
            {
                access.Handling = updatedValue;
            }

            // assert
            Event.Monitor<ParameterHandlingChangedEventArgs>(access, nameof(ParameterAccess.HandlingChanged)).
                When(Act).AssertEventRaised().WithSender(access).WithArgument(x => x.Handling, updatedValue);
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterAccess"/> does not raise <see cref="ParameterAccess.HandlingChanged"/> event,
        /// when <see cref="ParameterAccess.Handling"/> is set to the same value.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        /// <param name="updatedValue">Updated value that should trigger event.</param>
        [TestCase(ParameterHandling.NoAccess, ParameterHandling.NoAccess)]
        [TestCase(ParameterHandling.Read, ParameterHandling.Read)]
        [TestCase(ParameterHandling.Write, ParameterHandling.Write)]
        [TestCase(ParameterHandling.ReadWrite, ParameterHandling.ReadWrite)]
        public void ParameterAccess_HandlingIsSetToSameValue_HandlingChangedIsNotRaised(ParameterHandling initialValue, ParameterHandling updatedValue)
        {
            // arrange
            var access = new ParameterAccess
            {
                Handling = initialValue,
            };

            // act
            void Act()
            {
                access.Handling = updatedValue;
            }

            // assert
            Event.Monitor<ParameterHandlingChangedEventArgs>(access, nameof(ParameterAccess.HandlingChanged)).
                When(Act).AssertEventNotRaised();
        }

        /// <summary>
        /// Verifies that instance of <see cref="ParameterAccess"/> does not raise <see cref="ParameterAccess.HandlingChanged"/> event,
        /// when <see cref="ParameterAccess.Handling"/> is updated on a disposed <see cref="ParameterAccess"/> instance.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        /// <param name="updatedValue">Updated value that should trigger event.</param>
        [TestCase(ParameterHandling.ReadWrite, ParameterHandling.Read)]
        [TestCase(ParameterHandling.Read, ParameterHandling.Write)]
        [TestCase(ParameterHandling.Write, ParameterHandling.NoAccess)]
        [TestCase(ParameterHandling.NoAccess, ParameterHandling.ReadWrite)]
        public void ParameterAccess_HandlingUpdatedWhenDisposed_HandlingChangedIsNotRaised(ParameterHandling initialValue, ParameterHandling updatedValue)
        {
            // arrange
            var access = new ParameterAccess
            {
                Handling = initialValue,
            };
            access.Dispose();

            // act
            void Act()
            {
                access.Handling = updatedValue;
            }

            // assert
            Event.Monitor<ParameterHandlingChangedEventArgs>(access, nameof(ParameterAccess.HandlingChanged)).
                When(Act).AssertEventNotRaised();
        }
    }
}
