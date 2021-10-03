using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Step
{
    /// <summary>
    /// Класс клиент
    /// </summary>
    class Client: INotifyPropertyChanged
    {
        int id;         //поле номера клиента
        int CountOrder; //поле заказов
        public Ride Head;
        public int ClientId { get => id; set => id = value; }

        public ObservableCollection<Ride> ObservableCollectionRide { get => observableCollectionRide; set => observableCollectionRide = value; }

        private ObservableCollection<Ride> observableCollectionRide = new ObservableCollection<Ride>();

        public event PropertyChangedEventHandler PropertyChanged;

        public Client(int clientId)//конструктор
        {
            ClientId = clientId;
            CountOrder = 0;
            Head = new Ride(new DateTime(0001, 1, 1), 0, 0);
        }

        public bool AddRide(DateTime dateTime, int price, int time)//метод добавления поездки
        {
            bool check = true;
            Ride prev = Head;
            Ride current = Head.Next;
            Ride newRide = new Ride(dateTime, price, time);
            if (CountOrder == 0)
            {
                Head.Next = newRide;
                ObservableCollectionRide.Add(newRide);
            }
            else
            {
                while (current != null)
                {
                    if (DateTime.Compare(newRide.DateTime, current.DateTime) < 0)
                    {
                        newRide.Next = current;
                        prev.Next = newRide;
                        check = false;
                        break;
                    }
                    else
                    {
                        prev = current;
                        current = current.Next;
                    }
                }
                if (check)
                {
                    prev.Next = newRide;
                    ObservableCollectionRide.Add(newRide);
                    ObservableCollectionRide = new ObservableCollection<Ride>(ObservableCollectionRide.OrderBy(i => i.DateTime));
                }
            }
            CountOrder++;
            OnPropertyChanged("Add");
            return true;
        }
        public bool DeleteRide(DateTime dateTime)//метод удаления поездки
        {
            if (CountOrder != 0)
            {
                Ride current = FindPrevRide(dateTime);
                if (current == null)
                {
                    return false;
                }
                else
                {
                    current.Next = current.Next.Next;
                    ObservableCollectionRide.Remove(FindRide(dateTime));
                    OnPropertyChanged("Delete");
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public Ride FindRide(DateTime dateTime)//метод поиска поездки
        {
            Ride prev = Head;
            Ride current = Head.Next;
            while (current != null)
            {
                if (current.DateTime == dateTime)
                {
                    return current;
                }
                else
                {
                    prev = current;
                    current = current.Next;
                }
            }
            return null;
        }
        public Ride FindPrevRide(DateTime dateTime)//метод поиска предыдущей искомой поездки
        {
            Ride prev = Head;
            Ride current = Head.Next;
            while (current != null)
            {
                if (current.DateTime == dateTime)
                {
                    return prev;
                }
                else
                {
                    prev = current;
                    current = current.Next;
                }
            }
            return null;
        }
        public int SumRide()//метод подсчета суммы всех заказов
        {
            Ride current = Head.Next;
            int SumRide = 0;
            while (current != null)
            {
                SumRide += current.Price;
                current = current.Next;
            }
            return SumRide;
        }
        public string InfoClient()//метод получения ифнормации
        {
            Ride current = Head.Next;
            string dataClient = "";
            if (current != null)
            {
                dataClient = $"Кол-во заказов: {CountOrder}\n " +
                "поездки:\n";
                while (current != null)
                {
                    dataClient += $"Цена : {current.Price} Время : {current.DateTime}\n";
                    current = current.Next;
                }
            }
            else
            {
                dataClient = $"Кол-во заказов: {CountOrder}\n ";
            }
            return dataClient;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public IEnumerable<Ride> GetRides()
        {
            Ride current = Head.Next;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }
        public List<Ride> ToList()
        {
            List<Ride> rides = new List<Ride>(GetRides());
            return rides;
        }
    }
}
