using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Client.Model
{
    class Car
    {
        public int CarId { get; set; }
        public string Name { get; set; }
        public CarType Typ { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
