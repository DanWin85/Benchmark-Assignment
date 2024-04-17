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

namespace Benchmark_Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private FileManager? fileManager;

        DispatcherTimer timer = new DispatcherTimer();
        List<MyClass> Initial_data = new List<MyClass>();

        private string Image1directionx = "right";

        private string Image1directionY = "down";

        private string Image2directionx = "up";

        private string Image2directionY = "down";
        public MainWindow()
        {
            InitializeComponent();
            fileManager = new FileManager();
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string loadedData = fileManager.LoadData();

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
            }
          
            //try loading another image of your choice - this requires adding another image control to the XAML, adding new image to this application and its info. to the text file (like Sheep) 
            //then set the directions for image 2 like how we did in Timer_xdirection() and timer_TickTop() methods using a new set of variables (for image 2)
        }

        public void ImageAnimation()
        {
            // calling the event handler to move the image y direction(Up or Down)
            timer.Tick += new EventHandler(timer_TickTop);
            // calling the event handler to move the image x direction(Left or Right)
            timer.Tick += new EventHandler(Timer_xdirection);
            // Seting interval for dispatch timer Animation will repeat every 2 milliseconds
            timer.Interval = new TimeSpan(0, 0, 0, 0, 2);
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

            double image2speedX = Convert.ToDouble(Speed_x[0]);

            long image2Positionx = Convert.ToInt64(Image1Stack.GetValue(Canvas.LeftProperty));
            if (image2Positionx >= 400)
            {
                //setting image one direction to left
                Image2directionx = "left";
            }
            //if condition checking if Image one direction is left
            if (Image2directionx.Contains("left"))
            {
                //Maving Image one on x axis to the left by subtracting image1speedX
                Canvas.SetLeft(Image2Stack, image2Positionx - image2speedX);
            }

            //if condition checking if Image one position x is less then or equal to 5
            if (image2Positionx <= 5)
            {
                //setting image one direction X to right
                Image2directionx = "right";
            }
            //if condition checking if Image one direction is right
            if (Image2directionx.Contains("right"))
            {
                //Maving Image one on x axis to the right by adding image1speedX
                Canvas.SetLeft(Image2Stack, image2Positionx + image2speedX);
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
            double image2speedY = Convert.ToDouble(Speed_y[0]);

            long image2PositionY = Convert.ToInt64(Image1Stack.GetValue(Canvas.TopProperty));

            if (image2PositionY >= 300)
            {
                //setting image one direction Y to up
                Image2directionY = "up";
            }

            //if condition is checking if Image one Y direction is up
            if (Image2directionY.Contains("up"))
            {
                //Maving Image one on y axis upwards by subtracting image1speedX
                Canvas.SetTop(Image2Stack, image2PositionY - image2speedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (image2PositionY <= 5)
            {
                //setting image one direction Y to down
                Image2directionY = "down";
            }

            //if condition is checking if Image one Y direction is down
            if (Image2directionY.Contains("down"))
            {
                //Maving Image one on y axis downwards by adding image1speedX
                Canvas.SetTop(Image2Stack, image2PositionY + image2speedY);
            }
        }
    }
}
