#region usings

using System;
using Composable.DDD;
using NUnit.Framework;

#endregion

namespace Composable.Tests.DDD
{
    [TestFixture]
    public class PersistentEntityTests
    {
        public class Person : PersistentEntity<Person>
        {
            public Person()
            {
            }

            public Person(Guid id) : base(id)
            {
            }
        }

        public class Toddler : Person
        {
            public Toddler()
            {
            }

            public Toddler(Guid id) : base(id)
            {
            }
        }

        [Test]
        public void InstanceEqualsItself()
        {
            var person = new Person();
            AssertAreEqual(person, person);
        }

        [Test]
        public void IntstanceEqualsOtherInstanceWithSameId()
        {
            var lhs = new Person();
            var rhs = new Person(lhs.Id);
            AssertAreEqual(lhs, rhs);
        }

        [Test]
        public void IntstanceNotEqualToinstanceWithOtherId()
        {
            var lhs = new Person(Guid.NewGuid());
            var rhs = new Person(Guid.NewGuid());
            AssertAreNotEqual(lhs, rhs);
        }

        [Test]
        public void IntstancesWithSameIdHasSameHashCode()
        {
            var lhs = new Person();
            var rhs = new Person(lhs.Id);
            Assert.That(lhs.GetHashCode(), Is.EqualTo(rhs.GetHashCode()));
            Assert.That(lhs.GetHashCode() == rhs.GetHashCode(), Is.True);
        }


        [Test]
        public void ComparisonWithRhsNullReturnsFalse()
        {
            var lhs = new Person();
            Assert.That(lhs.Equals(null), Is.False);
            Assert.That(lhs == null, Is.False);
        }

        [Test]
        public void ComparisonWithLhsNullReturnsFalse()
        {
            var rhs = new Person();
            Assert.That(null == rhs, Is.False);
        }

        [Test]
        public void ComparisonWithLhsNullAndRhsNullReturnsTrue()
        {
            Person rhs = null;
            Person lhs = null;
            Assert.That(rhs == lhs, Is.True);
        }

        private static void AssertAreEqual(Person lhs, Person rhs)
        {
            Assert.That(lhs, Is.EqualTo(rhs));
            Assert.That(lhs.Equals(rhs), Is.True);
            Assert.That(Equals(lhs, rhs), Is.True);
            Assert.That(lhs == rhs, Is.True);
            Assert.That(lhs != rhs, Is.Not.True);
        }

        private static void AssertAreNotEqual(Person lhs, Person rhs)
        {
            Assert.That(lhs, Is.Not.EqualTo(rhs));
            Assert.That(lhs.Equals(rhs), Is.Not.True);
            Assert.That(Equals(lhs, rhs), Is.Not.True);
            Assert.That(lhs == rhs, Is.Not.True);
            Assert.That(lhs != rhs, Is.True);
        }
    }
}