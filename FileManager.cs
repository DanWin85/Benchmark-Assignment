using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Benchmark_Assignment
{
    public class FileManager
    {

        private const string FileName = "ImageData.txt";

        public void SaveData(object data)
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
