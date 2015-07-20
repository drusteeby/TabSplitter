using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabSplitter
{
    public class DebtContainer
    {
        public double Amount { get; set; }
        public Person OwedTo { get; set; }
        public Person OwedFrom { get; set; }
        public DebtContainer(){}
        public DebtContainer(Person payee,Person ower,double amount) 
                               {OwedTo = payee; OwedFrom = ower; Amount = amount;}

    }
}
