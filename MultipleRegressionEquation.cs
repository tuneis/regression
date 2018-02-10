using System;
using System.Collections.Generic;
using System.Text;

namespace Tuneis
{
    /// <summary>
    /// Y' = a + b1X1 + b2X2 ... bnXn
    /// </summary>
    public class MultipleRegressionEquation
    {
        /// <summary>
        /// 
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<double> B { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double R { get; set; }
    }
}
