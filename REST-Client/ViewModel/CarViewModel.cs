using Meziantou.Framework.WPF.Collections;
using Newtonsoft.Json;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace REST_Client.ViewModel
{
    
    public class CarViewModel: INotifyPropertyChanged
    {
        private const string CARURL = "http://192.168.1.205:45455/api";
        private RestClient client = new RestClient(CARURL);

        public Car CarToAdd { get; set; } = new Car { Name = "CarName", Typ = "SUV"};
        public ConcurrentObservableCollection<Car> Cars { get; set; } = new ConcurrentObservableCollection<Car>();

        public ICommand RefreshCars { get; set; }

        private Visibility _loadingVisibility = Visibility.Visible;
        public Visibility LoadingGifVisibility { get; set; }

        public ICommand AddCar { get; set; }

        private void RefreshInfosWithTask()
        {
            Task.Run(() => this.GetCarsAsync());
        }

        public CarViewModel()
        {
            RefreshCars = new RelayCommand(e => RefreshInfosWithTask(), e2 => true);
            AddCar = new RelayCommand(e =>
            {
                var request = new RestRequest("cars", Method.POST);
                var cts = new CarToSend { Name = CarToAdd.Name};
                cts.setType(CarToAdd.Typ);
                request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(cts), ParameterType.RequestBody);
                client.Execute(request);
            });
            RefreshInfosWithTask();

        }

        private async Task GetCarsAsync()
        {
            LoadingVisibility = Visibility.Visible;
            Cars.Clear();

            RestRequest request = new RestRequest("cars", RestSharp.DataFormat.Json);
            var response = await client.ExecuteAsync<List<Car>>(request);

            await Task.Delay(2500);
            LoadingVisibility = Visibility.Hidden;

            response.Data?.ForEach(car => {
                    car.DeleteCommand = new RelayCommand(e=> { client.Delete(new RestRequest("cars/"+car.CarId)); },e2=>true);
                    car.updateType();
                    Cars.Add(car);
                });
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
