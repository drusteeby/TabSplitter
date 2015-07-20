using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace TabSplitter.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EveryoneBreakEven()
        {
            DebtService service = new DebtService();
            service.PersonList.Add(new Person("Adam"));
            service.PersonList.Add(new Person("Kate"));
            service.PersonList.Add(new Person("Dru"));

            service.addDebt(100, service.PersonList[0], service.PersonList[1],service.PersonList[2]);
            service.addDebt(100, service.PersonList[1], service.PersonList[2], service.PersonList[0]);
            service.addDebt(100, service.PersonList[2], service.PersonList[0], service.PersonList[1]);

            service.Combine();
            
            Assert.AreEqual(service.DebtList.Count, 0);

        }
    }
}
