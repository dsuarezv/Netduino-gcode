using System;


namespace gcodeparser
{
    internal class CommandM: BaseCommand
    {
        public override void Parse()
        {
            int mType = GCodeParser.ParseInt();

            switch (mType)
            {
                case -1: Logger.Log("M: no command"); return;
                case 0:  Logger.Log("M00: PAUSE"); return;
                case 1:  Logger.Log("M01: PAUSE if the stop switch is ON"); return;
                case 2:  Logger.Log("M02: END PROGRAM"); return;
                
                case 3:  Logger.Log("M03: Start spindle (clockwise)"); return;
                case 4:  Logger.Log("M04: Start spindle (counterclockwise)"); return;
                case 5:  Logger.Log("M05: Stop spindle"); return;

                case 6: Logger.Log("M06: Stop spindle and CHANGE TOOL (current tool index)"); return;
                    
                case 30: Logger.Log("M30: PROGRAM FINISH"); return;
                case 60: Logger.Log("M60: PAUSE to exchange pallet shuttles"); return;

                default:
                    Logger.Log("M: Unsupported: {0}", mType);
                    break;
            }
        }
    }
}
