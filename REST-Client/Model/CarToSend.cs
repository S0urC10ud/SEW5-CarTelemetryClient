using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Client.Model
{
    class CarToSend
    {

        public void setType(string typeString)
        {
            IList<string> tempList = new List<string>();
            tempList.Add("SUV");
            tempList.Add("Micro");
            tempList.Add("Minivan");
            tempList.Add("Sedan");
            tempList.Add("CUV");
            tempList.Add("Roadster");
            this.Typ=tempList.IndexOf(typeString);
        }

        public string Name { get; set; }
        public int Typ { get; set; }
    }
}
