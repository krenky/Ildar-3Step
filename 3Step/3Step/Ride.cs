using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Step
{
    /// <summary>
    /// Класс поездки
    /// </summary>
    class Ride: INotifyPropertyChanged
    {
        DateTime _dateTime;//поле даты и времени
        int _price;        //поле цены поездки
        Ride _next;        //поле ссылка на следующую поездку

        public Ride(DateTime dateTime, int price)//конструктор
        {
            DateTime = dateTime;
            Price = price;
        }

        public DateTime DateTime { get => _dateTime; set => _dateTime = value; }
        public int Price { get => _price; 
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }
        internal Ride Next { get => _next; set
            {
                _next = value;
                OnPropertyChanged("Next");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
