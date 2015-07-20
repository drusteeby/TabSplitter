using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TabSplitter
{
    public partial class ViewModel : DependencyObject
    {
        public DelegateCommand AddNewPerson { get; set; }
        public DelegateCommand AddDebt { get; set; }

        public DelegateCommand Evaluate { get; set; }

        DebtService _debtService;
        public ViewModel()
        {
            _debtService = new DebtService();
            AddNewPerson = new DelegateCommand(OnAddNewPerson);
            AddDebt = new DelegateCommand(OnAddDebt);
            Evaluate = new DelegateCommand(OnEvaluate);

            peopleSharing = new List<Person>();
            personList = new ObservableCollection<Person>();

        }

        public void updateNodeText()
        {
            NodesDisplay = String.Empty;
            foreach (DebtContainer node in _debtService.DebtList)
            {
                NodesDisplay += node.OwedFrom.Name + " owes " + node.OwedTo.Name + " $" + node.Amount.ToString() + "\r\n";
            }
        }
        private void OnEvaluate()
        {
            _debtService.Combine();
            updateNodeText();
        }



        private void OnAddDebt()
        {
            foreach(Person person in peopleSharing)
                _debtService.addDebt((amount / (double)(peopleSharing.Count + 1)), personPaying, person);

            updateNodeText();

        
        }

        private void OnAddNewPerson()
        {
            if(!personList.Any(x => x.Name == newPerson) && !String.IsNullOrEmpty(newPerson))
                personList.Add(new Person(newPerson));

            newPerson = String.Empty;
        }





        public string NodesDisplay
        {
            get { return (string)GetValue(NodesDisplayProperty); }
            set { SetValue(NodesDisplayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NodesDisplay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodesDisplayProperty =
            DependencyProperty.Register("NodesDisplay", typeof(string), typeof(ViewModel), new UIPropertyMetadata(null));

        

        public Person personPaying
        {
            get { return (Person)GetValue(personPayingProperty); }
            set { SetValue(personPayingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for personPaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty personPayingProperty =
            DependencyProperty.Register("personPaying", typeof(Person), typeof(ViewModel), new UIPropertyMetadata(null));


        public List<Person> peopleSharing
        {
            get { return (List<Person>)GetValue(peopleSharingProperty); }
            set { SetValue(peopleSharingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for peopleSharing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty peopleSharingProperty =
            DependencyProperty.Register("peopleSharing", typeof(List<Person>), typeof(ViewModel), new UIPropertyMetadata(null));

        
       

        

        public string newPerson
        {
            get { return (string)GetValue(newPersonProperty); }
            set { SetValue(newPersonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for newPerson.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty newPersonProperty =
            DependencyProperty.Register("newPerson", typeof(string), typeof(ViewModel), new UIPropertyMetadata(null));



        public double amount
        {
            get { return (double)GetValue(amountProperty); }
            set { SetValue(amountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for amount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty amountProperty =
            DependencyProperty.Register("amount", typeof(double), typeof(ViewModel), new UIPropertyMetadata(null));




        public ObservableCollection<Person> personList
        {
            get { return (ObservableCollection<Person>)GetValue(personListProperty); }
            set { SetValue(personListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for personList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty personListProperty =
            DependencyProperty.Register("personList", typeof(ObservableCollection<Person>), typeof(ViewModel), new UIPropertyMetadata(null));

        

        
    }
}
