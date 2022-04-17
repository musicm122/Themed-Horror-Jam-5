using ThemedHorrorJam5.Scripts.Mission;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class MissionManagerEventArgs
    {
        public MissionElement mission;

        public MissionManagerEventArgs(MissionElement mission) => this.mission = mission;
    }
}