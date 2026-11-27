using FarmakodGorevProjesi.Domain;
using FarmakodGorevProjesi.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmakodGorevProjesi.Services
{
    public class FindProductService
    {
        public Result FindProduct(string code,List<Product> productList)
        {
            bool success;

            if (string.IsNullOrEmpty(code))
            {
                return new Result(false, null);
            }

            Product foundProduct = productList.Find(c => c.code == code);
            success = foundProduct != null;
            return new Result(success, foundProduct);
        }
    }
}
