using System;
using System.Collections.Generic;
using System.Text;

namespace CleverShoppingList.Models
{
    class SpendingMonth : ViewModelBase
    {
        DateTime time;
        decimal spent;
        decimal comparedSpent = 0m;
        string name;

        public DateTime Time { get => time; set { SetProperty(ref time, value); } }
        public decimal Spent { get => spent; set { SetProperty(ref spent, value); } }
        public decimal ComparedSpent { get => comparedSpent; set { SetProperty(ref comparedSpent, value); } }
        public string Name { get => name; set { SetProperty(ref name, value); } }

        public SpendingMonth(DateTime time, decimal spent, decimal previousSpent = 0m)
        {
            this.Time = time;
            this.Spent = spent;
            this.ComparedSpent = Spent - previousSpent;
            this.Name = Time.Year.ToString() + " " + Time.Month.ToString();
        }

    }
}
