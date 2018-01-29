using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveProject {
    class ComplexNumber {

        public void SetLength() {
            this.Length = Math.Sqrt(Math.Pow(Real, 2) + Math.Pow(Imaginary, 2));
        }

        public void SetPhase() {
            this.Phase = Math.Atan(this.Real / this.Imaginary);
        }

        public double Real { get; set; }
        public double Imaginary { get; set; }
        public double Length { get; set; }
        public double Phase { get; set; }
    }
}
