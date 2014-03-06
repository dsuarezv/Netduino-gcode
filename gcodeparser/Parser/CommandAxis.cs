using System;


namespace gcodeparser
{
    internal class CommandAxis: BaseCommand
    {
        public float ToX = float.MinValue;
        public float ToY = float.MinValue;
        public float ToZ = float.MinValue;
        public float ToI = float.MinValue;
        public float ToJ = float.MinValue;
        public float ToK = float.MinValue;
        public float ToR = float.MinValue;

        private char InitialAxis;

        public CommandAxis(char axis)
        {
            InitialAxis = axis;
        }

        public override void Parse()
        {
            AddAxis(InitialAxis);

            //Logger.Log("Move to {0},{1},{2} - {3},{4},{5}, R{6}", ToX, ToY, ToZ, ToI, ToJ, ToK, ToR);

            Machine.Move(ToX, ToY, ToZ, ToI, ToJ, ToK, ToR);
        }

        private void AddAxis(char c)
        {
            float distance = (float)GCodeParser.ParseDouble();

            if (distance == float.MinValue)
            {
                Logger.Error("ERROR, axis definition without real value");
                return;
            } 

            switch (c)
            {
                case 'X': ToX = distance; break;
                case 'Y': ToY = distance; break;
                case 'Z': ToZ = distance; break;
                case 'I': ToI = distance; break;
                case 'J': ToJ = distance; break;
                case 'K': ToK = distance; break;
                case 'R': ToR = distance; break;
                default:
                    Logger.Error("ERROR: unknown axis while parsing axes: {0}", c);
                    break;
            }

            char next = GCodeParser.PeekChar();

            switch (next)
            {
                case 'X': 
                case 'Y': 
                case 'Z':
                case 'I':
                case 'J':
                case 'K':
                case 'R':
                    GCodeParser.ReadChar();
                    AddAxis(next);
                    break;
            }
        }

    }
}
