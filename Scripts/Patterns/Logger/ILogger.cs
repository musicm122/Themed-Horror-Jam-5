using System;

namespace ThemedHorrorJam5.Scripts.Patterns.Logger
{
    public interface ILogger
    {
        public LogLevelOutput Level {get;set;}
        
        void Info(params object[] messages);
        void Debug(params object[] messages);
        void Warning(params object[] messages);
        void Error(params object[] messages);
        void Error(Exception ex);
        void Error(Exception ex, params object[] messages);
    }
}