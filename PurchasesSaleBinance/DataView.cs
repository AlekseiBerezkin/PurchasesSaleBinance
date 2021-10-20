using System;
using System.Collections.Generic;
using System.Text;

namespace PurchasesSaleBinance
{
    class DataView
    {
       public string symbol { get; set; }
       public decimal CurrentValue { get; set; }
       public string link { get; set; }
       public decimal futures { get; set; }
       public decimal shoulder { get; set; }
       public decimal StopLos { get; set; }
       public decimal TakeProfit { get; set; }
       public double PNL { get; set; }
    }
}
