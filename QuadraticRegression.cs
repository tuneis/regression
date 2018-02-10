using System;
using System.Linq;

namespace Tuneis
{
    /// <summary>
    /// See here for formulas used. http://keisan.casio.com/exec/system/14059932254941.
    /// </summary>
    public class QuadraticRegression
    {
        /// <summary>
        /// X Values.
        /// </summary>
        double[] _x;

        /// <summary>
        /// Y Values
        /// </summary>
        double[] _y;

        /// <summary>
        /// Resulting equation.
        /// </summary>
        public QuadraticRegressionEquation Equation { get; set; }

        /// <summary>
        /// Parabola.
        /// </summary>
        public Point[] Parabola { get; set; }

        /// <summary>
        /// X values.
        /// </summary>
        public double[] X { get; set; }

        /// <summary>
        /// Y Values.
        /// </summary>
        public double[] Y { get; set; }

        /// <summary>
        /// Pass 2 arrays of values to the constructor and the equation and parabola will be generated automatically.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public QuadraticRegression(double[] x, double[] y)
        {
            // check to see if arrays are of equal length
            if (x.Length != y.Length)
            {
                throw new Exception("X array and Y array must be of same length.");
            }
            if (x == null || y == null)
            {
                throw new NullReferenceException("X array or Y array cannot be null.");
            }

            _x = X = x;
            _y = Y = y;
            Equation = GenerateEquation();
            Parabola = CalculateParabola(Equation.A, Equation.B, Equation.C);
        }

        /// <summary>
        /// Calculates the quadratic regression equation and returns an object representing it.
        /// </summary>
        /// <returns></returns>
        private QuadraticRegressionEquation GenerateEquation()
        {
            int numPoints = _x.Length;
            double sxx = 0,
                    sxy = 0,
                    sxx2 = 0,
                    sx2x2 = 0,
                    sx2y = 0,
                    a = 0,
                    b = 0,
                    c = 0,
                    r = 0,
                    r2 = 0,
                    meanX = _x.Average(),
                    meanY = _y.Average(),
                    meanX2 = _x.Average(s => s * s),
                    s2x = 0,
                    s2y = 0;

            // calculate sums to be used for coefficients
            for (int i = 0; i < numPoints; i++)
            {
                double x2 = _x[i] * _x[i];
                sxx += ((_x[i] - meanX) * (_x[i] - meanX)) / numPoints;
                sxy += ((_x[i] - meanX) * (_y[i] - meanY)) / numPoints;
                sxx2 += ((_x[i] - meanX) * (x2 - meanX2)) / numPoints;
                sx2x2 += ((x2 - meanX2) * (x2 - meanX2)) / numPoints;
                sx2y += ((x2 - meanX2) * (_y[i] - meanY)) / numPoints;
            }

            // calculate coefficients
            b = ((sxy * sx2x2) - (sx2y * sxx2)) / ((sxx * sx2x2) - (sxx2 * sxx2));
            c = ((sx2y * sxx) - (sxy * sxx2)) / ((sxx * sx2x2) - (sxx2 * sxx2));
            a = meanY - (b * meanX) - (c * meanX2);

            // calculate correlation coefficient
            // calculate variance
            double rNumerator = 0;
            double rDenominator = _y.Sum(y => (y - meanY) * (y - meanY));
            for (int i = 0; i < numPoints; i++)
            {
                rNumerator += Math.Pow(_y[i] - (a + b * _x[i] + c * (_x[i] * _x[i])), 2);
                s2x += (_x[i] - meanX) * (_x[i] - meanX);
                s2y += (_y[i] - meanY) * (_y[i] - meanY);
            }
            r = Math.Sqrt(1.0 - (rNumerator / rDenominator));
            s2x /= (_x.Length - 1);
            s2y /= (_y.Length - 1);

            // calculate coefficient of determination
            r2 = r * r;

            // set the resulting equation
            return new QuadraticRegressionEquation
            {
                A = a,
                B = b,
                C = c,
                SXX = sxx,
                SXY = sxy,
                SXX2 = sxx2,
                SX2X2 = sx2x2,
                SX2Y = sx2y,
                R = r,
                R2 = r2,
                MeanX = meanX,
                MeanY = meanY,
                MeanX2 = meanX2,
                S2X = s2x,
                S2Y = s2y
            };
        }

        /// <summary>
        /// Calculates the curve line for the quadratic regression based on a set of x and y points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private Point[] CalculateParabola(double a, double b, double c)
        {
            var points = new Point[_x.Length];
            for (int i = 0; i < _x.Length; i++)
            {
                points[i] = new Point(_x[i], a + (b * _x[i]) + (c * (_x[i] * _x[i])));
            }
            Array.Sort(points, (x, y) => x.X.CompareTo(y.X));
            return points;
        }
    }
}
