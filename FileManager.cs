using System.IO;
using System.Collections.ObjectModel;

namespace Benchmark_Assignment
{
   public class FileManager
    {
        private const string InitialDataFileName = "ImageData.txt";
        private const string SavedDataFileName = "SavedImageData.txt";

        public static void SaveCurrentData(ObservableCollection<MyClass> data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SavedDataFileName))
                {
                    foreach (MyClass item in data)
                    {
                        writer.WriteLine($"{item.Name},{item.Type},{item.Speed_x},{item.Speed_y},{item.Position_x},{item.Position_y}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public static ObservableCollection<MyClass> LoadSavedData()
        {
            ObservableCollection<MyClass> data = new ObservableCollection<MyClass>();

            try
            {
                using (StreamReader reader = new StreamReader(SavedDataFileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');

                        if (values.Length == 6)
                        {
                            string[] trimmedValues = values.Select(v => v.Trim()).ToArray();
                            MyClass imageInfo = new MyClass(trimmedValues[0], trimmedValues[1], trimmedValues[2], trimmedValues[3], trimmedValues[4], trimmedValues[5]);
                            data.Add(imageInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }

            return data;
        }

        public string LoadInitialData()
        {
            string data = null;

            try
            {
                using (StreamReader reader = new StreamReader(InitialDataFileName))
                {
                    data = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }

            return data;
        }
    }
}