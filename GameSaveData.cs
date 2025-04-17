namespace SpartaDungeon
{
    public class GameSaveData   //게임 데이터 저장용 래퍼 클래스
    {
        public PlayerData PlayerData { get; set; }
        public List<ItemInfo> ItemData { get; set; }

        public GameSaveData(PlayerData playerData, List<ItemInfo> itemData)
        {
            PlayerData = playerData;
            ItemData = itemData;
        }
    }
}