using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    class CurrentEnviroment
    {
        public static ObservableCollection<string> Variables = new ObservableCollection<string>();
        public static ObservableCollection<string> SpriteNames = new ObservableCollection<string>();
        public static ObservableCollection<string> CurrentSpriteImages = new ObservableCollection<string>();
        public static ObservableCollection<string> Messages = new ObservableCollection<string>();
        public static Game Game { get; set; }
        public static int ScreenWidth = 400;
        public static int ScreenHeight = 300;
    }
}