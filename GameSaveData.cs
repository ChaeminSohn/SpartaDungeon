namespace SpartaDungeon
{
    public class GameSaveData   //게임 데이터 저장용 래퍼 클래스
    {
        public Player? Player { get; set; }
        public ItemConfig itemConfig { get; set; }
    }
}