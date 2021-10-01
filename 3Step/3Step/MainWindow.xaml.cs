using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        Clients clients = new Clients(10);
        ObservableCollection<Client> observClients = new ObservableCollection<Client>();
        private void Add_Client_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client(Convert.ToInt32(ClientId_TextBox.Text));
            clients.AddClient(client);
            observClients.Add(client);
        }

        private void Delete_Client_Click(object sender, RoutedEventArgs e)
        {
            clients.DeleteClient();
            observClients.Remove(observClients.LastOrDefault());
        }

        private void Add_Ride_Click(object sender, RoutedEventArgs e)
        {
            Client client = DataClient.SelectedItem as Client;
            client = clients.FindClient(client.ClientId);
            client.AddRide((DateTime)DatePick.SelectedDate, Convert.ToInt32(Price_TextBox.Text));
            DataRide.ItemsSource = client.GetRides();
        }

        private void DataClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client client = DataClient.SelectedItem as Client;
            client = clients.FindClient(client.ClientId);
            //DataRide.ItemsSource = client.observableCollectionRide;
            DataRide.ItemsSource = client.GetRides();
        }

        private void Delete_Ride_Click(object sender, RoutedEventArgs e)
        {
            Client client = DataClient.SelectedItem as Client;
            client = clients.FindClient(client.ClientId);
            Ride ride = DataRide.SelectedItem as Ride;
            client.DeleteRide(ride.DateTime);
            DataRide.ItemsSource = client.GetRides();
        }
    }
}
