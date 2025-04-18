namespace SpartaDungeon
{
    internal class PlayerConfig   //각 직업별 PlayerData의 초기 설정값을 저장하는 클래스
    {
        public PlayerData? BaseWarriorData { get; set; }
        public PlayerData? BaseMageData { get; set; }
        public PlayerData? BaseArcherData { get; set; }

    }
}