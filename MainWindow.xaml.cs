using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using static System.Net.WebRequestMethods;

namespace Benchmark_Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private FileManager? fileManager;
        private Filter Filter;
        

        DispatcherTimer timer = new DispatcherTimer();
        List<MyClass> Initial_data = new List<MyClass>();

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
        public MainWindow()
        {
            InitializeComponent();
            fileManager = new FileManager();
            Filter = new Filter(ListBox.Items.Cast<MyClass>().ToList());
           // ListBox.ItemsSource = Filter.GetData();
        }

        

        private void LoadInitialDataButton_Click(object sender, RoutedEventArgs e)
        {
            string loadedData = fileManager.LoadData();

            // Check if the loaded data is not null
            if (!string.IsNullOrEmpty(loadedData))
            {
                string[] lines = loadedData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                // Clear the existing data in Initial_data list
                Initial_data.Clear();

                // Clear all animations from the Canvas
                //mainCanvas.Children.Clear();

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
                    }
                }

                // Clear the ListBox items
              ListBox.Items.Clear();

                // Add each image information to the ListBox
                foreach (var imageInfo in Initial_data)
                {
                    ListBox.Items.Add(imageInfo);
                }

                // Call the LoadImage method
                LoadImage();
            }
            else
            {
                // Handle the case where the loaded data is null or empty
                MessageBox.Show("Error loading data. The file may be empty or not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void LoadImage()
        {
            //Here a new list is created is created to save all the names of images so these can be used to load the corresponding images on the canvas
            List<string> Name = new List<string>();
            //we are iterating over the initial_data object list to access the names of images and add them to the new list-Name
            foreach (MyClass a in Initial_data)
            {
                Name.Add(a.Name);
            }

            foreach (string imageName in Name)
            {
                if (imageName == "fighterjet")
                {
                    //Creating a Uri object img1. Loading the relative image pathway
                    Uri img1 = new Uri("pack://application:,,,/images/fighterjet.jpg");
                    //Making the image1 source equal to object pathway
                    Image1.Source = new BitmapImage(img1);
                    //Once loaded the custom method below will help animate the loaded object
                    ImageAnimation();
                }
                else if (imageName == "vintageaircraft")
                {
                    Uri img2 = new Uri("pack://application:,,,/images/vintageaircraft.jpg");
                    Image2.Source = new BitmapImage(img2);
                    ImageAnimation();
                }
                else if (imageName == "Hotairballoon")
                {
                    Uri img3 = new Uri("pack://application:,,,/images/Hotairballoon.jpg");
                    Image3.Source = new BitmapImage(img3);
                    ImageAnimation();
                }
                else if (imageName == "lightaircraft")
                {
                    Uri img4 = new Uri("pack://application:,,,/images/lightaircraft.jpg");
                    Image4.Source = new BitmapImage(img4);
                    ImageAnimation();
                }
                else if (imageName == "RedHelicopter")
                {
                    Uri img5 = new Uri("pack://application:,,,/images/RedHelicopter.jpg");
                    Image5.Source = new BitmapImage(img5);
                    ImageAnimation();
                }

            }
        }

        public void ImageAnimation()
        {
            // calling the event handler to move the image y direction(Up or Down)
            timer.Tick += new EventHandler(timer_TickTop);
            // calling the event handler to move the image x direction(Left or Right)
            timer.Tick += new EventHandler(Timer_xdirection);
            // Seting interval for dispatch timer Animation will repeat every 2 milliseconds
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            //Starting dispatch timer.
            timer.Start();

        }

        void Timer_xdirection(object sender, EventArgs e)
        {
            //List containing Speed_x list
            List<string> Speed_x = new List<string>();
            //Looping through initial data 
            foreach (MyClass c in Initial_data)
            {
                //Adding Speed x cordinate to list. Also getting rid of negative values
                Speed_x.Add(c.Speed_x.TrimStart('-'));
            }
            //converting speed x cordinate to double varible
            double image1speedX = Convert.ToDouble(Speed_x[0]);
           

            //Getting left property of the image grid stack with respect to canvas
            long image1Positionx = Convert.ToInt64(Image1Stack.GetValue(Canvas.LeftProperty));
            
            //if condition checking if Image position x is greater or equal to 390
            if (image1Positionx >= 390)
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
                //Maving Image one on x axis to the right by adding image1speedX
                Canvas.SetLeft(Image1Stack, image1Positionx + image1speedX);
            }

            double image2speedX = Convert.ToDouble(Speed_x[1]);
            long image2Positionx = Convert.ToInt64(Image2Stack.GetValue(Canvas.LeftProperty));
            if (image2Positionx >= 400)
            {
                //setting image one direction to left
                Image2directionx = "right";
            }
            //if condition checking if Image one direction is left
            if (Image2directionx.Contains("right"))
            {
                //Maving Image one on x axis to the left by subtracting image1speedX
                Canvas.SetLeft(Image2Stack, image2Positionx - image2speedX);
            }

            //if condition checking if Image one position x is less then or equal to 5
            if (image2Positionx <= 10)
            {
                //setting image one direction X to right
                Image2directionx = "left";
            }
            //if condition checking if Image one direction is right
            if (Image2directionx.Contains("left"))
            {
                //Maving Image one on x axis to the right by adding image1speedX
                Canvas.SetLeft(Image2Stack, image2Positionx + image2speedX);
            }

            double image3speedX = Convert.ToDouble(Speed_x[2]);
            long image3Positionx = Convert.ToInt64(Image3Stack.GetValue(Canvas.LeftProperty));
            if (image3Positionx >= 390)
            {
                //setting image one direction to left
                Image3directionx = "right";
            }
            //if condition checking if Image one direction is left
            if (Image3directionx.Contains("right"))
            {
                //Maving Image one on x axis to the left by subtracting image1speedX
                Canvas.SetLeft(Image3Stack, image3Positionx - image3speedX);
            }

            //if condition checking if Image one position x is less then or equal to 5
            if (image3Positionx <= 10)
            {
                //setting image one direction X to right
                Image3directionx = "left";
            }
            //if condition checking if Image one direction is right
            if (Image3directionx.Contains("left"))
            {
                //Maving Image one on x axis to the right by adding image1speedX
                Canvas.SetLeft(Image3Stack, image3Positionx + image3speedX);
            }

            double image4speedX = Convert.ToDouble(Speed_x[3]);
            long image4Positionx = Convert.ToInt64(Image4Stack.GetValue(Canvas.LeftProperty));
            if (image4Positionx >= 390)
            {
                //setting image one direction to left
                Image4directionx = "right";
            }
            //if condition checking if Image one direction is left
            if (Image4directionx.Contains("right"))
            {
                //Maving Image one on x axis to the left by subtracting image1speedX
                Canvas.SetLeft(Image4Stack, image4Positionx - image4speedX);
            }

            //if condition checking if Image one position x is less then or equal to 5
            if (image4Positionx <= 10)
            {
                //setting image one direction X to right
                Image4directionx = "left";
            }
            //if condition checking if Image one direction is right
            if (Image4directionx.Contains("left"))
            {
                //Maving Image one on x axis to the right by adding image1speedX
                Canvas.SetLeft(Image4Stack, image4Positionx + image4speedX);
            }

            double image5speedX = Convert.ToDouble(Speed_x[4]);
            long image5Positionx = Convert.ToInt64(Image5Stack.GetValue(Canvas.LeftProperty));
            if (image5Positionx >= 390)
            {
                //setting image one direction to left
                Image5directionx = "left";
            }
            //if condition checking if Image one direction is left
            if (Image5directionx.Contains("left"))
            {
                //Maving Image one on x axis to the left by subtracting image1speedX
                Canvas.SetLeft(Image5Stack, image5Positionx - image5speedX);
            }

            //if condition checking if Image one position x is less then or equal to 5
            if (image5Positionx <= 5)
            {
                //setting image one direction X to right
                Image5directionx = "right";
            }
            //if condition checking if Image one direction is right
            if (Image5directionx.Contains("right"))
            {
                //Maving Image one on x axis to the right by adding image1speedX
                Canvas.SetLeft(Image5Stack, image5Positionx + image5speedX);
            }

        }

        //Timer Method2
        void timer_TickTop(object sender, EventArgs e)
        {

            //List containing Speed_y list
            List<string> Speed_y = new List<string>();
            Speed_y.Clear();
            foreach (MyClass c in Initial_data)
            {
                //Adding Speed y cordinate to list. Also getting rid of negative values
                Speed_y.Add(c.Speed_y.TrimStart('-'));
            }

            //converting speed y cordinate to double varible
            double image1speedY = Convert.ToDouble(Speed_y[0]);
           
            //Getting Top property of the image 1 grid stack with respect to canvas
            long image1PositionY = Convert.ToInt64(Image1Stack.GetValue(Canvas.TopProperty));
            

            //if condition is checking if Image one position y is greater or equal to 355
            if (image1PositionY >= 355)
            {
                //setting image one direction Y to up
                Image1directionY = "up";
            }

            //if condition is checking if Image one Y direction is up
            if (Image1directionY.Contains("up"))
            {
                //Maving Image one on y axis upwards by subtracting image1speedX
                Canvas.SetTop(Image1Stack, image1PositionY - image1speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image1PositionY <= 15)
            {
                //setting image one direction Y to down
                Image1directionY = "down";
            }

            //if condition is checking if Image one Y direction is down
            if (Image1directionY.Contains("down"))
            {
                //Maving Image one on y axis downwards by adding image1speedX
                Canvas.SetTop(Image1Stack, image1PositionY + image1speedY);
            }
            double image2speedY = Convert.ToDouble(Speed_y[1]);

            long image2PositionY = Convert.ToInt64(Image2Stack.GetValue(Canvas.TopProperty));

            if (image2PositionY >= 355)
            {
                //setting image one direction Y to up
                Image2directionY = "down";
            }

            //if condition is checking if Image one Y direction is up
            if (Image2directionY.Contains("down"))
            {
                //Maving Image one on y axis upwards by subtracting image1speedX
                Canvas.SetTop(Image2Stack, image2PositionY - image2speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image2PositionY <= 50)
            {
                //setting image one direction Y to down
                Image2directionY = "up";
            }

            //if condition is checking if Image one Y direction is down
            if (Image2directionY.Contains("up"))
            {
                //Maving Image one on y axis downwards by adding image1speedX
                Canvas.SetTop(Image2Stack, image2PositionY + image2speedY);
            }

            double image3speedY = Convert.ToDouble(Speed_y[2]);

            long image3PositionY = Convert.ToInt64(Image3Stack.GetValue(Canvas.TopProperty));

            if (image3PositionY >= 355)
            {
                //setting image one direction Y to up
                Image3directionY = "down";
            }

            //if condition is checking if Image one Y direction is up
            if (Image3directionY.Contains("down"))
            {
                //Maving Image one on y axis upwards by subtracting image1speedX
                Canvas.SetTop(Image3Stack, image3PositionY - image3speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image3PositionY <= 50)
            {
                //setting image one direction Y to down
                Image3directionY = "up";
            }

            //if condition is checking if Image one Y direction is down
            if (Image3directionY.Contains("up"))
            {
                //Maving Image one on y axis downwards by adding image1speedX
                Canvas.SetTop(Image3Stack, image3PositionY + image3speedY);
            }

            double image4speedY = Convert.ToDouble(Speed_y[3]);
            long image4PositionY = Convert.ToInt64(Image4Stack.GetValue(Canvas.TopProperty));
            if (image4PositionY >= 355)
            {
                //setting image one direction Y to up
                Image4directionY = "up";
            }

            //if condition is checking if Image one Y direction is up
            if (Image4directionY.Contains("up"))
            {
                //Maving Image one on y axis upwards by subtracting image1speedX
                Canvas.SetTop(Image4Stack, image4PositionY - image4speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image4PositionY <= 5)
            {
                //setting image one direction Y to down
                Image4directionY = "down";
            }

            //if condition is checking if Image one Y direction is down
            if (Image4directionY.Contains("down"))
            {
                //Maving Image one on y axis downwards by adding image5speedX
                Canvas.SetTop(Image4Stack, image4PositionY + image4speedY);
            }

            double image5speedY = Convert.ToDouble(Speed_y[4]);
            long image5PositionY = Convert.ToInt64(Image5Stack.GetValue(Canvas.TopProperty));
            if (image5PositionY >= 355)
            {
                //setting image one direction Y to up
                Image5directionY = "up";
            }

            //if condition is checking if Image one Y direction is up
            if (Image5directionY.Contains("up"))
            {
                //Maving Image one on y axis upwards by subtracting image1speedX
                Canvas.SetTop(Image5Stack, image5PositionY - image5speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image5PositionY <= 5)
            {
                //setting image one direction Y to down
                Image5directionY = "down";
            }

            //if condition is checking if Image one Y direction is down
            if (Image5directionY.Contains("down"))
            {
                //Maving Image one on y axis downwards by adding image1speedX
                Canvas.SetTop(Image5Stack, image5PositionY + image5speedY);
            }

        }

        private void SortByAZButton_Click(object sender, RoutedEventArgs e)
        {
            Filter.SortAscending();
            ListBox.Items.Refresh();
        }

        private void SortByZAButton_Click(object sender, RoutedEventArgs e)
        {
            Filter.SortDescending();
            ListBox.Items.Refresh();
        }

        private void RemoveSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected in the ListBox
            if (ListBox.SelectedItem != null)
            {
                // Remove the selected item from the ListBox
                MyClass selectedItem = (MyClass)ListBox.SelectedItem;
                ListBox.Items.Remove(selectedItem);

                // Remove the corresponding animated image from the Canvas
                RemoveAnimatedImage(selectedItem.Name);
            }
            else
            {
                // Display a message if no item is selected
                MessageBox.Show("Please select an item to remove.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchByTypeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowStatusButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveCurrentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadPreviousSaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            //Clear the listbox
            ListBox.Items.Clear();
            // Clear the canvas
            mainCanvas.Children.Clear();
        }
    }
}
