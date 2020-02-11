using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class Background
    {
        public ResourcesList Images { get; set; }
        public Background()
        {
            Images = new ResourcesList();
            Name = Localize.GetString("cs_Background");
            CurrentImage = 0;
        }

        public string Name { get; set; }

        public int CurrentImage { get; set; }
    }
}
