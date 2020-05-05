using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            var data = JsonConvert.DeserializeObject<Data>(File.ReadAllText("test.json"));
            data.TemperatureType = TemperatureType.Celcjusz;
            DataContext = data;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var data = DataContext as Data;
            data.Temp.Add(new Temperature { Time = DateTime.Now, Value = rnd.Next(20, 40) });
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            var data = DataContext as Data;
            data.Temp.Clear();
        }

        private void btnUnit_Click(object sender, RoutedEventArgs e)
        {
            var data = DataContext as Data;

            if(data.TemperatureType == TemperatureType.Celcjusz)
            {
                foreach (var temperature in data.Temp)
                {
                    temperature.Value = (temperature.Value * 1.8) + 32;
                }
                data.TemperatureType = TemperatureType.Farenheit;
            }
            else
            {
                foreach (var temperature in data.Temp)
                {
                    temperature.Value = (temperature.Value - 32)/1.8;
                }
                data.TemperatureType = TemperatureType.Celcjusz;
            }
           
        }
    }
    public class Data
    {
        public TemperatureType TemperatureType { get; set; }
        public ObservableCollection<Temperature> Temp { get; set; }
    }
    public enum TemperatureType
    {
        Celcjusz,
        Farenheit
    }
    public class Temperature : INotifyPropertyChanged
    {

        private DateTime _time;
        public DateTime Time 
        {
            get => _time; 
            set 
            { 
                if(value == _time)
                {
                    return;
                }

                _time = value;
                OnPropertyChanged();
            } 
        }
        private double _value;
        public double Value 
        { 
            get => _value; 
            set 
            {
                if (value == _value)
                {
                    return;
                }
                _value = value;
                OnPropertyChanged();
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}