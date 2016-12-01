using NUnit.Framework;

namespace DDDBrisbane2016.Tests.Unit.SlideNameSequenceTests
{
    public abstract class UnitTest
    {
        [SetUp]
        public void SetUp()
        {
            Given();
            When();
        }

        protected abstract void Given();
        protected abstract void When();
    }
}