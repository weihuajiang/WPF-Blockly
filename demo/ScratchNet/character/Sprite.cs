using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ScratchNet
{
    public class Resource
    {
        public string FileName { get; set; }
        public string DisplayName { get; set; }
        BitmapFrame _img;
        public BitmapFrame Image
        {
            get
            {
                if(_img==null)
                    _img = BitmapFrame.Create(new Uri(FileName));
                return _img;
            }
        }
    }
    public class Sprite : Class
    {
        public string Name { get; set; }
        public ResourcesList Images { get; set; }
        public ResourcesList Sounds { get; set; }
        int _x = 0;
        int _y = 0;
        int _degree = 0;
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                if (value < 0)
                {
                    _x = 0;
                }
                else if (value > CurrentEnviroment.ScreenWidth)
                {
                    _x = CurrentEnviroment.ScreenWidth;
                }
                else
                {
                    _x = value;
                }
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value < 0)
                {
                    _y = 0;
                }
                else if (value > CurrentEnviroment.ScreenHeight)
                {
                    _y = CurrentEnviroment.ScreenHeight;
                }
                else
                {
                    _y = value;
                }
            }
        }
        public int Direction
        {
            get
            {
                return _degree;
            }
            set
            {
                while (value < 0)
                {
                    value = value + 360;
                }
                while (value >= 360)
                {
                    value = value - 360;
                }
                _degree = value;
            }
        }
        public CharacterRotationMode RotationMode { get; set; }
        public int Size { get; set; }
        public bool Visible { get; set; }
        public int Layer { get; set; }
        public int CurrentImage { get; set; }
        public Sprite()
        {
            Images = new ResourcesList();
            Sounds = new ResourcesList();
            Positions = new Dictionary<object, Point>();
            Variables = new List<Variable>();
            Functions = new List<Function>();
            Handlers = new List<EventHandler>();
            Expressions = new List<Expression>();
            BlockStatements = new List<BlockStatement>();
            CurrentImage = 0;
            Size=100;
            Visible = true;
        }
        public Dictionary<object, Point> Positions
        {
            get;
            set;
        }
        public List<Variable> Variables
        {
            get;
            set;
        }

        public List<Function> Functions
        {
            get;
            set;
        }

        public List<EventHandler> Handlers
        {
            get;
            set;
        }
        public List<Expression> Expressions
        {
            get;
            set;
        }
        public List<BlockStatement> BlockStatements
        {
            get;
            set;
        }
    }
}
