using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace CSharpApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car(4, "Toyota", Tuple.Create(1.5, 3.5));
            var bike = Vehicle.NewMotorbike("Bike", 50.0);

            Functions.CreateCar(4, "Toyota", 2, 3);
            Functions.CreateFourWheeledCar.Invoke("Audi").Invoke(3.0).Invoke(4.0);
        }
    }
}
