using System;
using System.Collections.Generic;

namespace SkyView.Image {

    public class Noise {

        struct Vector {
            public double X;
            public double Y;
            public double Z;

            public Vector(double x, double y, double z) : this() {
                X = x;
                Y = y;
                Z = z;
            }

            public static Vector operator +(Vector p, Vector q) {
                return new Vector( p.X + q.X, p.Y + q.Y, p.Z + q.Z );
            }
            public static Vector operator -(Vector p, Vector q) {
                return new Vector(p.X - q.X, p.Y - q.Y, p.Z - q.Z);
            }
            public static double operator *(Vector p, Vector q) {
                return p.X * q.X + p.Y * q.Y + p.Z * q.Z;
            }
        }

        private uint octave = 1;
        private double persistence = 1;
        private int seed = 0;

        private double XOffset = 0;
        private double YOffset = 0;
        private double Depth = 0;

        private uint tileSize = 256;
        private double unit = 16;

        public Noise() {
        }

        private readonly byte[] initTab = { 151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };
        private int[] permutationsTable = new int[512];

        private readonly Vector[] Gradients = new Vector[16] { 
           new Vector( 1, 1, 0 ), new Vector( -1, 1, 0 ), new Vector( 1, -1, 0 ), new Vector( -1, -1, 0 ) ,
           new Vector( 1, 0, 1 ), new Vector( -1, 0, 1 ), new Vector( 1, 0, -1 ), new Vector( -1, 0, -1 ) ,
           new Vector( 0, 1, 1 ), new Vector( 0, -1, 1 ), new Vector( 0, 1, -1 ), new Vector( 0, -1, -1 ) ,
           new Vector( 1, 1, 0 ), new Vector( -1, 1, 0 ), new Vector( 0, -1, 1 ), new Vector( 0, -1,  1 )
        };

        public void SetParameters(uint octave, double persistence, int seed) {
            if (octave == 0)
                throw new Exception("Octave must be different than zero");
            this.octave = octave;
            this.persistence = persistence;

            // Table initiale si la seed vaut 0
            if (seed == 0 && tileSize == 256) {
                permutationsTable = new int[512];
                for (int i = 0; i < 512; i++)
                    permutationsTable[i] = initTab[i & 255];
            } else {  // Sinon rebrassage de la table
                this.seed = seed;
                Random r = new Random(seed);
                List<int> initNumbers = new List<int>();
                permutationsTable = new int[tileSize * 2];
                for (int i = 0; i < tileSize; i++)
                    initNumbers.Add(i);
                for (int i = 0; i < tileSize; i++) {
                    permutationsTable[i] = initNumbers[r.Next((int)tileSize - i)];
                    permutationsTable[i + tileSize] = permutationsTable[i];
                    initNumbers.Remove(permutationsTable[i]);
                }
            }
        }

        public void SetAdvanced(uint tileSize, double unit) {
            if (tileSize <= 0)
                throw new Exception("The tile size must be different than zero");
            this.tileSize = tileSize;
            if (unit == 0)
                throw new Exception("The unit must be different than zero");
            this.unit = unit;
        }

        public void SetOffset(double xOffset, double yOffset, double depth) {
            XOffset = xOffset;
            YOffset = yOffset;
            Depth = depth;
        }

        public double GetNoise(int x, int y) {
            return GetNoise( x / unit, y / unit);
        }

        public double GetNoise(double x, double y) {

            double sum = 0;
            double amplitude = 1;
            double frequency = 1;
            for (int o = 0; o < octave; o++) {

                /*                           
                 *            C2    D2  |            k2    l2
                 *            +-----+   |            +-----+
                 *            |     |   |            |     |
                 *   C     D  |  .  |   |   k     l  |  .  |
                 *   +-----+  |     |   |   +-----+  |     |
                 *   |   P |  +-----+   |   |   P |  +-----+
                 *   |  x  |  A2   B2   |   |  x  |  i2   j2
                 *   |     |            |   |     |   
                 *   +-----+            |   +-----+
                 *   A     B            |   i     j
                 *                        
                 *  Toutes les coordonnées sont remises dans la plage de coordonnées définies par tileSize
                 */

                // Obtention du point A
                Vector A = new Vector((int)(Math.Floor(x * frequency + XOffset) % tileSize), (int)(Math.Floor(y * frequency + YOffset) % tileSize), (int)(Math.Floor(Depth * frequency) % tileSize));
                Vector P = new Vector((x * frequency + XOffset) % tileSize, (y * frequency + YOffset) % tileSize, (Depth * frequency) % tileSize);
                Vector AP = P - A;

                //Obtention des gradients

                int indexGradientA  = permutationsTable[ (int)A.X +     permutationsTable[ (int)A.Y +     permutationsTable[ (int)A.Z     ]]] % 16;
                int indexGradientA2 = permutationsTable[ (int)A.X +     permutationsTable[ (int)A.Y +     permutationsTable[ (int)A.Z + 1 ]]] % 16;
                int indexGradientB  = permutationsTable[ (int)A.X + 1 + permutationsTable[ (int)A.Y +     permutationsTable[ (int)A.Z     ]]] % 16;
                int indexGradientB2 = permutationsTable[ (int)A.X + 1 + permutationsTable[ (int)A.Y +     permutationsTable[ (int)A.Z + 1 ]]] % 16;
                int indexGradientC  = permutationsTable[ (int)A.X +     permutationsTable[ (int)A.Y + 1 + permutationsTable[ (int)A.Z     ]]] % 16;
                int indexGradientC2 = permutationsTable[ (int)A.X +     permutationsTable[ (int)A.Y + 1 + permutationsTable[ (int)A.Z + 1 ]]] % 16;
                int indexGradientD  = permutationsTable[ (int)A.X + 1 + permutationsTable[ (int)A.Y + 1 + permutationsTable[ (int)A.Z     ]]] % 16;
                int indexGradientD2 = permutationsTable[ (int)A.X + 1 + permutationsTable[ (int)A.Y + 1 + permutationsTable[ (int)A.Z + 1 ]]] % 16;

                Vector gradientA  = Gradients[indexGradientA];
                Vector gradientA2 = Gradients[indexGradientA2];
                Vector gradientB  = Gradients[indexGradientB];
                Vector gradientB2 = Gradients[indexGradientB2];
                Vector gradientC  = Gradients[indexGradientC];
                Vector gradientC2 = Gradients[indexGradientC2];
                Vector gradientD  = Gradients[indexGradientD];
                Vector gradientD2 = Gradients[indexGradientD2];

                // Obtention des poids de chaque gradient pour chaque point

                double i  = gradientA  * (AP - new Vector(0, 0, 0));
                double i2 = gradientA2 * (AP - new Vector(0, 0, 1));
                double j  = gradientB  * (AP - new Vector(1, 0, 0));
                double j2 = gradientB2 * (AP - new Vector(1, 0, 1));
                double k  = gradientC  * (AP - new Vector(0, 1, 0));
                double k2 = gradientC2 * (AP - new Vector(0, 1, 1));
                double l  = gradientD  * (AP - new Vector(1, 1, 0));
                double l2 = gradientD2 * (AP - new Vector(1, 1, 1));

                //
                // Interpolation en utilisant la fonction 6t^5 - 15t^4 + 10t^3
                //            
                double xCoeff = Interpolate(AP.X);
                double yCoeff = Interpolate(AP.Y);
                double zCoeff = Interpolate(AP.Z);

                // Interpolation sur X
                double Inter_i_j   = i + xCoeff * (j - i);
                double Inter_i2_j2 = i2 + xCoeff * (j2 - i2);
                double Inter_k_l   = k + xCoeff * (l - k);
                double Inter_k2_l2 = k2 + xCoeff * (l2 - k2);

                // Interpolation sur Y
                double Inter_ij_kl     = Inter_i_j + yCoeff * (Inter_k_l - Inter_i_j);
                double Inter_i2j2_k2l2 = Inter_i2_j2 + yCoeff * (Inter_k2_l2 - Inter_i2_j2);

                // Interpolation sur Z
                double Inter_ijkl_i2j2k2l2 = Inter_ij_kl + zCoeff * (Inter_i2j2_k2l2 - Inter_ij_kl);

                sum += amplitude * (Inter_ijkl_i2j2k2l2 + 1) * 0.5;

                amplitude *= persistence;
                frequency *= 2;
            }

            if (persistence == 1)
                return sum / octave;
            else return sum * (1 - persistence) / (1 - amplitude);
        }

        internal double Interpolate(double t) {
            return t * t * t * ( t * ( 6 * t - 15 ) + 10 ) ;
        }

    }

}
