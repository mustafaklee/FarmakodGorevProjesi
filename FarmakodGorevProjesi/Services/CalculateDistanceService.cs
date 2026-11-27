using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FarmakodGorevProjesi.Services
{
    public class CalculateDistanceService
    {
        public int calculate(int robotX, int robotY, int productX, int productY)
        {
            int distance;
            distance = Math.Abs(productX - robotX) + Math.Abs(productY - robotY);
            return distance;
        }
 
    }
}
