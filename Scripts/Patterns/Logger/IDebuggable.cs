using Godot;
using System;
using System.Runtime.CompilerServices;

namespace ThemedHorrorJam5.Scripts.Patterns.Logger
{
    public interface IDebuggable<T> where T : Node
    {
        bool IsDebugPrintEnabled() => IsDebugging;
        
        
        [Export]
        bool IsDebugging { get; set; }
    }
}