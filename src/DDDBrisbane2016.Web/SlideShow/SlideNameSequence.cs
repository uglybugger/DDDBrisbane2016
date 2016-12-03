using System;
using System.Collections.Generic;
using System.Linq;

namespace DDDBrisbane2016.Web.SlideShow
{
    public abstract class SlideNameSequence
    {
        private readonly List<string> _slideNames;

        protected SlideNameSequence(params string[] slideNames)
        {
            if (slideNames.Length == 0) throw new ArgumentException(nameof(slideNames));

            _slideNames = slideNames.Distinct().ToList();
        }

        public string GetFirst()
        {
            var first = _slideNames.First();
            return first;
        }

        public bool TryGetNext(string current, out string next)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));

            next = null;
            var currentIndex = _slideNames.IndexOf(current);
            if (currentIndex < 0) return false;
            if (currentIndex == _slideNames.Count - 1) return false;
            next = _slideNames[currentIndex + 1];
            return true;
        }

        public bool TryGetPrevious(string current, out string previous)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));

            previous = null;
            var currentIndex = _slideNames.IndexOf(current);
            if (currentIndex < 0) return false; // not found
            if (currentIndex == 0) return false; // already at the beginning
            previous = _slideNames[currentIndex - 1];
            return true;
        }
    }
}