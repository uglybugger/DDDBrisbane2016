using NUnit.Framework;
using Shouldly;

namespace DDDBrisbane2016.Tests.Unit.SlideNameSequenceTests.OneElement
{
    public class WhenRequestingTheNextElement : GivenASequenceWithOneElement
    {
        private bool _elementFound;
        private string _slideName;

        protected override void When()
        {
            var firstSlideName = Sequence.GetFirst();
            _elementFound = Sequence.TryGetNext(firstSlideName, out _slideName);
        }

        [Test]
        public void TheResponseShouldBeUnsuccessful()
        {
            _elementFound.ShouldBeFalse();
        }
    }
}