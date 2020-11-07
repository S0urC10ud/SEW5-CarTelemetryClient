using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace REST_Client.Model
{
    public class Car : CarToSend
    {
        public int CarId { get; set; }

        public void updateType()
        {
            this.Typ = Types[Int32.Parse(this.Typ)];
        }


        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public string Typ { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand UpdateCommand { get; set; }
    }
}
