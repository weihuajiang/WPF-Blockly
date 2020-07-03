using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ThreadCollection : Library
    {
        public ThreadCollection()
        {
            Name = "Thread";
            Title = Language.Title;
            DefaultColor = "lightblue";
            Description = Language.Description;
            CommandGroup thread = new CommandGroup(Language.ThreadCategory, Language.ThreadCategoryDesc);
            thread.Add(new Command("wait", Language.SleepDescription, true, new WaitStatement()));
            Add(thread);
        }
    }
}
