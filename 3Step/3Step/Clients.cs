using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;

namespace _3Step
{
    
    class Clients
    {
        int _countClient;      //поле количества клиентов
        Client[] _arrayClients;//поле массив клиентов
        int _sizeArray;        //поле размер массива
        int _lastClient;       //поле индекс последнего элемента(индекс первого элемента всегда равен 0)
        public ObservableCollection<Client> observClients = new ObservableCollection<Client>();

        public Clients(int sizeArray)//конструктор
        {
            SizeArray = sizeArray;
            CountClient = 0;
            ArrayClients = new Client[sizeArray];
            LastClient = -1;
        }

        public int CountClient { get => _countClient;
            set
            {
                _countClient = value;
            } 
        }
        public int SizeArray { get => _sizeArray; set => _sizeArray = value; }
        public Client[] ArrayClients { get => _arrayClients; set => _arrayClients = value; }
        public int LastClient { get => _lastClient; set => _lastClient = value; }

        public bool AddClient(int clientId)//метод добавления клиента
        {
            Client client = new Client(clientId);
            ArrayClients[CountClient] = client;
            CountClient++;
            LastClient++;
            observClients.Add(client);
            if (CountClient == SizeArray)
            {
                SizeArray *= 2;
                Array.Resize(ref _arrayClients, SizeArray);
                return true;
            }
            return false;
        }
        public bool AddClient(Client client)//метод добавления клиента
        {
            ArrayClients[CountClient] = client;
            CountClient++;
            LastClient++;
            observClients.Add(client);
            if (CountClient == SizeArray)
            {
                SizeArray *= 2;
                Array.Resize(ref _arrayClients, SizeArray);
                return true;
            }
            return false;
        }
        public bool DeleteClient()//метод удаления клиента
        {
            if (CountClient != 0)
            {
                ArrayClients[0] = null;
                for (int i = 1; i < CountClient; i++)
                    ArrayClients[i - 1] = ArrayClients[i];
                CountClient--;
                LastClient--;
                observClients.Remove(observClients.FirstOrDefault());
                return true;
            }
            return false;
        }
        public Client FindClient(int clientId)//метод поиска клиента
        {
            for (int i = 0; i < CountClient; i++)
            {
                if (ArrayClients[i].ClientId == clientId)
                {
                    return ArrayClients[i];
                }
            }
            return null;
        }
        public int SumClients()//метод подсчета суммы всех заказов
        {
            int SumClients = 0;
            for (int i = 0; i < CountClient; i++)
            {
                SumClients += ArrayClients[i].SumRide();
            }
            return SumClients;
        }
        public string AllInfo()//метод всей информации
        {
            string AllData = "Клиенты:\n";
            for (int i = 0; i < CountClient; i++)
            {
                if (ArrayClients[i] != null)
                {
                    AllData += "ClientId : " + ArrayClients[i].ClientId + "\n" + ArrayClients[i].InfoClient() +
                    "\n=======================================================\n";
                }
            }
            AllData += $"Сумма всех заказов{SumClients()}";
            return AllData;
        }
        public bool Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if ((bool)saveFileDialog.ShowDialog())
                using (FileStream fs = (FileStream)saveFileDialog.OpenFile())
                {
                    JsonSerializer.Serialize<Clients>(new Utf8JsonWriter(fs), this);
                    
                }
            return true;
        }
        public async void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
                using (FileStream fs = (FileStream)openFileDialog.OpenFile())
                {
                    //_Enterprises
                    Clients clients = await JsonSerializer.DeserializeAsync<Clients>(fs);
                    //_clients = new Clients(10);
                    //observClients = new ObservableCollection<Client>();
                    //DataClient.ItemsSource = observClients;
                    foreach (var i in clients.ArrayClients)
                    {
                        if (i != null)
                        {
                            Client client = new Client(i.ClientId);
                            foreach (Ride j in i.ObservableCollectionRide)
                            {
                                client.AddRide(j.DateTime, j.Price, j.Time);
                            }
                            AddClient(client);
                            //observClients.Add(client);
                        }
                    }
                }
        }
    }
}
