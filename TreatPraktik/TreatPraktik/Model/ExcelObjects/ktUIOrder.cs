﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreatPraktik.Model
{
    public class ktUIOrder
    {
        public string DesignID { get; set; }
        public Double GroupOrder { get; set; }
        public string GroupTypeID { get; set; }
        public string PageTypeID { get; set; }
        public string IncludedTypeID { get; set; }

        public ktUIOrder()
        {

        }
    }
}
