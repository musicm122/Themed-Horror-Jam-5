namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class MissionManagerEventArgs
    {
        public Mission mission;

        public MissionManagerEventArgs(Mission mission) => this.mission = mission;
    }
}