﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3Step
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataClient.ItemsSource = observClients;
        }
        Clients _clients = new Clients(10);
        /// <summary>
        /// обЪявление коллекции(ObservableCollection) для привязки данных к таблице
        /// </summary>
        ObservableCollection<Client> observClients = new ObservableCollection<Client>();
        /// <summary>
        /// добавление клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Client_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client(Convert.ToInt32(ClientId_TextBox.Text));
            _clients.AddClient(client);
            observClients.Add(client);
        }
        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Client_Click(object sender, RoutedEventArgs e)
        {
            _clients.DeleteClient();
            observClients.Remove(observClients.LastOrDefault());
        }
        /// <summary>
        /// добавления поездки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Ride_Click(object sender, RoutedEventArgs e)
        {
            Client client = DataClient.SelectedItem as Client;
            client = _clients.FindClient(client.ClientId);
            client.AddRide((DateTime)DatePick.SelectedDate, Convert.ToInt32(Price_TextBox.Text));
            DataRide.ItemsSource = client.GetRides();
        }
        /// <summary>
        /// Обработчик выбора нового элемента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client client = DataClient.SelectedItem as Client;
            client = _clients.FindClient(client.ClientId);
            //DataRide.ItemsSource = client.observableCollectionRide;
            DataRide.ItemsSource = client.GetRides();
        }
        /// <summary>
        /// Удаление поездки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Ride_Click(object sender, RoutedEventArgs e)
        {
            Client client = DataClient.SelectedItem as Client;
            client = _clients.FindClient(client.ClientId);
            Ride ride = DataRide.SelectedItem as Ride;
            client.DeleteRide(ride.DateTime);
            DataRide.ItemsSource = client.GetRides();
        }
        /// <summary>
        /// Обработчик сохранения файла сохраниения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_file_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if ((bool)saveFileDialog.ShowDialog())
                using (FileStream fs = (FileStream)saveFileDialog.OpenFile())
                {
                    JsonSerializer.Serialize<Clients>(new Utf8JsonWriter(fs), _clients);
                }
        }
        /// <summary>
        /// Обработчик загрузки файла сохраниения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Load_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
                using (FileStream fs = (FileStream)openFileDialog.OpenFile())
                {
                    //_Enterprises
                    Clients clients = await JsonSerializer.DeserializeAsync<Clients>(fs);
                    _clients = new Clients(10);
                    foreach (var i in clients.ArrayClients)
                    {
                        if (i != null)
                        {
                            Client client = new Client(i.ClientId);
                            foreach (Ride j in i.ObservableCollectionRide)
                            {
                                client.AddRide(j.DateTime, j.Price);
                            }
                            _clients.AddClient(client);
                            observClients.Add(client);
                        }
                    }
                }
        }

        private void ClientId_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void ClientId_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void Price_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void Price_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
