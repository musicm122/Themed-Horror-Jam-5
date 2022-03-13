using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class DialogicUtils
    {
        private const string path = @"X:\Projects\Godot\dialog_2day_game\addons\dialogic\Other\DialogicClass.gd";
        private static readonly Script _dialogic = GD.Load<Script>("res://addons/dialogic/Other/DialogicClass.gd");
        private const string DEFAULT_DIALOG_RESOURCE = "res://addons/dialogic/Nodes/DialogNode.tscn";
        public static Node GetDialog(string timeLine)
        {
            var retval = _dialogic.Call("Start", timeLine);
            return (Node2D)retval;
        }
    }
}
