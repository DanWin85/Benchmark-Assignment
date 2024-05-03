using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Benchmark_Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    // This is the main window class of the application
    public partial class MainWindow : Window
    {
        private FileManager? fileManager;
        private Filter Filter { get; set; }
        ObservableCollection<MyClass> Initial_data = new ObservableCollection<MyClass>();
        private List<string> Speed_x = new List<string>();
        private List<string> Speed_y = new List<string>();
        DispatcherTimer timer = new DispatcherTimer();

        // Strings to track the direction of each image
        private string Image1directionx = "right";
        private string Image1directionY = "down";

        private string Image2directionx = "right";
        private string Image2directionY = "down";

        private string Image3directionx = "right";
        private string Image3directionY = "down";

        private string Image4directionx = "right";
        private string Image4directionY = "down";

        private string Image5directionx = "right";
        private string Image5directionY = "down";

        // Constructor to initialize the main window
        public MainWindow()
        {
            InitializeComponent();

            fileManager = new FileManager();

            Filter = new Filter(Initial_data);

            Filter.PropertyChanged += Filter_PropertyChanged;

            CollectionViewSource viewSource = new CollectionViewSource();
            viewSource.Source = Filter.InitialData;
            ListBox.ItemsSource = viewSource.View;

        }

        // Event handler for the "Load Initial Data" button click
        private void LoadInitialDataButton_Click(object sender, RoutedEventArgs e)
        {
           
            string loadedData = fileManager.LoadInitialData();

            // Check if the loaded data is not null
            if (!string.IsNullOrEmpty(loadedData))
            {
                string[] lines = loadedData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                // Clear the existing data in Initial_data list
                Initial_data.Clear();

                // Iterate through each line
                foreach (string line in lines)
                {
                    // Split the line into an array using commas
                    string[] values = line.Split(',');

                    // Check if the array has the expected number of elements
                    if (values.Length == 6)
                    {
                        // Trim the whitespace from the values
                        string[] trimmedValues = values.Select(v => v.Trim()).ToArray();

                        // Create a new instance of MyClass using the trimmed values
                        MyClass imageInfo = new MyClass(trimmedValues[0], trimmedValues[1], trimmedValues[2], trimmedValues[3], trimmedValues[4], trimmedValues[5]);

                        // Add the image information to the Initial_data list
                        Initial_data.Add(imageInfo);
                        PopulateSpeedXList(Initial_data, Speed_x);
                        PopulateSpeedYList(Initial_data, Speed_y);
                    }
                    else
                    {
                        // Handle the case where a line does not have the expected number of elements
                        MessageBox.Show($"Error parsing line: {line}. The line does not have the expected number of elements.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Update the ItemsSource of ListBox to reflect changes in Initial_data
                ListBox.ItemsSource = Initial_data;

                // Call the LoadImage method
                LoadImage();

                Filter.InitialData = Initial_data;
                Filter.mainCanvas = mainCanvas;
                Filter.SetGridInstances(Image1Stack, Image2Stack, Image3Stack, Image4Stack, Image5Stack);

                // Set visibility of image elements to Visible
                Image1Stack.Visibility = Visibility.Visible;
                Image2Stack.Visibility = Visibility.Visible;
                Image3Stack.Visibility = Visibility.Visible;
                Image4Stack.Visibility = Visibility.Visible;
                Image5Stack.Visibility = Visibility.Visible;

                if (!mainCanvas.Children.Contains(Image1Stack))
                    mainCanvas.Children.Add(Image1Stack);
                if (!mainCanvas.Children.Contains(Image2Stack))
                    mainCanvas.Children.Add(Image2Stack);
                if (!mainCanvas.Children.Contains(Image3Stack))
                    mainCanvas.Children.Add(Image3Stack);
                if (!mainCanvas.Children.Contains(Image4Stack))
                    mainCanvas.Children.Add(Image4Stack);
                if (!mainCanvas.Children.Contains(Image5Stack))
                    mainCanvas.Children.Add(Image5Stack);
            }
            else
            {
                // Handle the case where the loaded data is null or empty
                MessageBox.Show("Error loading data. The file may be empty or not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to load images and set up animation
        public void LoadImage()
        {
            // Stop the timer if it's already running
            if (timer.IsEnabled)
            {
                StopTimer();
            } 

            // Clear the mainCanvas if already populated
            mainCanvas.Children.Clear();

            int index = 0;
            foreach (MyClass item in Filter.InitialData)
            {
                string imageName = item.Name.ToLower();
                Uri imageUri = new Uri($"pack://application:,,,/images/{imageName}.jpg");

                if (index == 0)
                {
                    Image1.Source = new BitmapImage(imageUri);
                    mainCanvas.Children.Add(Image1Stack);
                }
                else if (index == 1)
                {
                    Image2.Source = new BitmapImage(imageUri);
                    mainCanvas.Children.Add(Image2Stack);
                }
                else if (index == 2)
                {
                    Image3.Source = new BitmapImage(imageUri);
                    mainCanvas.Children.Add(Image3Stack);
                }
                else if (index == 3)
                {
                    Image4.Source = new BitmapImage(imageUri);
                    mainCanvas.Children.Add(Image4Stack);
                }
                else if (index == 4)
                {
                    Image5.Source = new BitmapImage(imageUri);
                    mainCanvas.Children.Add(Image5Stack);
                }

                index++;
            }

            ImageAnimation(Filter.InitialData);
        }

        // Method to start the animation timer and event handlers
        public void ImageAnimation(ObservableCollection<MyClass> loadedData)
        {
            // Stop the timer if it's already running
            if (timer.IsEnabled)
            {
                StopTimer();
            }

            // Populate the Speed_x and Speed_y lists from the loaded data
            PopulateSpeedXList(loadedData, Speed_x);
            PopulateSpeedYList(loadedData, Speed_y);

            // Calling the event handlers to move the images
            timer.Tick += new EventHandler(Timer_xdirection);
            timer.Tick += new EventHandler(timer_TickTop);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Start();
        }

        // Method to stop the animation timer
        private void StopTimer()
        {
            if (timer.IsEnabled)
            {
                timer.Tick -= new EventHandler(Timer_xdirection);
                timer.Tick -= new EventHandler(timer_TickTop);
                timer.Stop();
            }
        }

        // Method to populate the Speed_x list with values from the loaded data
        private void PopulateSpeedXList(ObservableCollection<MyClass> data, List<string> speedXList)
        {
            speedXList.Clear();
            foreach (MyClass c in data)
            {
                speedXList.Add(c.Speed_x.TrimStart('-'));
            }
        }

        // Method to populate the Speed_y list with values from the loaded data
        private void PopulateSpeedYList(ObservableCollection<MyClass> data, List<string> speedYList)
        {
            speedYList.Clear();
            foreach (MyClass c in data)
            {
                speedYList.Add(c.Speed_y.TrimStart('-'));
            }
        }

        // Event handler for the animation timer (X-axis movement)
        void Timer_xdirection(object sender, EventArgs e)
        {
            //List containing Speed_x list
            List<string> Speed_x = new List<string>();
            PopulateSpeedXList(Initial_data, Speed_x);

            double image1speedX = 0;
            if (Speed_x.Count > 0)
            {
                //converting speed x cordinate to double varible
                 image1speedX = Convert.ToDouble(Speed_x[0]);
            }
            
            //Getting left property of image 1 grid stack with respect to canvas
            long image1Positionx = Convert.ToInt64(Image1Stack.GetValue(Canvas.LeftProperty));

            //if condition checking if Image position x is greater or equal to 390
            if (image1Positionx >= 485)
            {
                //setting image one direction to left
                Image1directionx = "left";
            }
            //if condition checking if Image one direction is left
            if (Image1directionx.Contains("left"))
            {
                //Maving Image one on x axis to the left by subtracting image1speedX
                Canvas.SetLeft(Image1Stack, image1Positionx - image1speedX);
            }

            //if condition checking if Image one position x is less then or equal to 5
            if (image1Positionx <= 5)
            {
                //setting image one direction X to right
                Image1directionx = "right";
            }
            //if condition checking if Image one direction is right
            if (Image1directionx.Contains("right"))
            {
                //Moving Image one on x axis to the right by adding image1speedX
                Canvas.SetLeft(Image1Stack, image1Positionx + image1speedX);
            }

            double image2speedX = 0;
            if (Speed_x.Count > 0)
            {
                //converting speed x cordinate to double varible
                image2speedX = Convert.ToDouble(Speed_x[0]);
            }
            //Getting left property of image 2 grid stack with respect to canvas
            long image2Positionx = Convert.ToInt64(Image2Stack.GetValue(Canvas.LeftProperty));
            if (image2Positionx >= 485)
            {
                //setting image two direction to right
                Image2directionx = "right";
            }
            //if condition checking if Image two direction is right
            if (Image2directionx.Contains("right"))
            {
                //Moving Image one on x axis to the left by subtracting image2speedX
                Canvas.SetLeft(Image2Stack, image2Positionx - image2speedX);
            }

            //if condition checking if Image two position x is less then or equal to 5
            if (image2Positionx <= 5)
            {
                //setting image two direction X to left
                Image2directionx = "left";
            }
            //if condition checking if Image two direction is left
            if (Image2directionx.Contains("left"))
            {
                //Moving Image one on x axis to the right by adding image2speedX
                Canvas.SetLeft(Image2Stack, image2Positionx + image2speedX);
            }
            
            double image3speedX = 0;
            if (Speed_x.Count > 0)
            {
                //converting speed x cordinate to double varible
                image3speedX = Convert.ToDouble(Speed_x[0]);
            }
            //Getting left property of image 3 grid stack with respect to canvas
            long image3Positionx = Convert.ToInt64(Image3Stack.GetValue(Canvas.LeftProperty));
            if (image3Positionx >= 485)
            {
                //setting image three direction to right
                Image3directionx = "right";
            }
            //if condition checking if Image three direction is right
            if (Image3directionx.Contains("right"))
            {
                //Moving Image three on x axis to the left by subtracting image3speedX
                Canvas.SetLeft(Image3Stack, image3Positionx - image3speedX);
            }

            //if condition checking if Image three position x is less then or equal to 5
            if (image3Positionx <= 5)
            {
                //setting image three direction X to left
                Image3directionx = "left";
            }
            //if condition checking if Image three direction is left
            if (Image3directionx.Contains("left"))
            {
                //Moving Image three on x axis to the right by adding image3speedX
                Canvas.SetLeft(Image3Stack, image3Positionx + image3speedX);
            }

            double image4speedX = 0;
            if (Speed_x.Count > 0)
            {
                //converting speed x cordinate to double varible
                image4speedX = Convert.ToDouble(Speed_x[0]);
            }
            //Getting left property of image 4 grid stack with respect to canvas
            long image4Positionx = Convert.ToInt64(Image4Stack.GetValue(Canvas.LeftProperty));
            if (image4Positionx >= 485)
            {
                //setting image four direction to left
                Image4directionx = "left";
            }
            //if condition checking if Image four direction is left
            if (Image4directionx.Contains("left"))
            {
                //Moving Image four on x axis to the left by subtracting image4speedX
                Canvas.SetLeft(Image4Stack, image4Positionx - image4speedX);
            }

            //if condition checking if Image four position x is less then or equal to 5
            if (image4Positionx <= 5)
            {
                //setting image four direction X to right
                Image4directionx = "right";
            }
            //if condition checking if Image four direction is right
            if (Image4directionx.Contains("right"))
            {
                //Moving Image four on x axis to the right by adding image4speedX
                Canvas.SetLeft(Image4Stack, image4Positionx + image4speedX);
            }

            double image5speedX = 0;
            if (Speed_x.Count > 0)
            {
                //converting speed x cordinate to double varible
                image5speedX = Convert.ToDouble(Speed_x[0]);
            }
            //Getting left property of image 5 grid stack with respect to canvas
            long image5Positionx = Convert.ToInt64(Image5Stack.GetValue(Canvas.LeftProperty));
            if (image5Positionx >= 485)
            {
                //setting image five direction to left
                Image5directionx = "left";
            }
            //if condition checking if Image five direction is left
            if (Image5directionx.Contains("left"))
            {
                //Moving Image five on x axis to the left by subtracting image5speedX
                Canvas.SetLeft(Image5Stack, image5Positionx - image5speedX);
            }

            //if condition checking if Image five position x is less then or equal to 5
            if (image5Positionx <= 5)
            {
                //setting Image five direction X to right
                Image5directionx = "right";
            }
            //if condition checking if Image five direction is right
            if (Image5directionx.Contains("right"))
            {
                //Moving Image five on x axis to the right by adding image5speedX
                Canvas.SetLeft(Image5Stack, image5Positionx + image5speedX);
            }
            // interating through items to set current position x 
            foreach (MyClass item in Filter.InitialData)
            {
                if (item.Name == "fighterjet")
                {
                    item.CurrentPositionX = image1Positionx;
                }
                else if (item.Name == "vintageaircraft")
                {
                    item.CurrentPositionX = image2Positionx;
                }
                else if (item.Name == "Hotairballoon")
                {
                    item.CurrentPositionX = image3Positionx;
                }
                else if (item.Name == "lightaircraft")
                {
                    item.CurrentPositionX = image4Positionx;
                }
                else if (item.Name == "RedHelicopter")
                {
                    item.CurrentPositionX = image5Positionx;
                }
            }
        }

        // Event handler for the animation timer (Y-axis movement)
        void timer_TickTop(object sender, EventArgs e)
        {

            //List containing Speed_y list
            List<string> Speed_y = new List<string>();
            PopulateSpeedYList(Initial_data, Speed_y);
            double image1speedY = 0;
            if(Speed_y.Count > 0)
            {
                //converting speed y cordinate to double varible
                image1speedY = Convert.ToDouble(Speed_y[0]);
            }
            

            //Getting Top property of the image 1 grid stack with respect to canvas
            long image1PositionY = Convert.ToInt64(Image1Stack.GetValue(Canvas.TopProperty));


            //if condition is checking if Image one position y is greater or equal to 355
            if (image1PositionY >= 530)
            {
                //setting image one direction Y to up
                Image1directionY = "up";
            }

            //if condition is checking if Image one Y direction is up
            if (Image1directionY.Contains("up"))
            {
                //Moving Image one on y axis upwards by subtracting image1speedX
                Canvas.SetTop(Image1Stack, image1PositionY - image1speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image1PositionY <= 5)
            {
                //setting image one direction Y to down
                Image1directionY = "down";
            }

            //if condition is checking if Image one Y direction is down
            if (Image1directionY.Contains("down"))
            {
                //Moving Image one on y axis downwards by adding image1speedX
                Canvas.SetTop(Image1Stack, image1PositionY + image1speedY);
            }

            double image2speedY = 0;
            if (Speed_y.Count > 0)
            {
                //converting speed y cordinate to double varible
                image2speedY = Convert.ToDouble(Speed_y[0]);
            }
            //Getting Top property of the image 2 grid stack with respect to canvas
            long image2PositionY = Convert.ToInt64(Image2Stack.GetValue(Canvas.TopProperty));

            if (image2PositionY >= 530)
            {
                //setting image two direction Y to up
                Image2directionY = "up";
            }

            //if condition is checking if Image two Y direction is up
            if (Image2directionY.Contains("up"))
            {
                //Moving Image one on y axis upwards by subtracting image2speedX
                Canvas.SetTop(Image2Stack, image2PositionY - image2speedY);
            }

            //if condition is checking if Image two position y is less than or equal to 5
            if (image2PositionY <= 5)
            {
                //setting image two direction Y to down
                Image2directionY = "down";
            }

            //if condition is checking if Image two Y direction is down
            if (Image2directionY.Contains("down"))
            {
                //Moving Image one on y axis downwards by adding image2speedX
                Canvas.SetTop(Image2Stack, image2PositionY + image2speedY);
            }

            double image3speedY = 0;
            if (Speed_y.Count > 0)
            {
                //converting speed y cordinate to double varible
                image3speedY = Convert.ToDouble(Speed_y[0]);
            }
            //Getting Top property of the image 3 grid stack with respect to canvas
            long image3PositionY = Convert.ToInt64(Image3Stack.GetValue(Canvas.TopProperty));

            if (image3PositionY >= 530)
            {
                //setting image three direction Y to up
                Image3directionY = "up";
            }

            //if condition is checking if Image three Y direction is up
            if (Image3directionY.Contains("up"))
            {
                //Moving Image one on y axis upwards by subtracting image3speedX
                Canvas.SetTop(Image3Stack, image3PositionY - image3speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image3PositionY <= 5)
            {
                //setting image three direction Y to down
                Image3directionY = "down";
            }

            //if condition is checking if Image three Y direction is down
            if (Image3directionY.Contains("down"))
            {
                //Moving Image one on y axis downwards by adding image3speedX
                Canvas.SetTop(Image3Stack, image3PositionY + image3speedY);
            }

            double image4speedY = 0;
            if (Speed_y.Count > 0)
            {
                //converting speed y cordinate to double varible
                image4speedY = Convert.ToDouble(Speed_y[0]);
            }
            //Getting Top property of the image 4 grid stack with respect to canvas
            long image4PositionY = Convert.ToInt64(Image4Stack.GetValue(Canvas.TopProperty));
            if (image4PositionY >= 530)
            {
                //setting image four direction Y to up
                Image4directionY = "up";
            }

            //if condition is checking if Image four Y direction is up
            if (Image4directionY.Contains("up"))
            {
                //Moving Image one on y axis upwards by subtracting image4speedX
                Canvas.SetTop(Image4Stack, image4PositionY - image4speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image4PositionY <= 5)
            {
                //setting image four direction Y to down
                Image4directionY = "down";
            }

            //if condition is checking if Image four Y direction is down
            if (Image4directionY.Contains("down"))
            {
                //Moving Image four on y axis downwards by adding image4speedX
                Canvas.SetTop(Image4Stack, image4PositionY + image4speedY);
            }

            double image5speedY = 0;
            if (Speed_y.Count > 0)
            {
                //converting speed y cordinate to double varible
                image5speedY = Convert.ToDouble(Speed_y[0]);
            }
            //Getting Top property of the image 5 grid stack with respect to canvas
            long image5PositionY = Convert.ToInt64(Image5Stack.GetValue(Canvas.TopProperty));
            if (image5PositionY >= 530)
            {
                //setting image five direction Y to up
                Image5directionY = "up";
            }

            //if condition is checking if Image five Y direction is up
            if (Image5directionY.Contains("up"))
            {
                //Moving Image one on y axis upwards by subtracting image5speedX
                Canvas.SetTop(Image5Stack, image5PositionY - image5speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image5PositionY <= 5)
            {
                //setting image five direction Y to down
                Image5directionY = "down";
            }

            //if condition is checking if Image five Y direction is down
            if (Image5directionY.Contains("down"))
            {
                //Moving Image five on y axis downwards by adding image5speedX
                Canvas.SetTop(Image5Stack, image5PositionY + image5speedY);
            }
            //iterating through items to set current position Y
            foreach (MyClass item in Filter.InitialData)
            {
                if (item.Name == "fighterjet")
                {
                    item.CurrentPositionY = image1PositionY;
                }
                else if (item.Name == "vintageaircraft")
                {
                    item.CurrentPositionY = image2PositionY;
                }
                else if (item.Name == "Hotairballoon")
                {
                    item.CurrentPositionY = image3PositionY;
                }
                else if (item.Name == "lightaircraft")
                {
                    item.CurrentPositionY = image4PositionY;
                }
                else if (item.Name == "RedHelicopter")
                {
                    item.CurrentPositionY = image5PositionY;
                }
            }
        }

        // Event handler for the "Sort by A-Z" button click
        private void SortByAZButton_Click(object sender, RoutedEventArgs e)
        {
            //Pointing to the Filter class and calling the SortAscending method in the class 

            Filter.SortAscending();
        }

        // Event handler for the "Sort by Z-A" button click
        private void SortByZAButton_Click(object sender, RoutedEventArgs e)
        { 
            //Pointing to the Filter class and calling the SortDescending method in the class 

            Filter.SortDescending();
        }

        // Event handler for the Filter's PropertyChanged event
        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Filter.InitialData))
            {
                // Handle changes to the InitialData collection
            }
        }

        // Event handler for the "Remove Selected" button click
        private void RemoveSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected in the ListBox
            if (ListBox.SelectedItem != null)
            {
                // Get the selected item
                MyClass selectedItem = (MyClass)ListBox.SelectedItem;

                // Remove the selected item from the InitialData collection
                Filter.InitialData.Remove(selectedItem);

                // Remove the corresponding animated image from the Canvas
                RemoveAnimatedImage(selectedItem.Name);
                PopulateSpeedXList(Filter.InitialData, Speed_x);
                PopulateSpeedYList(Filter.InitialData, Speed_y);
                
            }
            else
            {
                // Display a message if no item is selected
                MessageBox.Show("Please select an item to remove.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Method to remove the animated image from the Canvas based on the image name
        private void RemoveAnimatedImage(string imageName)
        {
            // Determine which animated image corresponds to the given image name and remove it from the Canvas
            if (imageName == "fighterjet")
            {
                // Remove the animated image for the fighter jet
                mainCanvas.Children.Remove(Image1Stack);
            }
            else if (imageName == "vintageaircraft")
            {
                // Remove the animated image for the vintage aircraft
                mainCanvas.Children.Remove(Image2Stack);
            }  
            else if(imageName == "Hotairballoon")
            { 
                // Remove the animated image for the Hot Air balloon
                mainCanvas.Children.Remove(Image3Stack);
            }
            else if (imageName == "lightaircraft")
            {
                // Remove the animated image for the light Aircraft
                mainCanvas.Children.Remove(Image4Stack);
            }
            else if (imageName == "RedHelicopter")
            {
                // Remove the animated image for the Red helicopter
                mainCanvas.Children.Remove(Image5Stack);
            }
        }

        // Event handler for the "Search by Type" button click
        private void SearchByTypeButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = TextBox.Text; // Get the keyword from the text box
            Filter.FilterByKeyword(keyword, mainCanvas, Image1Stack, Image2Stack, Image3Stack, Image4Stack, Image5Stack); // Point to the Filter class and call the FilterByKeyword method with the keyword and UI elements
        }

        // Event handler for the "Show Status" button click
        private void ShowStatusButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a temporary list to store the updated items
            List<MyClass> updatedItems = new List<MyClass>();

            // Iterate through the InitialData collection
            foreach (MyClass item in Filter.InitialData)
            {
                // Create a new MyClass instance with the updated values
                MyClass updatedItem = new MyClass(
                    item.Name,
                    item.Type,
                    item.Speed_x,
                    item.Speed_y,
                    item.CurrentPositionX.ToString(),
                    item.CurrentPositionY.ToString()
                );

                // Update the speed values
                updatedItem.Speed_x = Filter.GetSpeed_x(item.Name).ToString();
                updatedItem.Speed_y = Filter.GetSpeed_y(item.Name).ToString();

                // Add the updated item to the temporary list
                updatedItems.Add(updatedItem);
            }

            // Clear the InitialData collection
            Filter.InitialData.Clear();

            // Add the updated items to the InitialData collection
            foreach (var updatedItem in updatedItems)
            {
                Filter.InitialData.Add(updatedItem);
            }

            // Update the itemsSource of the ListBox with the updated InitialData
            ListBox.ItemsSource = Filter.InitialData;
        }

        // Event handler for the "Save Current" button click
        private void SaveCurrentButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the current data from the ListBox
            ObservableCollection<MyClass> currentData = (ObservableCollection<MyClass>)ListBox.ItemsSource;

            // Save the current data to the file
            FileManager.SaveCurrentData(currentData);

            // Display confirmation message of a successful save
            MessageBox.Show("List successfully saved.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Event handler for the "Load Previous Save" button click
        private void LoadPreviousSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //Stop timer for later reanimation
            StopTimer();

            // Load the saved data from the file
            ObservableCollection<MyClass> savedData = FileManager.LoadSavedData();

            // Update the CurrentPositionX and CurrentPositionY properties for each item
            foreach (var item in savedData)
            {
                item.CurrentPositionX = Convert.ToInt64(item.Position_x);
                item.CurrentPositionY = Convert.ToInt64(item.Position_y);
            }

            // Update the ListBox with the loaded data
            ListBox.ItemsSource = savedData;

            // Create a new instance of the Filter class with the loaded data
            Filter = new Filter(savedData);

            // Set the necessary instances for the new Filter instance
            Filter.mainCanvas = mainCanvas;
            Filter.SetGridInstances(Image1Stack, Image2Stack, Image3Stack, Image4Stack, Image5Stack);

            // Subscribe to the PropertyChanged event of the new Filter instance
            Filter.PropertyChanged += Filter_PropertyChanged;

            //call the load image method
            LoadImage();
            //call the ImageAnimation method with the new savedData
            ImageAnimation(savedData);
        }

        // Event handler for the "Clear All" button click
        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the InitialData collection
            Filter.InitialData.Clear();
            // Clear the canvas
            mainCanvas.Children.Clear();
            //Populate the Speed_x and Speed_y lists with data from the cleared InitialData 
            PopulateSpeedXList(Filter.InitialData, Speed_x);
            PopulateSpeedYList(Filter.InitialData, Speed_y);
            //Remove any text in the TextBox
            TextBox.Text = null;
            //Stop the timer
            timer.Stop();
        }
    }
}
