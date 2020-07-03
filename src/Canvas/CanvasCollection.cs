using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchNet
{
    public class CanvasCollection : Library
    {
        public CanvasCollection()
        {
            Name ="Canvas";
            Title = Language.Title;
            DefaultColor = "orchid";
            Description = Language.Description;
            CommandGroup draw = new CommandGroup(Language.DrawCategory, Language.DrawCategoryDesc);
            draw.Add(new Command("line", Language.LineDescription, true, new LineStatement()));
            draw.Add(new Command("lineTo", Language.LineToDescription, true, new LineToStatement()));
            draw.Add(new Command("arc", Language.ArcDescription, true, new ArcStatement()));
            draw.Add(new Command("text", Language.TextDescription, true, new TextStatement()));
            draw.Add(new Command("pen down", Language.PenDownDescription, true, new CallCanvasFuncStatement("PenDown", "penDown()")));
            draw.Add(new Command("pen up", Language.PenUpDescription, true, new CallCanvasFuncStatement("PenUp", "penUp()")));
            draw.Add(new Command("start fill", Language.StartFillDescription, true, new CallCanvasFuncStatement("StartFill", "startFill()")));
            draw.Add(new Command("stop fill", Language.StopFillDescription, true, new CallCanvasFuncStatement("StopFill", "stopFill()")));
            draw.Add(new Command("clear", Language.ClearDescription, true, new CallCanvasFuncStatement("Clear", "clear()")));
            draw.Add(new Command("reset", Language.ResetDescription, true, new CallCanvasFuncStatement("Reset", "reset()")));
            CommandGroup position = new CommandGroup(Language.PositionCategory, Language.PositionCategoryDesc);
            position.Add(new Command("turn", Language.RotateDescription, true, new TurnStatement()));
            position.Add(new Command("goto", Language.MoveDescription, true, new GotoStatement()));
            position.Add(new Command("x", Language.XPosDescription, true, new XExpression()));
            position.Add(new Command("x", Language.YPosDescription, true, new YExpression()));
            position.Add(new Command("direction", Language.AngleDescription, true, new HeadingExpression()));
            CommandGroup parameter = new CommandGroup(Language.ParameterCategory, Language.ParameterCategoryDesc);
            parameter.Add(new Command("color", Language.LineColorDesc, true, new LineColorExpression()));
            parameter.Add(new Command("fill", Language.FillColorDesc, true, new FillColorExpression()));
            parameter.Add(new Command("thickness", Language.LineSizeDesc, true, new ThicknessExpression()));
            parameter.Add(new Command("fontSize", Language.FontSizeDesc, true, new FontSizeExpression()));
            parameter.Add(new Command("setFont", Language.SetFontDesc, true, new SetFontStatement()));
            CommandGroup colors = new CommandGroup(Language.ColorCategory, Language.ColorCategoryDesc);
            colors.Add(new Command("rgb", Language.RGBDesc, true, new RgbExpression()));
            colors.Add(new Command("red", Language.ColorRedDesc, true, new ColorExpression() { DisplayText = "red", Color = Colors.Red }));
            colors.Add(new Command("green", Language.ColorGreenDesc, true, new ColorExpression() { DisplayText = "green", Color = Colors.Green }));
            colors.Add(new Command("blue", Language.ColorBlueDesc, true, new ColorExpression() { DisplayText = "blue", Color = Colors.Blue }));
            colors.Add(new Command("orange", Language.ColorOrangeDesc, true, new ColorExpression() { DisplayText = "orange", Color = Colors.Orange }));
            colors.Add(new Command("yellow", Language.ColorYellowDesc, true, new ColorExpression() { DisplayText = "yellow", Color = Colors.Yellow }));
            colors.Add(new Command("black", Language.ColorBlackDesc, true, new ColorExpression() { DisplayText = "black", Color = Colors.Black }));
            colors.Add(new Command("white", Language.ColorWhiteDesc, true, new ColorExpression() { DisplayText = "white", Color = Colors.White }));
            Add(draw);
            Add(parameter);
            Add(position);
            Add(colors);
        }
    }
}
