using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabSplitter
{

    public class DebtService
    {
        public ObservableCollection<DebtContainer> DebtList {

            get
            {
                if (_debtList == null)
                    _debtList = new ObservableCollection<DebtContainer>();

                return _debtList;
            }
            set
            {
                _debtList = value;
            }
        } private ObservableCollection<DebtContainer> _debtList;


        public ObservableCollection<Person> PersonList
        {

            get
            {
                if (_personList == null)
                    _personList = new ObservableCollection<Person>();

                return _personList;
            }
            set
            {
                _personList = value;
            }
        } private ObservableCollection<Person> _personList;

        public DebtService()
        {
            
        }

        public void addDebt(DebtContainer node)
        {
            addDebt(node.Amount, node.OwedTo, node.OwedFrom);
        }
        //Person B owes Person A amount
        public void addDebt(double amount, Person OwedTo, params Person[] OwedFromList)
        {
            amount = amount / ((double)OwedFromList.Count() + 1);

            foreach(Person OwedFrom in OwedFromList)
                DebtList.Add(new DebtContainer(OwedTo, OwedFrom, amount));           

        }

        void cleanUp(DebtContainer existingDebt)
        {
            if (existingDebt.Amount == 0) DebtList.Remove(existingDebt);
            else if (existingDebt.Amount < 0) swap(existingDebt);
        }

        public void Combine()
        {
            for(int i = 0; i< DebtList.Count; i++)
            {
                for(int j = 0; j<DebtList.Count; j++ )
                {
                    if (i == j) continue;
                    var nodeOne = DebtList.ElementAt(i);
                    var nodeTwo = DebtList.ElementAt(j);
                    var newNodeOne = new DebtContainer();
                    var newNodeTwo = new DebtContainer();


                    //check to see if they're the same owedTo and owedFrom
                    if (nodeOne.OwedTo == nodeTwo.OwedTo && nodeOne.OwedFrom == nodeTwo.OwedFrom)
                    {
                        nodeOne.Amount += nodeTwo.Amount;
                        DebtList.Remove(nodeTwo);
                        continue;
                    }
                    else if (nodeOne.OwedTo == nodeTwo.OwedFrom && nodeOne.OwedFrom == nodeTwo.OwedTo)
                    {
                        if (nodeOne.Amount - nodeTwo.Amount > 0)
                        {
                            nodeOne.Amount -= nodeTwo.Amount;
                            DebtList.Remove(nodeTwo);
                        }
                        else if (nodeOne.Amount - nodeTwo.Amount < 0)
                        {
                            nodeTwo.Amount -= nodeOne.Amount;
                            DebtList.Remove(nodeOne);
                        }
                        else
                        {
                            DebtList.Remove(nodeOne);
                            DebtList.Remove(nodeTwo);
                        
                        }

                        continue;
                    }                  
                                       

                    //check to see if they can be transitivly combined.
                    // (A -> B), (B -> C) ->> (A -> C)
                    if(nodeOne.OwedFrom == nodeTwo.OwedTo)
                    {
                        nodeOne = DebtList.ElementAt(j);
                        nodeTwo = DebtList.ElementAt(i);
                    }
                    if(nodeOne.OwedTo == nodeTwo.OwedFrom)
                    {
                        if(nodeOne.Amount > nodeTwo.Amount)
                        {
                            newNodeOne.OwedTo = nodeTwo.OwedTo;
                            newNodeOne.OwedFrom = nodeOne.OwedFrom;
                            newNodeOne.Amount = nodeTwo.Amount;

                            newNodeTwo.OwedFrom = nodeOne.OwedFrom;
                            newNodeTwo.OwedTo = nodeOne.OwedTo;
                            newNodeTwo.Amount = nodeOne.Amount - nodeTwo.Amount;

                            //if adding them will result in a combination, add them then call the recursive function
                            if(DebtList.Any(x => (x.OwedFrom == newNodeOne.OwedFrom && x.OwedTo == newNodeOne.OwedTo) 
                                                    || (x.OwedFrom == newNodeOne.OwedTo && x.OwedTo == newNodeOne.OwedFrom)))
                            {
                                AddToDebtList(nodeOne, nodeTwo, newNodeOne, newNodeTwo);
                                Combine();
                            }
                        }
                        else if (nodeOne.Amount < nodeTwo.Amount)
                        {
                            newNodeOne.OwedFrom = nodeOne.OwedFrom;
                            newNodeOne.OwedTo = nodeTwo.OwedTo;
                            newNodeOne.Amount = nodeOne.Amount;


                            newNodeTwo.OwedFrom = nodeTwo.OwedFrom;
                            newNodeTwo.OwedTo = nodeTwo.OwedTo;
                            newNodeTwo.Amount = nodeTwo.Amount - nodeOne.Amount;

                            //if adding them will result in a combination, add them then call the recursive function
                            if (DebtList.Any(x => (x.OwedFrom == newNodeOne.OwedFrom && x.OwedTo == newNodeOne.OwedTo)
                                                    || (x.OwedFrom == newNodeOne.OwedTo && x.OwedTo == newNodeOne.OwedFrom)))
                            {
                                AddToDebtList(nodeOne, nodeTwo, newNodeOne, newNodeTwo);
                                Combine();
                            }
                        }

                        else
                        {

                            newNodeOne.OwedFrom = nodeOne.OwedFrom;
                            newNodeOne.OwedTo = nodeTwo.OwedTo;
                            newNodeOne.Amount = nodeOne.Amount;


                            DebtList.Remove(nodeOne);
                            DebtList.Remove(nodeTwo);
                            DebtList.Add(new DebtContainer(newNodeOne.OwedTo, newNodeOne.OwedFrom, newNodeOne.Amount));
                            Combine();
                        }
                    }


                }
            }
        }

        private void AddToDebtList(DebtContainer nodeOne, DebtContainer nodeTwo, DebtContainer newNodeOne, DebtContainer newNodeTwo)
        {
            DebtList.Remove(nodeOne);
            DebtList.Remove(nodeTwo);
            DebtList.Add(new DebtContainer(newNodeOne.OwedTo, newNodeOne.OwedFrom, newNodeOne.Amount));
            DebtList.Add(new DebtContainer(newNodeTwo.OwedTo, newNodeTwo.OwedFrom, newNodeTwo.Amount));
        }      

        private void swap(DebtContainer toSwap)
        {
            toSwap.Amount = Math.Abs(toSwap.Amount);
            Person temp = toSwap.OwedTo;
            toSwap.OwedTo = toSwap.OwedFrom;
            toSwap.OwedFrom = temp;
        }
    }
}
