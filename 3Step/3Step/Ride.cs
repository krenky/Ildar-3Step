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
        int _time;
        Ride _next;        //поле ссылка на следующую поездку

        public Ride(DateTime dateTime, int price, int time)//конструктор
        {
            DateTime = dateTime;
            Price = price;
            Time = time;
        }

        public DateTime DateTime { get => _dateTime; set => _dateTime = value; }
        public int Price { get => _price; 
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        public int Time { get => _time; set => _time = value; }

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
