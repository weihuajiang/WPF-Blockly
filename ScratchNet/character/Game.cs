using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class Game
    {
        //public Background Background { get; set; }
        public ObservableCollection<Sprite> Sprites { get; set; }
        public List<Variable> Variables { get; set; }
        public Game()
        {
            //Background = new Background();
            //Background.Images.Add(new Resource() { DisplayName = "backdrop", FileName = System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar+"images\\backdrop1.png" });
            Sprites = new ObservableCollection<Sprite>();
            Variables = new List<Variable>();
            Instances = new Dictionary<Instance, Sprite>();
        }
        public void AddSprite(Sprite sp)
        {
            Sprites.Add(sp);
            Instance inst = new Instance(sp);
            Instances.Add(inst, sp);
            Dictionary<string, object> states = inst.States;
            states.Add("X", 5);
            states.Add("Y", 5);
            states.Add("Size", 100);
            states.Add("Direction", 0);
        }
        public void RemoveSprite(Sprite sp)
        {
            Sprites.Remove(sp);
            foreach (Instance inst in Instances.Keys)
            {
                if (Instances[inst] == sp)
                {
                    Instances.Remove(inst);
                    break;
                }
            }
        }
        public void Clear()
        {
            Sprites.Clear();
            Instances.Clear();
            Variables.Clear();
        }

        //for execution
        public Dictionary<Instance, Sprite> Instances;
    }
}
