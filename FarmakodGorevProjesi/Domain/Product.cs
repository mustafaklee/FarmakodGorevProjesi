using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using System.Text;

namespace FarmakodGorevProjesi.Domain
{
    public class Product
    {
        public string? code { get; set; }
        public string? name { get; set; } 
        public int X { get; set; }
        public int Y { get; set; }
    }
}
