using FarmakodGorevProjesi.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmakodGorevProjesi.Results
{
    public class Result
    {

        public bool success;
        public Product product;

        public  Result(bool success,Product product)
        {
            this.success = success;
            this.product = product;
        }
    }
}
