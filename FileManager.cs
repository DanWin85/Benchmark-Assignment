using System.IO;

namespace Benchmark_Assignment
{
    class FileManager
    {
        private const string FileName = "ImageData.txt";

        public static void SaveData(object data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FileName))
                {
                    writer.WriteLine(data.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public string LoadData()
        {
            string data = null;

            try
            {
                using (StreamReader reader = new StreamReader(FileName))
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
