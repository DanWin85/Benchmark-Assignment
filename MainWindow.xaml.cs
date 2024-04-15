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
        private FileManager fileManager = new FileManager();

        DispatcherTimer timer = new DispatcherTimer();
        List<MyClass> Initial_data = new List<MyClass>();

        private string Bluejetdirectionx = "right";
        
        private string Bluejetdirectiony = "down";
        public MainWindow()
        {
            InitializeComponent();
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

            //If condition is checking the name of the image so the relevant image can be loaded on the canvas.
            if (Name.Contains("Bluejet"))
            {
                //Creating a Uri object img1. Loading the relative image pathway
                Uri Bluejet = new Uri("Bluejet.jpg", UriKind.Relative);
                //Making the image1 source equal to object pathway
                BitmapImage bluejetImage = new BitmapImage(Bluejet);
                //Once loaded the custom method below will help animate the loaded object
                ImageAnimation();
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
            double BluejetspeedX = Convert.ToDouble(Speed_x[0]);
            //Getting left property of the image 1 grid stack with respect to canvas
            long BluejetPositionx = Convert.ToInt64(BluejetStack.GetValue(Canvas.LeftProperty));
            //if condition checking if Image one position x is greater or equal to 390
            if (BluejetPositionx >= 390)
            {
                //setting image one direction to left
                Bluejetdirectionx = "left";
            }
            //if condition checking if Image one direction is left
            if (Bluejetdirectionx.Contains("left"))
            {
                //Maving Image one on x axis to the left by subtracting image1speedX
                Canvas.SetLeft(BluejetStack, BluejetPositionx - BluejetspeedX);
            }

            //if condition checking if Image one position x is less then or equal to 5
            if (BluejetPositionx <= 5)
            {
                //setting image one direction X to right
                Bluejetdirectionx = "right";
            }
            //if condition checking if Image one direction is right
            if (Bluejetdirectionx.Contains("right"))
            {
                //Maving Image one on x axis to the right by adding image1speedX
                Canvas.SetLeft(BluejetStack, BluejetPositionx + BluejetspeedX);
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
            double BluejetspeedY = Convert.ToDouble(Speed_y[0]);
            //Getting Top property of the image 1 grid stack with respect to canvas
            long BluejetPositionY = Convert.ToInt64(BluejetStack.GetValue(Canvas.TopProperty));

            //if condition is checking if Image one position y is greater or equal to 355
            if (BluejetPositionY >= 355)
            {
                //setting image one direction Y to up
                Bluejetdirectiony = "up";
            }

            //if condition is checking if Image one Y direction is up
            if (Bluejetdirectiony.Contains("up"))
            {
                //Maving Image one on y axis upwards by subtracting image1speedX
                Canvas.SetTop(BluejetStack, BluejetPositionY - BluejetspeedY);
            }

            //if condition is checking if Image one position y is less than or equal to 5
            if (BluejetPositionY <= 5)
            {
                //setting image one direction Y to down
                Bluejetdirectiony = "down";
            }

            //if condition is checking if Image one Y direction is down
            if (Bluejetdirectiony.Contains("down"))
            {
                //Maving Image one on y axis downwards by adding image1speedX
                Canvas.SetTop(BluejetStack, BluejetPositionY + BluejetspeedY);
            }


        }

   


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string loadedData = fileManager.LoadData();

            // Check if the loaded data is not null
            if (!string.IsNullOrEmpty(loadedData))
            {
                string[] lines = loadedData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                // Declare a list to store the image information
                List<MyClass> initialData = new List<MyClass>();

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

                            // Add the image information to the list
                            initialData.Add(imageInfo);
                        }
                }

                // Clear the ListBox items
                ListBox.Items.Clear();

                // Add each image information to the ListBox
                foreach (var imageInfo in initialData)
                {
                    ListBox.Items.Add(imageInfo);
                }

                // Call the LoadImage method
                LoadImage();
            }
            else
            {
                // Handle the case where the loaded data is null or empty
                // For example, you could display an error message or take appropriate action
                MessageBox.Show("Error loading data. The file may be empty or not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        }
    }
