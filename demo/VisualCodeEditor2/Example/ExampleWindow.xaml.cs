using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for ExampleWindow.xaml
    /// </summary>
    public partial class ExampleWindow : Window
    {
        public ExampleWindow()
        {
            InitializeComponent();
            List<ExampleItem> examples = new List<ExampleItem>();
            examples.Add(new ExampleItem() { Name = Properties.Resources.VariableExample, 
                Description = Properties.Resources.VariableExampleDescription, 
                ScriptFile="ScratchNet.scripts.variable.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleVariableScope, 
                Description = Properties.Resources.ExampleVariableScopeDesc, ScriptFile = 
                "ScratchNet.scripts.variableScope.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleLogic, 
                Description = Properties.Resources.ExampleLogicDesc, 
                ScriptFile = "ScratchNet.scripts.if.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleLoop, 
                Description = Properties.Resources.ExampleLoopDesc, 
                ScriptFile = "ScratchNet.scripts.loop.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleRecursive, 
                Description = Properties.Resources.ExampleRecursiveDesc, 
                ScriptFile = "ScratchNet.scripts.recursion.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleArray, 
                Description = Properties.Resources.ExampleArrayDesc, 
                ScriptFile = "ScratchNet.scripts.array.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleNumber, 
                Description = Properties.Resources.ExampleNumberDesc, 
                ScriptFile = "ScratchNet.scripts.narcissistic.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExamplePrime, 
                Description = Properties.Resources.ExamplePrimeDesc, 
                ScriptFile = "ScratchNet.scripts.prime.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleArray2, 
                Description = Properties.Resources.ExampleArray2Desc, 
                ScriptFile = "ScratchNet.scripts.array2.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleCanvas, 
                Description = Properties.Resources.ExampleCanvasDesc, 
                ScriptFile = "ScratchNet.scripts.draw2.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleSpeech, 
                Description = Properties.Resources.ExampleSpeechDesc, 
                ScriptFile = "ScratchNet.scripts.speech.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleStack, 
                Description = Properties.Resources.ExampleStackDesc, 
                ScriptFile = "ScratchNet.scripts.stack.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleQueue, 
                Description = Properties.Resources.ExampleQueueDesc, 
                ScriptFile = "ScratchNet.scripts.queue.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleList, 
                Description = Properties.Resources.ExampleListDesc, 
                ScriptFile = "ScratchNet.scripts.list.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleLinkedList, 
                Description = Properties.Resources.ExampleLinkedListDesc, 
                ScriptFile = "ScratchNet.scripts.linkedList.vsc" });
            examples.Add(new ExampleItem() { Name = Properties.Resources.ExampleBinaryTree, 
                Description = Properties.Resources.ExampleBinaryTreeDesc, 
                ScriptFile = "ScratchNet.scripts.binaryTree.vsc" });
            ExampleList.ItemsSource = examples;
        }
        public Class Script
        {
            get
            {
                ExampleItem select = ExampleList.SelectedValue as ExampleItem;
                if (select == null)
                    return null;
                string resource = select.ScriptFile;
                try
                {
                    using (var stream = GetType().Assembly.GetManifestResourceStream(resource))
                    {
                        Class t = Serialization.Load(stream);
                        stream.Close();
                        return t;
                    }
                }
                catch(Exception e) { Console.WriteLine(e.Message);Console.WriteLine(e.StackTrace); }
                return null;
            }
        }
        private void OnComfirm(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ExampleList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExampleItem select = ExampleList.SelectedValue as ExampleItem;
            if (select == null)
                return;
            this.DialogResult = true;
            this.Close();
        }
    }
}
