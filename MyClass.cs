using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Benchmark_Assignment
{
   public class MyClass : INotifyPropertyChanged
    {
        private string _name;
        private string _type;
        private string _speed_x;
        private string _speed_y;
        private string _position_x;
        private string _position_y;

        public event PropertyChangedEventHandler PropertyChanged;
        private long _currentPositionX;
        private long _currentPositionY;

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

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type, Speed_x, Speed_y, Position_x, Position_y);
        }
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


        public MyClass(string name, string type, string speed_x, string speed_y, string position_x, string position_y)
        {
            _name = name;
            _type = type;
            _speed_x = speed_x;
            _speed_y = speed_y;
            _position_x = position_x;
            _position_y = position_y;
        }

        public override string ToString()
        {
            return Name + "  |  " + Type + "  |  X " + Speed_x + "  |  Y " + Speed_y + "  |  X " + Position_x + "  |  Y " + Position_y;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}