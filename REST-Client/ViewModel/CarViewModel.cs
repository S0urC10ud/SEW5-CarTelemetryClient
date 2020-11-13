using Meziantou.Framework.WPF.Collections;
using Newtonsoft.Json;
using Rechnungsverwaltung.ViewModel;
using REST_Client.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
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
        private RestClient Client = new RestClient(CARURL);

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

        public static string SerializeObject<T>(T obj, bool ignoreBase)
        {
            if (!ignoreBase)
            {
                return JsonConvert.SerializeObject(obj);
            }

            var myType = typeof(T);
            var props = myType.GetProperties().Where(p => p.DeclaringType == myType).ToList();

            var x = new ExpandoObject() as IDictionary<string, Object>;
            props.ForEach(p => x.Add(p.Name, p.GetValue(obj, null)));

            return JsonConvert.SerializeObject(x);
        }

        public CarViewModel()
        {
            RefreshCars = new RelayCommand(e => RefreshInfosWithTask(), e2 => true);
            AddCar = new RelayCommand(e =>
            {
                var request = new RestRequest("cars", Method.POST, RestSharp.DataFormat.Json);
                var cts = new CarToSend { Name = CarToAdd.Name};
                cts.setType(CarToAdd.Typ);
                request.AddJsonBody(cts);
                Client.Execute(request);
                RefreshInfosWithTask();
            });
            RefreshInfosWithTask();
        }

        private async Task GetCarsAsync()
        {
            LoadingVisibility = Visibility.Visible;
            Cars.Clear();

            RestRequest request = new RestRequest("cars", RestSharp.DataFormat.Json);
            var response = await Client.ExecuteAsync<List<Car>>(request);

            
            //await Task.Delay(2500);

            LoadingVisibility = Visibility.Hidden;

            response.Data?.ForEach(car => {
                    car.DeleteCommand = new RelayCommand(e=> {
                        var req = new RestRequest("cars/{id}", Method.DELETE);
                        req.AddParameter("id", car.CarId, ParameterType.UrlSegment);
                        Client.Execute(req);
                        RefreshInfosWithTask(); 
                    },e2=>true);
                    car.UpdateCommand = new RelayCommand(e => {
                        var putRequest = new RestRequest("cars/" + car.CarId, Method.PUT);
                        CarToSend carToSend = new CarToSend { Name = car.Name };
                        carToSend.setType(car.Typ);
                        putRequest.AddJsonBody(carToSend);
                        Client.Execute(putRequest);
                        RefreshInfosWithTask();
                    }, e2 => true);
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
