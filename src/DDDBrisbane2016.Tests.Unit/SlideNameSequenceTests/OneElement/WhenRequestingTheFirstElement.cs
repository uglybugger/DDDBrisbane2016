using NUnit.Framework;
using Shouldly;

namespace DDDBrisbane2016.Tests.Unit.SlideNameSequenceTests.OneElement
{
    public class WhenRequestingTheFirstElement : GivenASequenceWithOneElement
    {
        private string _firstSlideName;

        protected override void When()
        {
            _firstSlideName = Sequence.GetFirst();
        }

        [Test]
        public void TheElementShouldBeCorrect()
        {
            _firstSlideName.ShouldBe(SequenceWithOneElement.FirstSlideName);
        }
    }
}