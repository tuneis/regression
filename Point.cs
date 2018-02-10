namespace Tuneis
{
    /// <summary>
    /// Point struct.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// X value.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y value.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Almighty point struct constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
