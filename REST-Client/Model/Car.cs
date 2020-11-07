﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace REST_Client.Model
{
    public class Car
    {
        public int CarId { get; set; }
        public string Name { get; set; }

        public void updateType()
        {
            IList<string> tempList = new List<string>();
            tempList.Add("SUV");
            tempList.Add("Micro");
            tempList.Add("Minivan");
            tempList.Add("Sedan");
            tempList.Add("CUV");
            tempList.Add("Roadster");
            this.Typ = tempList[Int32.Parse(this.Typ)];
        }

        public string Typ { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ICommand DeleteCommand { get; set; }
    }
}
