using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderTest
{
    class Sphere
    {
        float light_X = 1;
        float light_Y = 1;
        float light_Z = -5;
        float light_R = 3;

        float theta_spacing = 0.007F;
        float phi_spacing = 0.002F;

        float K2 = 5;

        int screen_width = 150;
        int screen_height = 50;

        float radius = 3.0F;

        char[] shades = ".'`^\",:;i][{)xuzo8B@".ToCharArray();
        //char[] shades = ".,-~:;=!*#$@".ToCharArray();

        public char[,] RenderFrame()
        {
            float K1 = (screen_width * K2 * 1) / (8 * (radius));

            char[,] printout = new char[screen_width, screen_height];
            Populate<char>(printout, ' ');

            float[,] zBuffer = new float[screen_width, screen_height];
            Populate<float>(zBuffer, 0);

            for (float theta = 0; theta < 2 * Math.PI; theta += theta_spacing)
            {
                for (float phi = 0; phi < Math.PI; phi += phi_spacing)
                {
                    float x = (float) (radius * Math.Sin(phi) * Math.Cos(theta));
                    float y = (float) (radius * Math.Sin(phi) * Math.Sin(theta));
                    float z = K2 + (float) (1 * radius * Math.Cos(phi));
                    float OOZ = 1 / z;

                    int xp = (int) Math.Round((screen_width / 2) + (K1 * OOZ * x));
                    int yp = (int) Math.Round((screen_height / 2) - (K1 * OOZ * y));

                    float Nx = (float) (Math.Sin(phi) * Math.Cos(theta));
                    float Ny = (float) (Math.Sin(phi) * Math.Sin(theta));
                    float Nz = (float)(Math.Cos(phi));

                    float luminence = (Nx * light_X) + (Ny * light_Y) + (Nz * light_Z);

                    if (luminence > 0)
                    {
                        if (OOZ > zBuffer[xp, yp])
                        {
                            zBuffer[xp, yp] = OOZ;

                            int luminence_index = (int) (luminence * FindIndex());

                            printout[xp, yp] = shades[luminence_index];
                        }
                    }
                }
            }
                
            return printout;
        }

        private float FindIndex()
        {
            float magnitude = (float) Math.Sqrt(Math.Pow(light_X, 2) + Math.Pow(light_Y, 2) + Math.Pow(light_Z, 2));

            return shades.Length / magnitude;
        }

        private void Populate<T>(T[,] arr, T value)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    arr[i, x] = value;
                }
            }
        }

        private void PrintOut(char[,] printout)
        {
            for (int j = 0; j < screen_height; j++)
            {
                for (int i = 0; i < screen_width; i++)
                {
                    Console.Write(printout[i, j] + " ");
                }

                Console.Write('\n');
            }
        }

        public void runAnimationCircle()
        {
            while (true)
            {
                for (float theta = 0; theta < 2 * Math.PI; theta += 0.25F)
                {
                    light_X = (float)(light_R * Math.Cos(theta));
                    light_Y = (float)(light_R * Math.Sin(theta));

                    PrintOut(RenderFrame());
                    Thread.Sleep(500);
                    Console.Clear();
                }
            }
        }
        public void runAnimationEightCurve()
        {
            while (true)
            {
                for (float theta = 0; theta < 2 * Math.PI; theta += 0.25F)
                {
                    light_X = (float) (light_R * Math.Sin(theta));
                    light_Y = (float) (light_R * Math.Sin(theta) * Math.Cos(theta));

                    PrintOut(RenderFrame());
                    Thread.Sleep(500);
                    Console.Clear();
                }
            }
        }

        public static void Main(String[] args)
        {
            Sphere program = new Sphere();
            program.runAnimationEightCurve();
        }
    }
}
