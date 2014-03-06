using System;
using Tao.OpenGl;

namespace vtree
{

    public class Util3d
    {
        public static void SolidCylinder(double radius, double height, int slices, int stacks)
        {
            int i,j;

            /* Step in z and radius as stacks are drawn. */

            double z0, z1;
            double zStep = height / ( ( stacks > 0 ) ? stacks : 1 );

            /* Pre-computed circle */

            double[] sint, cost;

            fghCircleTable(out sint, out cost, -slices);

            /* Cover the base and top */

            //Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glNormal3d(0.0, 0.0, -1.0 );
            Gl.glVertex3d(0.0, 0.0,  0.0 );
            
            for (j = 0; j <= slices; j++)
            {                
                Gl.glVertex3d(cost[j] * radius, sint[j] * radius, 0.0);
            }

            Gl.glEnd();

            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glNormal3d(0.0, 0.0, 1.0   );
            Gl.glVertex3d(0.0, 0.0, height);

            for (j = slices; j >= 0; j--)
            {
                Gl.glVertex3d(cost[j] * radius, sint[j] * radius, height);
            }

            Gl.glEnd();

            /* Do the stacks */

            z0 = 0.0;
            z1 = zStep;
            
            for (i = 1; i <= stacks; i++)
            {
                if (i == stacks)
                {
                    z1 = height;
                }

                Gl.glBegin(Gl.GL_QUAD_STRIP);

                for (j = 0; j <= slices; j++ )
                {
                    Gl.glNormal3d(cost[j],          sint[j],          0.0 );
                    Gl.glVertex3d(cost[j] * radius, sint[j] * radius, z0  );
                    Gl.glVertex3d(cost[j] * radius, sint[j] * radius, z1  );
                }
                
                Gl.glEnd();

                z0 = z1; z1 = zStep * i;

                
                
            }
            //Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
        }

        public static void SolidCone(double baseRadius, double height, int slices, int stacks)
        {
            int i,j;

            /* Step in z and radius as stacks are drawn. */

            double z0,z1;
            double r0,r1;

            double zStep = height / ( ( stacks > 0 ) ? stacks : 1 );
            double rStep = baseRadius / ( ( stacks > 0 ) ? stacks : 1 );

            /* Scaling factors for vertex normals */

            double cosn = ( height / Math.Sqrt ( height * height + baseRadius * baseRadius ));
            double sinn = ( baseRadius  / Math.Sqrt ( height * height + baseRadius * baseRadius ));

            /* Pre-computed circle */

            double[] sint, cost;

            fghCircleTable(out sint, out cost, slices);

            /* Cover the circular base with a triangle fan... */

            z0 = 0.0;
            z1 = zStep;

            r0 = baseRadius;
            r1 = r0 - rStep;

            Gl.glBegin(Gl.GL_TRIANGLE_FAN);

            Gl.glNormal3d(0.0, 0.0, -1.0);
            Gl.glVertex3d(0.0, 0.0, z0 );

            for (j = 0; j <= slices; j++)
            {
                Gl.glVertex3d(cost[j] * r0, sint[j] * r0, z0);
            }

            Gl.glEnd();

            /* Cover each stack with a quad strip, except the top stack */

            for( i=0; i < stacks - 1; i++ )
            {
                Gl.glBegin(Gl.GL_QUAD_STRIP);

                for(j = 0; j <= slices; j++)
                {
                    Gl.glNormal3d(cost[j]*sinn, sint[j]*sinn, cosn);
                    Gl.glVertex3d(cost[j]*r0,   sint[j]*r0,   z0  );
                    Gl.glVertex3d(cost[j]*r1,   sint[j]*r1,   z1  );
                }

                z0 = z1; z1 += zStep;
                r0 = r1; r1 -= rStep;

                Gl.glEnd();
            }

            /* The top stack is covered with individual triangles */

            Gl.glBegin(Gl.GL_TRIANGLES);

            Gl.glNormal3d(cost[0]*sinn, sint[0]*sinn, cosn);

            for (j=0; j<slices; j++)
            {
                Gl.glVertex3d(cost[j+0]*r0,   sint[j+0]*r0,   z0    );
                Gl.glVertex3d(0,              0,              height);
                Gl.glNormal3d(cost[j+1]*sinn, sint[j+1]*sinn, cosn  );
                Gl.glVertex3d(cost[j+1]*r0,   sint[j+1]*r0,   z0    );
            }

            Gl.glEnd();
        }

        public static void fghCircleTable(out double[] sint, out double[] cost, int n)
        {
            int i;

            /* Table size, the sign of n flips the circle direction */

            int size = Math.Abs(n);

            /* Determine the angle between samples */

            double angle = 2 * Math.PI / (double)( ( n == 0 ) ? 1 : n );

            /* Allocate memory for n samples, plus duplicate of first entry at the end */

            sint = new double[size + 1];
            cost = new double[size + 1];

            /* Compute cos and sin around the circle */

            sint[0] = 0.0;
            cost[0] = 1.0;

            for (i=1; i < size; i++)
            {
                sint[i] = Math.Sin(angle * i);
                cost[i] = Math.Cos(angle * i);
            }

            /* Last sample is duplicate of the first */

            sint[size] = sint[0];
            cost[size] = cost[0];
        }

    }

}
