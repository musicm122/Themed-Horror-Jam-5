using System;
using Godot;

namespace ThemedHorrorJam5.Scripts.Patterns.Logger
{

    public class GDLogger : ILogger
    {
        public GDLogger(LogLevelOutput level = LogLevelOutput.Warning) => Level = level;

        public LogLevelOutput Level { get; set; }

        public void Debug(params object[] messages)
        {
            if (Level <= LogLevelOutput.Debug)
            {
                GD.Print("Debug:", messages);
            }
        }

        public void Error(params object[] messages)
        {
            if (Level <= LogLevelOutput.Error)
            {
                GD.PrintErr("Error:", messages);
            }

        }

        public void Error(Exception ex)
        {
            if (Level <= LogLevelOutput.Error)
            {
                GD.PrintErr("Error:", ex.Message, ex.StackTrace, ex.Source);
            }
        }

        public void Error(Exception ex, params object[] messages)
        {
            if (Level <= LogLevelOutput.Error)
            {
                GD.PrintErr("Error:", messages);
            }
        }

        public void Info(params object[] messages)
        {
            if (Level <= LogLevelOutput.Info)
            {
                GD.Print("Info:", messages);
            }
        }

        public void Warning(params object[] messages)
        {
            if (Level <= LogLevelOutput.Warning)
            {
                GD.Print("Warning:", messages);
            }
        }
    }
}