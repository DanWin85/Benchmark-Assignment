using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Benchmark_Assignment
{
    // This class represents an object with properties like name, type, speed, and position
    // It implements the INotifyPropertyChanged interface to notify when a property value changes
    public class MyClass : INotifyPropertyChanged
    {
        private string _name;
        private string _type;
        private string _speed_x;
        private string _speed_y;
        private string _position_x;
        private string _position_y;

        // Event to notify when a property value changes
        public event PropertyChangedEventHandler PropertyChanged;
        private long _currentPositionX;
        private long _currentPositionY;

        // Override the Equals method to compare objects based on their properties (added this block to help get a test in the filter class to pass)
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            MyClass other = (MyClass)obj;
            return Name == other.Name &&
                   Type == other.Type &&
                   Speed_x == other.Speed_x &&
                   Speed_y == other.Speed_y &&
                   Position_x == other.Position_x &&
                   Position_y == other.Position_y;
        }

        // Override the GetHashCode method to generate a hash code based on the object's properties (added this block to help get a test in the filter class to pass)
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type, Speed_x, Speed_y, Position_x, Position_y);
        }

        // Property to get or set the current position on the X-axis
        // Raises the PropertyChanged event when the Position X value changes
        public long CurrentPositionX
        {
            get { return _currentPositionX; }
            set
            {
                if (_currentPositionX != value)
                {
                    _currentPositionX = value;
                    OnPropertyChanged(nameof(CurrentPositionX));
                }
            }
        }
        // Property to get or set the current position on the Y-axis
        // Raises the PropertyChanged event when the Position Y value changes
        public long CurrentPositionY
        {
            get { return _currentPositionY; }
            set
            {
                if (_currentPositionY != value)
                {
                    _currentPositionY = value;
                    OnPropertyChanged(nameof(CurrentPositionY));
                }
            }
        }

        // Property to get or set the name
        // Raises the PropertyChanged event when the 'Name' value changes
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property to get or set the Type
        // Raises the PropertyChanged event when the 'Type' value changes
        public string Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property to get or set the speed on the X-axis
        // Raises the PropertyChanged event when the 'Speed_x' value changes
        public string Speed_x
        {
            get { return _speed_x; }
            set
            {
                if (_speed_x != value)
                {
                    _speed_x = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property to get or set the speed on the Y-axis
        // Raises the PropertyChanged event when the 'Speed_y' value changes
        public string Speed_y
        {
            get { return _speed_y; }
            set
            {
                if (_speed_y != value)
                {
                    _speed_y = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property to get or set the position on the X-axis
        // Raises the PropertyChanged event when the 'Position_x' value changes
        public string Position_x
        {
            get { return _position_x; }
            set
            {
                if (_position_x != value)
                {
                    _position_x = value;
                    OnPropertyChanged();
                }
            }
        }

        // Property to get or set the position on the Y-axis
        // Raises the PropertyChanged event when the 'Position_y' value changes
        public string Position_y
        {
            get { return _position_y; }
            set
            {
                if (_position_y != value)
                {
                    _position_y = value;
                    OnPropertyChanged();
                }
            }
        }

        // Constructor to initialize the object with name, type, speed, and position
        public MyClass(string name, string type, string speed_x, string speed_y, string position_x, string position_y)
        {
            _name = name;
            _type = type;
            _speed_x = speed_x;
            _speed_y = speed_y;
            _position_x = position_x;
            _position_y = position_y;
        }

        // Override the ToString method to provide a string representation of the object
        public override string ToString()
        {
            return Name + "  |  " + Type + "  |  X " + Speed_x + "  |  Y " + Speed_y + "  |  X " + Position_x + "  |  Y " + Position_y;
        }

        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}