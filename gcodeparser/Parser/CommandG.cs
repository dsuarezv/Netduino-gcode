using System;


namespace gcodeparser
{
    internal class CommandG: BaseCommand
    {
        public override void Parse()
        {
            int gType = GCodeParser.ParseInt();

            switch (gType)
            {
                case -1: Logger.Log("G: no command"); return;

                case 90: Parse90(); break;
                case 91: Parse91(); break;
                case 92: Parse92(); break;

                case 40: Parse40(); break;
                case 17: Parse17(); break;
                case 18: Parse18(); break;
                case 19: Parse19(); break;
                case 0:  Parse0(); break;
                case 1:  Parse1(); break;
                case 2:  Parse2(); break;
                case 3:  Parse3(); break;

                default:
                    Logger.Log("G: Unsupported: {0}", gType);
                    break;
            }
        }

        private void Parse0()
        {
            Logger.Log("G00: Set Rapid Linear Motion mode.");

            Machine.SetState(MachineState.G0_RapidMove);
        }

        private void Parse1()
        {
            Logger.Log("G01: Set linear interpolation mode.");

            Machine.SetState(MachineState.G1_LinearMove);
        }

        private void Parse2()
        {
            Logger.Log("G02: Set circular/helical interpolation (clockwise) mode.");

            Machine.SetState(MachineState.G2_ArcMove);
        }

        private void Parse3()
        {
            Logger.Log("G03: Set circular/helical interpolation (counter clockwise) mode.");

            Machine.SetState(MachineState.G3_ArcMoveCCW);
        }

        private void Parse17()
        {
            Logger.Log("G17: Select XY plane");

            Machine.SetPlane(MachinePlane.XY);
        }

        private void Parse18()
        {
            Logger.Log("G18: Select XZ plane");

            Machine.SetPlane(MachinePlane.XZ);
        }

        private void Parse19()
        {
            Logger.Log("G19: Select YZ plane");

            Machine.SetPlane(MachinePlane.YZ);
        }

        private void Parse40()
        {
            Logger.Error("G40: Cancel cutter radius compensation");
        }

        private void Parse90()
        {
            Logger.Log("G90: Set absolute distance mode");

            Machine.SetMode(DistanceMode.Absolute);
        }

        private void Parse91()
        {
            Logger.Log("G91: Set incremental distance mode");

            Machine.SetMode(DistanceMode.Relative);
        }

        private void Parse92()
        {
            Logger.Log("G92: Set Position");

            // Allows programming of absolute zero point, by reseting the 
            // current position to the values specified. This would set the 
            // machine's X coordinate to 10, and the extrude coordinate to 90.
            // No physical motion will occur.

            Machine.SetState(MachineState.G92_SetPosition);
        }

    }
}
