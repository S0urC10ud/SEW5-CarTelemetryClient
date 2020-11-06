using Rechnungsverwaltung.ViewModel;
using REST_Client.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace REST_Client.ViewModel
{
    
    public class CarViewModel: INotifyPropertyChanged
    {
        private const string CARURL = "http://192.168.1.205:45458/api";
        private RestClient client = new RestClient(CARURL);
        private ObservableCollection<Car> Cars { get; set; } = new ObservableCollection<Car>();

        public ICommand RefreshCars { get; set; }

        private Visibility _loadingVisibility = Visibility.Visible;
        public Visibility LoadingGifVisibility { get; set; }



        private void RefreshInfosWithTask()
        {
            Task.Run(() => this.GetCarsAsync());
        }

        public CarViewModel()
        {
            RefreshCars = new RelayCommand(e => RefreshInfosWithTask(), e2 => true);
            RefreshInfosWithTask();

        }

        private async Task GetCarsAsync()
        {
            LoadingVisibility = Visibility.Visible;

            Cars.Clear();

            RestRequest request = new RestRequest("cars", RestSharp.DataFormat.Json);
            var response = await client.ExecuteAsync<List<Car>>(request);

            LoadingVisibility = Visibility.Hidden;

            response.Data?.ForEach(car => Cars.Add(car));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public Visibility LoadingVisibility
        {
            get
            {
                return _loadingVisibility;
            }
            set
            {
                if (_loadingVisibility.Equals(value))
                    return;

                _loadingVisibility = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
