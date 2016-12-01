using DDDBrisbane2016.Web.SlideShow;

namespace DDDBrisbane2016.Tests.Unit.SlideNameSequenceTests.OneElement
{
    public abstract class GivenASequenceWithOneElement : UnitTest
    {
        protected SlideNameSequence Sequence;

        protected override void Given()
        {
            Sequence = new SequenceWithOneElement();
        }

        protected class SequenceWithOneElement : SlideNameSequence
        {
            public const string FirstSlideName = "FirstSlide";

            public SequenceWithOneElement() : base(FirstSlideName)
            {
            }
        }
    }
}