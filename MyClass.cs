using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark_Assignment
{
     class MyClass
    {
        string name;

        string type;

        string speed_x; 

        string speed_y;

        string position_x; 

        string position_y;

        public string Name
        {
            get { return name; }

            set { name = value; }
        }

        public string Type
        {
            get { return type; }

            set { type = value; }
        }

        public string Speed_x
        {
            get { return speed_x; }

            set { speed_x = value; }
        }

        public string Speed_y
        {
            get { return speed_y; } 

            set { speed_y = value; }
        }

        public string Position_x
        {
            get { return position_x; }

            set { position_x = value; }
        }

        public string Position_y
        {
            get { return position_y; }

            set { position_y = value; }
        }

        public MyClass(string name, string type, string speed_x, string speed_y, string position_x, string position_y)
        {
            this.name = name;
            this.type = type;
            this.speed_x = speed_x;
            this.speed_y = speed_y;
            this.position_x = position_x;
            this.position_y = position_y;
        }

        public override string ToString()
        {
            return name + "   |    " + type + "    |     X " + speed_x + "    |    Y " + speed_y + "    |   X " + position_x + "    |    Y " + position_y;
        }
    }
}
