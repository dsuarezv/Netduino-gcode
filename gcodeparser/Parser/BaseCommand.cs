using System;
using System.Text;

namespace gcodeparser
{
    class BaseCommand
    {
        public bool Deleted = false;
        public int LineNumber = -1;

        public virtual void Parse()
        { 
            
        }

        public virtual void Run()
        { 
            
        }
    }
}
