using System;

namespace Nut.Tests
{
    /// <summary>
    /// Defects that are reproducible today. Each test asserts the WRONG behaviour on
    /// purpose, so the suite stays green until someone fixes the defect — at which point
    /// the test fails and has to be rewritten as the correct expectation. Every case here
    /// was reproduced against the library before being written down.
    /// </summary>
    [TestFixture]
    public class KnownDefects
    {
        /// <summary>Callers cannot catch this selectively.</summary>
        [Test]
        public void OverTheLimitThrowsBareException()
        {
            var ex = Assert.Throws<Exception>(() => 1000000000000L.ToText(Language.English));
            Assert.That(ex.GetType(), Is.EqualTo(typeof(Exception)));
        }
    }
}
