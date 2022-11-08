using ShaderTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadedSolids
{
    internal class main
    {
        public static void Main(String[] args)
        {
            Torus program = new Torus();
            program.runAnimation();
        }
    }
}
