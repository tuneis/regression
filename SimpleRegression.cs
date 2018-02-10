using System;
using System.Linq;

namespace Tuneis
{
    public class SimpleRegression
    {
        /// <summary>
        /// X values.
        /// </summary>
        double[] _x;

        /// <summary>
        /// Y values.
        /// </summary>
        double[] _y;

        /// <summary>
        /// X values.
        /// </summary>
        public double[] X { get; set; }

        /// <summary>
        /// Y values.
        /// </summary>
        public double[] Y { get; set; }

        /// <summary>
        /// Equation that best represents the data.
        /// </summary>
        public SimpleRegressionEquation Equation { get; set; }

        /// <summary>
        /// Line of best fit points.
        /// </summary>
        public Point[] LineOfBestFit { get; set; }

        /// <summary>
        /// Pass X and Y arrays and the equation and line of best fit points will be generated for you.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SimpleRegression(double[] x, double[] y)
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
            LineOfBestFit = CalculateLineOfBestFit(Equation.Slope, Equation.YIntercept);
        }

        /// <summary>
        /// Generates the regression equation for the model.
        /// </summary>
        /// <returns></returns>
        private SimpleRegressionEquation GenerateEquation()
        {
            double meanX = _x.Average(),
                meanY = _y.Average(),
                sumX = _x.Sum(),
                sumY = _y.Sum(),
                sumX2 = _x.Sum(x => Math.Pow(x, 2)),
                sumY2 = _y.Sum(y => Math.Pow(y, 2)),
                sumXY = 0,
                r = 0, // Multiple R in Excel Regression Statistics
                n = _x.Length, // Observations in Excel Regression Statistics
                r2a = 0, // Adjusted R Square in Excel Regression Statistics
                m = 0, // Coefficients X Variable 1 in Excel
                b = 0, // Coefficients Intercept in Excel
                r2 = 0, // R Square in Excel Regression Statistics
                f = 0, // Regression F in Excel ANOVA
                oy = 0,
                ox = 0,
                o2y = 0,
                o2x = 0,
                gm = 0,
                t = 0, // t Stat for X Variable 1 in Excel
                se = 0, // Standard Error in Excel Regression Statistics
                ssr = 0, // regression sum of squares - Regression SS in Excel ANOVA
                sse = 0, // error sum of squares - Residual SS in Excel ANOVA
                sstoY = 0, // total sum of squares y - Total SS in Excel ANOVA
                sstoX = 0, // total sum of squares x
                nr = 0, // r numerator
                dr = 0, // r denominator
                ssrDf = 1, // degrees of freedom associated with ssr - Regression Df in Excel ANOVA
                sseDf = n - 2, // degrees of freedom associated with sse - Residual Df in Excel ANOVA
                sstoDf = n - 1, // degrees of freedom associated with ssto - Total Df in Excel ANOVA
                mse = 0, // mean squared error - Residual MS in Excel ANOVA
                msr = 0; // regression mean square - Regression MS in Excel ANOVA

            for (int i = 0; i < n; i++)
            {
                // sum of x * y
                sumXY += _x[i] * _y[i];
            }

            // calculate y intercept
            b = ((sumY * sumX2) - (sumX * sumXY)) / ((n * sumX2) - Math.Pow(sumX, 2));

            // calculate slope
            m = ((n * sumXY) - (sumX * sumY)) / ((n * sumX2) - Math.Pow(sumX, 2));

            for (int i = 0; i < n; i++)
            {
                // predicted y
                double yHat = b + m * _x[i];

                // total sum of squares
                sstoY += Math.Pow(_y[i] - meanY, 2);
                sstoX += Math.Pow(_x[i] - meanX, 2);

                // regression sum of squares
                ssr += Math.Pow(yHat - meanY, 2);

                // error sum of squares
                sse += Math.Pow(_y[i] - yHat, 2);

                // r numerator
                nr += (_x[i] - meanX) * (_y[i] - meanY);

                // grand mean
                gm += (_x[i] + _y[i]) / (n + n);
            }

            // calculate mean squared error
            mse = sse / sseDf;

            // calculate regression mean square
            msr = ssr / ssrDf;

            // standard error
            se = Math.Pow(mse, 0.5);

            // calculate regression f statistic
            f = msr / mse;

            // calculate variance
            o2x = sstoX / (n - 1);
            o2y = sstoY / (n - 1);

            // calculate standard deviation
            ox = Math.Sqrt(o2x);
            oy = Math.Sqrt(o2y);

            // calculate denominator for r
            dr = ox * oy;

            // calculate correlation coefficient r
            r = 1.0 / (n - 1) * (nr / dr);

            // calculate R squared
            r2 = Math.Pow(r, 2);

            // calculate adjusted R squared
            // K is the number of independent regressors, i.e. the number of variables in your model, excluding the constant.
            double k = 1;
            r2a = 1 - (((1 - r2) * (n - 1)) / (n - k - 1));

            // calculate t value
            t = (r * Math.Sqrt(n - 2)) / Math.Sqrt(1 - Math.Pow(r, 2));

            //// calculate standard error for x and y
            //seX = ox / Math.Sqrt(n);
            //seY = oy / Math.Sqrt(n);

            //// calculate slope
            //m = r * oy / ox;

            //// calculate y intercept
            //b = meanY - (m * meanX);

            // calculate t value
            t = (r * Math.Sqrt(n - 2)) / Math.Sqrt(1 - Math.Pow(r, 2));

            //var tee = CalculateT(_x, _y, o2x, o2y);
            //double testt = b / se;
            //double testm = m / se;
            //// https://www.khanacademy.org/math/statistics-probability/analysis-of-variance-anova-library/analysis-of-variance-anova/v/anova-2-calculating-ssw-and-ssb-total-sum-of-squares-within-and-between-avi
            //// calculate total sum of squares
            //// calculate sum of squares within
            //// calculate sum of squares between
            //double sst = 0;
            //double ssw = 0;
            //double ssb = 0;
            //for (int i = 0; i < n; i++)
            //{
            //    sst += Math.Pow(_x[i] - gm, 2) + Math.Pow(_y[i] - gm, 2);
            //    ssw += Math.Pow(_x[i] - meanX, 2) + Math.Pow(_y[i] - meanY, 2);
            //    ssb += Math.Pow(meanX - gm, 2) + Math.Pow(meanY - gm, 2);
            //}

            //// mg = 2 for 2 groups x array and y array
            //// ng = number of items in each group
            //double mg = 2,
            //       ng = n;

            //// calculate total degrees of freedom
            //// calculate degrees of freedom within
            //// calculate degrees of freedom between
            //double df = mg * ng - 1,
            //       dfw = mg * (ng - 1),
            //       dfb = mg - 1;

            // calculate f statistic
            //f = o2x > o2y ? o2x / o2y : o2y / o2x; // (ssb / dfb) / (ssw / dfw);

            // return the equation
            return new SimpleRegressionEquation
            {
                F = f,
                MeanX = meanX,
                MeanY = meanY,
                Observations = n,
                R = r,
                R2 = r2,
                R2Adjusted = r2a,
                Slope = m,
                StandardDeviationX = ox,
                StandardDeviationY = oy,
                StandardError = se,
                T = t,
                VarianceX = o2x,
                VarianceY = o2y,
                YIntercept = b,
            };
        }

        /// <summary>
        /// Calculates the line for the linear regression based on a set of x and y points.
        /// </summary>
        /// <param name="m">Slope</param>
        /// <param name="b">Y Intercept</param>
        /// <returns></returns>
        private Point[] CalculateLineOfBestFit(double m, double b)
        {
            var points = new Point[_x.Length];
            for (int i = 0; i < _x.Length; i++)
            {
                points[i] = new Point(_x[i], m * _x[i] + b);
            }
            Array.Sort(points, (x, y) => x.X.CompareTo(y.X));
            return points;
        }

        /// <summary>
        /// Calculates T statistic and degrees of freedom for this t statistics. Returns a tuple, Item1 = t, Item 2 = df.
        /// </summary>
        /// <param name="x1">Group 1 Independent Variables</param>
        /// <param name="x2">Group 2 Independent Variables</param>
        /// <param name="o2X1">Variance of Group 1</param>
        /// <param name="o2X2">Variance of Group 2</param>
        public Tuple<double, double> CalculateT(double[] x1, double[] x2, double o2X1, double o2X2)
        {
            double meanX1 = x1.Average(),
                   meanX2 = x2.Average(),
                   n1 = x1.Length,
                   n2 = x2.Length;

            // determine t statistic
            double t = (meanX1 - meanX2) / Math.Sqrt(((((n1 - 1) * Math.Pow(o2X1, 2)) + ((n2 - 1) * Math.Pow(o2X2, 2))) / (n1 + n2 - 2)) * ((n1 + n2) / (n1 * n2)));

            // determine degrees of freedom for this t statistic
            double df = n1 + n2 - 2;
            return new Tuple<double, double>(t, df);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <returns></returns>
        public double CalculateF(double[] x1, double[] x2, double[] x3)
        {
            double n1 = x1.Length,
                n2 = x2.Length,
                n3 = x3.Length,
                sumX1 = x1.Sum(),
                sumX2 = x2.Sum(),
                sumX3 = x3.Sum(),
                meanX1 = x1.Average(),
                meanX2 = x2.Average(),
                meanX3 = x3.Average(),
                sx21 = x1.Sum(x => Math.Pow(x, 2)),
                sx22 = x2.Sum(x => Math.Pow(x, 2)),
                sx23 = x3.Sum(x => Math.Pow(x, 2)),
                sx21n = Math.Pow(sumX1, 2) / n1,
                sx22n = Math.Pow(sumX2, 2) / n2,
                sx23n = Math.Pow(sumX3, 2) / n3,
                N = n1 + n2 + n3,
                ssx = sumX1 + sumX2 + sumX3,
                ssx2N = Math.Pow(ssx, 2) / N,
                ssx2 = sx21 + sx22 + sx23,
                ssx2n = sx21n + sx22n + sx23n;

            // sum of squares between, within, and total
            double bss = ssx2n - ssx2N,
                   wss = ssx2 - ssx2n,
                   tss = ssx2 - ssx2N;

            // number of groups (arrays)
            double k = 3;

            // degrees of freedom between and within
            double bdf = k - 1,
                   wdf = N - k;

            // mean sum of squares between and within
            double bmss = bss / bdf,
                   wmss = wss / wdf;

            // calculate f ratio
            double f = bmss / wmss;

            return 0;
        }
    }
}
