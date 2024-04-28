using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Benchmark_Assignment
{
   public class Filter :  INotifyPropertyChanged
    {
        private ObservableCollection<MyClass> _initialData;

        public event PropertyChangedEventHandler PropertyChanged;
        public Canvas mainCanvas { get; set; }
        public Grid Image1Stack { get; set; }
        public Grid Image2Stack { get; set; }
        public Grid Image3Stack { get; set; }
        public Grid Image4Stack { get; set; }
        public Grid Image5Stack { get; set; }
        public Filter(ObservableCollection<MyClass> initialData)
        {
            _initialData = initialData;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<MyClass> InitialData
        {
            get { return _initialData; }
            set
            {
                _initialData = value;
                OnPropertyChanged(nameof(InitialData));
            }
        }

        public void FilterByKeyword(string keyword, Canvas mainCanvas, Grid Image1Stack, Grid Image2Stack, Grid Image3Stack, Grid Image4Stack, Grid Image5Stack)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // If the keyword is empty, reset the filter and display all data
                InitialData = new ObservableCollection<MyClass>(_initialData);
            }
            else
            {
                // Filter the data based on the keyword
                var filteredData = _initialData.Where(item => item.Type.Contains(keyword)).ToList();

                // Store the items to be removed
                List<MyClass> itemsToRemove = _initialData.Except(filteredData).ToList();

                // Remove the UI elements for items where Type does not contain the keyword
                foreach (var item in itemsToRemove)
                {
                    RemoveImageFromCanvas(item.Name, mainCanvas, Image1Stack, Image2Stack, Image3Stack, Image4Stack, Image5Stack);
                }

                // Clear the InitialData collection
                InitialData.Clear();

                // Add the filtered items to the InitialData collection
                foreach (var item in filteredData)
                {
                    InitialData.Add(item);
                }
            }
        }

        private void RemoveImageFromCanvas(string imageName, Canvas mainCanvas, Grid Image1Stack, Grid Image2Stack, Grid Image3Stack, Grid Image4Stack, Grid Image5Stack)
        {
            if (mainCanvas == null)
                return;
            // Remove the UI element associated with the item from the canvas
            switch (imageName)
            {
                case "fighterjet":
                    mainCanvas.Children.Remove(Image1Stack);
                    break;
                case "vintageaircraft":
                    mainCanvas.Children.Remove(Image2Stack);
                    break;
                case "Hotairballoon":
                    mainCanvas.Children.Remove(Image3Stack);
                    break;
                case "lightaircraft":
                    mainCanvas.Children.Remove(Image4Stack);
                    break;
                case "RedHelicopter":
                    mainCanvas.Children.Remove(Image5Stack);
                    break;
                default:
                    // Handle unknown image name
                    break;
            }
        }

        public void SortAscending()
        {
            var sortedData = new ObservableCollection<MyClass>(_initialData.OrderBy(item => item.Type));
            UpdateInitialData(sortedData);
        }

        public void SortDescending()
        {
            var sortedData = new ObservableCollection<MyClass>(_initialData.OrderByDescending(item => item.Type));
            UpdateInitialData(sortedData);
        }

        private void UpdateInitialData(ObservableCollection<MyClass> newData)
        {
            InitialData.Clear();
            foreach (var item in newData)
            {
                InitialData.Add(item);
            }
        }
        public void SetGridInstances(Grid image1Stack, Grid image2Stack, Grid image3Stack, Grid image4Stack, Grid image5Stack)
        {
            Image1Stack = image1Stack;
            Image2Stack = image2Stack;
            Image3Stack = image3Stack;
            Image4Stack = image4Stack;
            Image5Stack = image5Stack;
        }
        public long GetGridLeft(string imageName)
        {
            long currentPositionX = 0;

            if (mainCanvas != null && mainCanvas.Children.Count > 0)
            {
                switch (imageName)
                {
                    case "fighterjet":
                        currentPositionX = Convert.ToInt64(Image1Stack.GetValue(Canvas.LeftProperty));
                        break;
                    case "vintageaircraft":
                        currentPositionX = Convert.ToInt64(Image2Stack.GetValue(Canvas.LeftProperty));
                        break;
                    case "Hotairballoon":
                        currentPositionX = Convert.ToInt64(Image3Stack.GetValue(Canvas.LeftProperty));
                        break;
                    case "lightaircraft":
                        currentPositionX = Convert.ToInt64(Image4Stack.GetValue(Canvas.LeftProperty));
                        break;
                    case "RedHelicopter":
                        currentPositionX = Convert.ToInt64(Image5Stack.GetValue(Canvas.LeftProperty));
                        break;
                }
            }
         

            return currentPositionX;
        }

        
        public long GetGridTop (string imageName)
        {
            long currentPositionY = 0;
            if (mainCanvas != null && mainCanvas.Children.Count > 0)
            {
                switch (imageName)
                {
                    case "fighterjet":
                        currentPositionY = Convert.ToInt64(Image1Stack.GetValue(Canvas.TopProperty));
                        break;
                    case "vintageaircraft":
                        currentPositionY = Convert.ToInt64(Image2Stack.GetValue(Canvas.TopProperty));
                        break;
                    case "Hotairballoon":
                        currentPositionY = Convert.ToInt64(Image3Stack.GetValue(Canvas.TopProperty));
                        break;
                    case "lightaircraft":
                        currentPositionY = Convert.ToInt64(Image4Stack.GetValue(Canvas.TopProperty));
                        break;
                    case "RedHelicopter":
                        currentPositionY = Convert.ToInt64(Image5Stack.GetValue(Canvas.TopProperty));
                        break;
                }
            }
            return currentPositionY;
        }
        
        public double GetSpeed_x(string imageName)
        {
            double currentSpeedX = 0;
            foreach (MyClass item in InitialData)
            {
                if (item.Name == imageName)
                {
                    currentSpeedX = double.Parse(item.Speed_x.TrimStart(','));
                    break;
                }
            }
            return currentSpeedX;
        }

        public double GetSpeed_y(string imageName)
        {
            double currentSpeedY = 0;

            foreach (MyClass item in InitialData)
            {
                if (item.Name == imageName)
                {
                    currentSpeedY = double.Parse(item.Speed_y.TrimStart(','));
                    break;
                }
            }
            return currentSpeedY;
        }
        
    }
}