using System;


namespace gcodeparser
{
    internal class CommandD: BaseCommand
    {
        public override void Parse()
        {
            int gType = GCodeParser.ParseInt();

            switch (gType)
            {
                case -1: Logger.Log("D: no command"); return;

                case 1: Parse1(); return;
                case 10: Parse10(); return;
                case 11: Parse11(); return;
                case 21: Parse21(); return;
                case 92: Parse92(); return;

                default:
                    Logger.Log("D: Unsupported: {0}", gType);
                    break;
            }
        }

        private void Parse1()
        {
            Logger.Log("D1: Reset axis to 0, 0 on current location");

            DeviceFactory.Get().ResetAxis();
        }

        private void Parse10()
        {
            Logger.Log("D10: move axis n steps");

            char axis = GCodeParser.ReadChar();
            int steps = GCodeParser.ParseInt();

            if (steps == -1) return;

            switch (axis)
            { 
                case 'x': 
                case 'X':
                    DeviceFactory.Get().MoveRawX(steps);
                    break;
                case 'y':
                case 'Y':
                    DeviceFactory.Get().MoveRawY(steps);
                    break;
                case 'z':
                case 'Z':
                    DeviceFactory.Get().MoveRawZ(steps);
                    break;
            }
        }

        private void Parse11()
        {
            //Logger.Log("D11: move y axis n steps");
        }

        

        // __ Manual mode _____________________________________________________


        private void Parse21()
        {
            //Logger.Log("D21: Enter MANUAL mode");
            Machine.SetState(MachineState.D21_ManualMode);
        }


        // __ Calibration _____________________________________________________


        private void Parse92()
        {
            // Calibrate axis
            Machine.SetState(MachineState.D92_Calibration);
        }
    }
}
