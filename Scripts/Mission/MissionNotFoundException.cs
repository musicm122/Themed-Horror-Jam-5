using System;
using System.Runtime.Serialization;

namespace ThemedHorrorJam5.Scripts.Mission
{
    [Serializable]
    internal class MissionNotFoundException : Exception
    {
        public string MissionTitle { get; set; }

        public MissionNotFoundException()
        {
        }

        public MissionNotFoundException(string title)
        {
            MissionTitle = title;
        }

        public MissionNotFoundException(string title, string message) : base(message)
        {
            MissionTitle = title;
        }

        public MissionNotFoundException(string title, string message, Exception innerException) : base(message, innerException)
        {
            MissionTitle = title;
        }

        protected MissionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MissionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}