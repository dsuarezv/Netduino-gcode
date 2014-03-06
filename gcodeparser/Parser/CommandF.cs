using System;


namespace gcodeparser
{
    // Set feed rate
    internal class CommandF : BaseCommand
    {
        public override void Parse()
        {
            float feedRate = (float)GCodeParser.ParseDouble();

            DeviceFactory.Get().SetFeedRate(feedRate);
        }
    }
}
