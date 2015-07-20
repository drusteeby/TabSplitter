using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabSplitter;

namespace TabSplitter.UnitTest
{
    [TestFixture]
    public class CombineTest
    {


        [Test]
        public void EveryoneBreakEven()
        {
            DebtService service = new DebtService();
            service.PersonList.Add(new Person("Adam"));
            service.PersonList.Add(new Person("Kate"));
            service.PersonList.Add(new Person("Dru"));

            service.addDebt(100, service.PersonList.ElementAt(0), service.PersonList.ElementAt(1));
            service.addDebt(100, service.PersonList.ElementAt(1), service.PersonList.ElementAt(2));
            service.addDebt(100, service.PersonList.ElementAt(2), service.PersonList.ElementAt(0));

            service.Combine();
            ObservableCollection<DebtContainer> emptyList = new ObservableCollection<DebtContainer>();
            Console.WriteLine(emptyList.Count);
            Console.WriteLine(service.DebtList.Count);

            Assert.True(true);
            Assert.AreEqual(service.DebtList, emptyList);

        }
    }
}
