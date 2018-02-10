namespace Tuneis
{
    public class SimpleRegressionEquation
    {
        /// <summary>
        /// Calculated F test. (Not SigF)
        /// </summary>
        public double F { get; set; }

        /// <summary>
        /// Mean of X values.
        /// </summary>
        public double MeanX { get; set; }

        /// <summary>
        /// Mean of Y values.
        /// </summary>
        public double MeanY { get; set; }

        /// <summary>
        /// Count of points used in this regression.
        /// </summary>
        public double Observations { get; set; }

        /// <summary>
        /// Correlation Coefficient.
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// Coefficient of Determination.
        /// </summary>
        public double R2 { get; set; }

        /// <summary>
        /// Adjusted R2.
        /// </summary>
        public double R2Adjusted { get; set; }

        /// <summary>
        /// Slope.
        /// </summary>
        public double Slope { get; set; }

        /// <summary>
        /// X standard deviation.
        /// </summary>
        public double StandardDeviationX { get; set; }

        /// <summary>
        /// Y standard deviation
        /// </summary>
        public double StandardDeviationY { get; set; }

        /// <summary>
        /// Standard error.
        /// </summary>
        public double StandardError { get; set; }

        /// <summary>
        /// Value of T test.
        /// </summary>
        public double T { get; set; }

        /// <summary>
        /// X Variance.
        /// </summary>
        public double VarianceX { get; set; }

        /// <summary>
        /// Y Variance.
        /// </summary>
        public double VarianceY { get; set; }

        /// <summary>
        /// Y Intercept.
        /// </summary>
        public double YIntercept { get; set; }
    }
}
