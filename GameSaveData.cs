namespace SpartaDungeon
{
    public class GameSaveData   //게임 데이터 저장용 래퍼 클래스
    {
        public PlayerData PlayerData { get; set; }  //플레이어의 정보를 담는 객체
        public List<ItemInfo> ItemData { get; set; }    //모든 아이템들의 정보를 담는 객체

        public GameSaveData(PlayerData playerData, List<ItemInfo> itemData)
        {
            PlayerData = playerData;
            ItemData = itemData;
        }
    }
}