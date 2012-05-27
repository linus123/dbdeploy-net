using System.Collections.Generic;
using Net.Sf.Dbdeploy.Exceptions;
using NUnit.Framework;

namespace Net.Sf.Dbdeploy.Scripts
{
    [TestFixture]
    public class ChangeScriptRepositoryTest
    {
        [Test]
        public void TestGivenASetOfChangeScriptsReturnsThemCorrectly()
        {
            ChangeScript one = new ChangeScript(1);
            ChangeScript two = new ChangeScript(2);
            ChangeScript three = new ChangeScript(3);
            ChangeScript four = new ChangeScript(4);

            ChangeScript[] scripts = {three, two, four, one};

            ChangeScriptRepository repository = new ChangeScriptRepository(new List<ChangeScript>(scripts));

            List<ChangeScript> list = repository.GetOrderedListOfDoChangeScripts();

            Assert.AreEqual(4, list.Count);
            Assert.AreSame(one, list[0]);
            Assert.AreSame(two, list[1]);
            Assert.AreSame(three, list[2]);
            Assert.AreSame(four, list[3]);
        }

        [Test]
        public void TestThrowsWhenConstructedWithAChangeScriptListThatHasDuplicates()
        {
            ChangeScript two = new ChangeScript(2);
            ChangeScript three = new ChangeScript(3);
            ChangeScript anotherTwo = new ChangeScript(2);

            try
            {
                ChangeScript[] scripts = {three, two, anotherTwo};
                new ChangeScriptRepository(new List<ChangeScript>(scripts));
                Assert.Fail("expected exception");
            }
            catch (DuplicateChangeScriptException ex)
            {
                Assert.AreEqual("There is more than one change script with number 2", ex.Message);
            }
        }

        [Test]
        public void TestChangeScriptsMayBeNumberedFromZero()
        {
            ChangeScript zero = new ChangeScript(0);
            ChangeScript four = new ChangeScript(4);


            ChangeScript[] scripts = new ChangeScript[] {zero, four};
            ChangeScriptRepository repository =
                new ChangeScriptRepository(new List<ChangeScript>(scripts));

            List<ChangeScript> list = repository.GetOrderedListOfDoChangeScripts();

            Assert.AreEqual(2, list.Count);
            Assert.AreSame(zero, list[0]);
            Assert.AreSame(four, list[1]);
        }
    }
}