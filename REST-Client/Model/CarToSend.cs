using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Client.Model
{
    public class CarToSend
    {
        [JsonIgnore]
        public IList<string> Types { get; set; } = new List<string> { "SUV", "Micro", "Minivan", "Sedan", "CUV", "Roadster" };

        public void setType(string typeString)
        {
            this.Typ=Types.IndexOf(typeString);
        }

        public string Name { get; set; }
        public int Typ { get; set; }
    }
}
