using System;

namespace ThemedHorrorJam5.Scripts.Patterns.Logger
{
    public interface ILogger
    {
        public LogLevelOutput Level {get;set;}
        
        void Info(params string[] messages);
        void Debug(params string[] messages);
        void Warning(params string[] messages);
        void Error(params string[] messages);
        void Error(Exception ex);
        void Error(Exception ex, params string[] messages);
    }
}