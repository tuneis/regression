namespace Tuneis
{
    /// <summary>
    /// y = ax^2 + bx + c
    /// </summary>
    public class QuadraticRegressionEquation
    {
        /// <summary>
        /// Quadratic formula coefficient A.
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// Quadratic formula coefficient B.
        /// </summary>
        public double B { get; set; }

        /// <summary>
        /// Quadratic formula coefficient C.
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// Mean of X values.
        /// </summary>
        public double MeanX { get; set; }

        /// <summary>
        /// Mean of X values squared.
        /// </summary>
        public double MeanX2 { get; set; }

        /// <summary>
        /// Mean of Y values.
        /// </summary>
        public double MeanY { get; set; }

        /// <summary>
        /// Sum of XX.
        /// </summary>
        public double SXX { get; set; }

        /// <summary>
        /// Sum of XY.
        /// </summary>
        public double SXY { get; set; }

        /// <summary>
        /// Sum of XX2.
        /// </summary>
        public double SXX2 { get; set; }

        /// <summary>
        /// Sum of X2X2.
        /// </summary>
        public double SX2X2 { get; set; }

        /// <summary>
        /// Sum of X2Y.
        /// </summary>
        public double SX2Y { get; set; }

        /// <summary>
        /// Correlation coefficient.
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// Coefficient of determination.
        /// </summary>
        public double R2 { get; set; }

        /// <summary>
        /// X variance.
        /// </summary>
        public double S2X { get; set; }

        /// <summary>
        /// Y variance.
        /// </summary>
        public double S2Y { get; set; }
    }
}
