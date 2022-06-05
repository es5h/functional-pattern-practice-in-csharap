using static System.Math;

namespace functional_pattern_practice_in_csharap;

internal record Circle(double Radius)
{
    // Using Static Style is applied.
    public double Circumference => PI * 2 * Radius;

    public double Area
    {
        get
        { 
            double Square(double d) => Pow(d, 2); // lambda local function
            //return PI * Square(Radius);
            
            var square = (double d) => Pow(d, 2); // Delegate Style
            return PI * square(Radius);
        }
    }
}