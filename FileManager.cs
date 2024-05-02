using System.IO;
using System.Collections.ObjectModel;

namespace Benchmark_Assignment
{
    // This class is responsible for managing file operations
    public class FileManager
    {
        private const string InitialDataFileName = "ImageData.txt"; // File name for storing initial data
        private const string SavedDataFileName = "SavedImageData.txt"; // File name for storing saved data

        // Method to save the current data to a file
        public static void SaveCurrentData(ObservableCollection<MyClass> data)
        {
            try
            {
                // Open the file for writing
                using (StreamWriter writer = new StreamWriter(SavedDataFileName))
                {
                    // Iterate through each item in the data collection
                    foreach (MyClass item in data)
                    {
                        // Write each item's properties to a new line in the file
                        writer.WriteLine($"{item.Name},{item.Type},{item.Speed_x},{item.Speed_y},{item.Position_x},{item.Position_y}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the file write operation
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        // Method to load the saved data from a file
        public static ObservableCollection<MyClass> LoadSavedData()
        {
            ObservableCollection<MyClass> data = new ObservableCollection<MyClass>();

            try
            {
                // Open the file for reading
                using (StreamReader reader = new StreamReader(SavedDataFileName))
                {
                    string line;
                    // Read each line from the file
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split the line into an array using commas as the delimiter
                        string[] values = line.Split(',');
                        // Check if the line has the expected number of elements
                        if (values.Length == 6)
                        {
                            // Trim any leading or trailing whitespace from the values
                            string[] trimmedValues = values.Select(v => v.Trim()).ToArray();
                            // Create a new instance of MyClass using the trimmed values
                            MyClass imageInfo = new MyClass(trimmedValues[0], trimmedValues[1], trimmedValues[2], trimmedValues[3], trimmedValues[4], trimmedValues[5]);
                            // Add the image information to the data collection
                            data.Add(imageInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the file read operation
                Console.WriteLine($"Error loading data: {ex.Message}");
            }

            return data;
        }

        // Method to load the initial data from a file
        public string LoadInitialData()
        {
            string data = null;

            try
            {
                // Open the file for reading
                using (StreamReader reader = new StreamReader(InitialDataFileName))
                {
                    // Read the entire file content into a string
                    data = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the file read operation
                Console.WriteLine($"Error loading data: {ex.Message}");
            }

            return data;
        }
    }
}