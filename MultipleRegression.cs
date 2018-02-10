using System;
using System.Collections.Generic;
using System.Linq;

namespace Tuneis
{
    public class MultipleRegression
    {
        /// <summary>
        /// Multiple X values (Independent variables).
        /// </summary>
        List<double[]> _xn;

        /// <summary>
        /// Y values (Dependent variables)
        /// </summary>
        double[] _y;

        /// <summary>
        /// X values.
        /// </summary>
        public List<double[]> XN { get; set; }

        /// <summary>
        /// Y values.
        /// </summary>
        public double[] Y { get; set; }

        /// <summary>
        /// The equation for Multiple Regression
        /// </summary>
        public MultipleRegressionEquation Equation { get; set; }

        /// <summary>
        /// Line of best fit points.
        /// </summary>
        public Point[] LineOfBestFit { get; set; }

        public MultipleRegression(List<double[]> xn, double[] y)
        {
            if (xn.Count > 2)
            {
                throw new Exception("Unable to do multi regression with more than 2 independent variables at this time.");
            }
            _xn = XN = xn;
            _y = Y = y;
            Equation = Calculate();
        }

        /// <summary>
        /// Used for creating a list of index numbers to do Linear Regression on a dynamic amount of independent variables from the XN values.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="subset"></param>
        /// <returns></returns>
        private int[] GetSubset(int[] input, int[] subset)
        {
            int[] result = new int[subset.Length];
            for (int i = 0; i < subset.Length; i++)
                result[i] = input[subset[i]];
            return result;
        }

        /// <summary>
        /// Creates an array of indice combinations for use with doing linear regressions on the lists contained in XN.
        /// </summary>
        /// <param name="sequenceLength">Pairs of 2, 3, 4... etc</param>
        /// <returns></returns>
        private List<int[]> CreateCombinations(int sequenceLength = 2)
        {
            // store an array of indices pointing the the arrays of independent variables in the xn array
            int[] indices = new int[_xn.Count];
            for (int i = 0; i < _xn.Count; i++)
            {
                indices[i] = i;
            }

            // sequence length for combinations
            int k = sequenceLength;

            // array to store indice subsets of variables to do linear regressions against
            List<int[]> subsets = new List<int[]>();

            // here we'll keep indices
            // pointing to elements in input array
            int[] s = new int[k];

            if (k <= indices.Length)
            {
                // first index sequence: 0, 1, 2, ...
                for (int i = 0; (s[i] = i) < k - 1; i++) ;

                // add first subset of indices
                subsets.Add(GetSubset(indices, s));

                // add remaining subset of indices
                for (; ; )
                {
                    int i;
                    // find position of item that can be incremented
                    for (i = k - 1; i >= 0 && s[i] == indices.Length - k + i; i--) ;
                    if (i < 0)
                    {
                        break;
                    }
                    // increment this item
                    s[i]++;
                    for (++i; i < k; i++)
                    {    // fill up remaining items
                        s[i] = s[i - 1] + 1;
                    }
                    subsets.Add(GetSubset(indices, s));
                }
            }
            return subsets;
        }

        // http://www.biddle.com/documents/bcg_comp_chapter4.pdf
        /// <summary>
        /// Calculates the multi regression for a 2 independent variable regression.
        /// </summary>
        /// <returns></returns>
        public MultipleRegressionEquation Calculate()
        {
            // list for storing all linear regressions performed
            List<SimpleRegression> dependents = new List<SimpleRegression>();
            List<SimpleRegression> independents = new List<SimpleRegression>();

            // dependent(y) vs independent (x)
            for (int i = 0; i < _xn.Count; i++)
            {
                dependents.Add(new SimpleRegression(_xn[i], _y));
            }

            // array to store indice subsets of variables to do linear regressions against
            List<int[]> independentSubsets = CreateCombinations();

            // independent (x) vs independent (x)
            for (int i = 0; i < independentSubsets.Count; i++)
            {
                var x1 = _xn[independentSubsets[i][0]];
                var x2 = _xn[independentSubsets[i][1]];
                independents.Add(new SimpleRegression(x1, x2));
            }

            // calculate R
            double R = CalculateR(independents, dependents);

            // calculate b values for each correlation
            List<double> b = CalculateBList(independents, dependents);

            // calculate A
            double a = _y.Average();
            for (int i = 0; i < _xn.Count; i++)
            {
                a -= b[i] * _xn[i].Average();
            }

            return new MultipleRegressionEquation
            {
                A = a,
                B = b,
                R = R
            };
        }

        /// <summary>
        /// Calculates the R value for this regression from the independent and dependent variables.
        /// </summary>
        /// <param name="independents"></param>
        /// <param name="dependents"></param>
        /// <returns></returns>
        double CalculateR(List<SimpleRegression> independents, List<SimpleRegression> dependents)
        {
            // square then sum dependent vs independent r values
            // add r values from all arrays to be multiplied by together
            double leftSideValue = 0;
            double rightSideValue = 2;
            foreach (var lr in dependents)
            {
                leftSideValue += Math.Pow(lr.Equation.R, 2);
                rightSideValue *= lr.Equation.R;
            }

            double denominator = 0;
            foreach (var lr in independents)
            {
                rightSideValue *= lr.Equation.R;
                denominator += Math.Pow(lr.Equation.R, 2);
            }

            double numerator = leftSideValue - rightSideValue;
            denominator = 1 - denominator;

            return Math.Sqrt(numerator / denominator);
        }

        /// <summary>
        /// Calculates the B values for the regression equation from the independent and dependent lists.
        /// </summary>
        /// <param name="independents"></param>
        /// <param name="dependents"></param>
        /// <returns></returns>
        List<double> CalculateBList(List<SimpleRegression> independents, List<SimpleRegression> dependents)
        {
            double[] ryx = new double[dependents.Count];
            double[] ryxrxx = new double[dependents.Count];
            for (int i = 0; i < dependents.Count; i++)
            {
                // set to 1 to avoid multiplying by 0
                ryxrxx[i] = 1;

                // set left side of equation for b
                ryx[i] = dependents[i].Equation.R;

                // multiply all the independents
                for (int j = 0; j < independents.Count; j++)
                {
                    ryxrxx[i] *= independents[j].Equation.R;
                }
            }

            // loop through dependents in reverse to include in the ryxrxx calculations
            int index = 0;
            for (int i = dependents.Count - 1; i >= 0; i--)
            {
                ryxrxx[index++] *= dependents[i].Equation.R;
            }

            // calculate shared denominator for the equation
            double denominator = 0;
            foreach (var lr in independents)
            {
                denominator += Math.Pow(lr.Equation.R, 2);
            }
            denominator = 1 - denominator;

            // calculate b values for each correlation
            List<double> b = new List<double>();
            for (int i = 0; i < dependents.Count; i++)
            {
                b.Add(((ryx[i] - ryxrxx[i]) / denominator) * (dependents[i].Equation.StandardDeviationY / dependents[i].Equation.StandardDeviationX));
            }
            return b;
        }

        ///// <summary>
        ///// Calculates the line for the linear multiple regression based on a set of x and y points.
        ///// </summary>
        ///// <param name="m">Slope</param>
        ///// <param name="b">Y Intercepts</param>
        ///// <returns></returns>
        //private List<Point[]> CalculateLineOfBestFit(double a, double[] b)
        //{
        //    var points = new Point[_x.Length];
        //    for (int i = 0; i < _x.Length; i++)
        //    {
        //        points[i] = new Point(_x[i], m * _x[i] + b);
        //    }
        //    Array.Sort(points, (x, y) => x.X.CompareTo(y.X));
        //    return points;
        //}
    }
}
