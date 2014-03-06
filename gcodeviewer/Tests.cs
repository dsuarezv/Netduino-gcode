using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gcodeparser
{
    public class ArcData
    {
        public CPointF Start;
        public CPointF End;
        public float Radius;

        public ArcData(float x1, float y1, float x2, float y2, float radius)
        {
            Start.X = x1;
            Start.Y = y1;
            End.X = x2;
            End.Y = y2;
            Radius = radius;
        }
    }


    public class Tests
    {
        ArcData[] ArcTestData = new ArcData[] {
            new ArcData(30.527f, 37.758f, 32.671f, 34.965f, 13.5f)
        };

        private void Test2()
        {
            foreach (ArcData d in ArcTestData)
            {
                ArcInterpolation arc = new ArcInterpolation(d.Start, d.End, d.Radius, false);
            }

            
        }

        private void Test()
        {
            Device d = DeviceFactory.Get();

            d.MoveAbsoluteLinear(100, 100, 0);
            d.MoveAbsoluteLinear(100, 100, -7);

            //d.MoveRelativeLinear(50, 0, 0);
            //d.MoveRelativeLinear(0, 50, 0);
            //d.MoveRelativeLinear(-50, 0, 0);
            //d.MoveRelativeLinear(0, -50, 0);

            //d.MoveRelativeLinear(0, 0, 7);
            //d.MoveAbsoluteLinear(80, 200, 0);

            //d.MoveRelativeLinear(0, 0, -7);

            //d.MoveRelativeLinear(50, 0, 0);
            //d.MoveRelativeLinear(0, 50, 0);
            //d.MoveRelativeLinear(-50, 0, 0);
            //d.MoveRelativeLinear(0, -50, 0);
        }


    }
}
