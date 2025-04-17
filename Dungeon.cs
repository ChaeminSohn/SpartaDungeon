using System.Linq.Expressions;

namespace SpartaDungeon
{
    internal class Dungeon
    {
        //던전은 총 3단계로 구성.

        //권장 방어력
        public int[] DefenseLevel { get; private set; } = { 5, 10, 20 };

        //보상 골드
        public int[] GoldReward { get; private set; } = { 1000, 1700, 2500 };

        private static Random rand = new Random();


        public bool EnterDungeon(Player player, int level)  //던전 입장
        {   //던전 성공시 true, 실패시 false 반환
            rand = new Random();
            level--;
            int bonusDamage = player.Defense - DefenseLevel[level];
            if (player.Defense < DefenseLevel[level])
            {
                if (rand.NextDouble() < 0.4)  //40프로 확률로 던전 실패
                {
                    //플레이어 체력 절반 감소
                    player.OnDamage(player.FullHP / 2);
                    return false;
                }
            }
            //던전 성공
            //체력 소모 : 내 방어력 - 권장 방어력만큼 랜덤 값에 설정
            int damage = rand.Next(20 - bonusDamage, 35 - bonusDamage);
            player.OnDamage(damage);

            //공격력 ~ 공격력 * 2 의 % 만큼 추가 보상 획득 가능
            float bonusGold = rand.Next(player.Attack, player.Attack * 2) / 100f + 1f;
            player.ChangeGold((int)(GoldReward[level] * bonusGold));
            return true;
        }
    }
}