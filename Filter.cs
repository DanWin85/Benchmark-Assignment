using System.IO;

namespace Benchmark_Assignment
{
    class Filter
    {
        private List<MyClass> data;

        public Filter(List<MyClass> data)
        {
            this.data = data;
        }

        // Method to search data based on a keyword
        public List<MyClass> Search(string keyword)
        {
            return data.Where(item => item.Name.ToLower().Contains(keyword.ToLower())).ToList();
        }

        // Method to sort data in ascending order based on the name
        public void SortAscending()
        {
            data = data.OrderBy(item => item.Name).ToList();
        }

        // Method to sort data in descending order based on the name
        public void SortDescending()
        {
            data = data.OrderByDescending(item => item.Name).ToList(); ;
        }

        public List<MyClass> GetData()
        {
            return data;
        }

    }
}