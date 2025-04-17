using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{

    public enum Job    //직업 종류
    {
        Warrior,
        Mage,
        Archer
    }
    internal class Player : IBattleUnit
    {
        public string Name { get; private set; }    //이름
        public int Level { get; private set; }  //레벨
        public int MaxLevel { get; private set; }     //만랩
        public int Experience { get; private set; }   //경험치
        public int[] ExpThresholds { get; private set; } //경험치 상한선
        public int BaseFullHP { get; private set; }     //기본 최대 체력
        public int BonusFullHP { get; private set; }    //추가 최대 체력
        public int FullHP => BaseFullHP + BonusFullHP;  //최대 체력
        public int CurrentHP { get; private set; }      //현재 체력
        public int BaseAttack { get; private set; }     //기본 공격력
        public int BonusAttack { get; private set; }    //추가 공격력
        public int Attack => BaseAttack + BonusAttack;  //공력력
        public int BaseDefense { get; private set; }    //기본 방어력
        public int BonusDefense { get; private set; }   //추가 방어력
        public int Defense => BaseDefense + BonusDefense;   //방어력
        public Job Job { get; private set; }    //직업
        public int Gold { get; private set; }   //골드
        public Inventory Inventory { get; private set; }    //인벤토리

        public Player(string name, Job job, Inventory inventory)    //새 게임 생성자
        {
            Name = name;
            Job = job;
            Level = 1;
            Experience = 0;
            this.Inventory = inventory;
            LoadPlayerData();
            inventory.OnEquipChanged += UpdatePlayerStats;
        }

        public Player(PlayerData playerData, Inventory inventory)   //게임 불러오기 생성자
        {
            Inventory = inventory;
            Name = playerData.Name;
            Level = playerData.Level;
            MaxLevel = playerData.MaxLevel;
            Experience = playerData.Experience;
            ExpThresholds = playerData.ExpThresholds;
            BaseFullHP = playerData.BaseFullHP;
            CurrentHP = playerData.CurrentHP;
            BaseAttack = playerData.BaseAttack;
            BaseDefense = playerData.BaseDefense;
            Gold = playerData.Gold;
        }

        public void LoadPlayerData()
        {
            //플레이어 기본 데이터 파일 읽어오기
            if (!ConfigLoader.TryLoad<PlayerConfig>("player_config.json", out var config))
            {
                Console.WriteLine("플레이어 설정을 불러오지 못했습니다.");
                return;
            }
            PlayerData baseData;
            switch (Job)
            {
                case Job.Warrior:
                    baseData = config.BaseWarriorData;
                    break;
                case Job.Mage:
                    baseData = config.BaseMageData;
                    break;
                case Job.Archer:
                    baseData = config.BaseArcherData;
                    break;
                default:
                    baseData = config.BaseWarriorData;
                    break;
            }
            MaxLevel = baseData.MaxLevel;
            ExpThresholds = baseData.ExpThresholds;
            BaseFullHP = baseData.BaseFullHP;
            CurrentHP = BaseFullHP;
            BaseAttack = baseData.BaseAttack;
            BaseDefense = baseData.BaseDefense;
            Gold = baseData.Gold;

        }

        public void RestoreAfterLoad()  //게임 불러오기 후 실행
        {
            Inventory.OnEquipChanged += UpdatePlayerStats;
            UpdatePlayerStats();
        }
        public void OnDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                OnDie();
            }
        }

        public void OnDie()
        {

        }

        public void RecoverHP(int hp)
        {
            CurrentHP += hp;
            if (CurrentHP >= FullHP)
            {
                CurrentHP = FullHP;
            }
        }
        public void UpdatePlayerStats()
        {
            BonusFullHP = 0;
            BonusAttack = 0;
            BonusDefense = 0;
            foreach (Equipment item in Inventory.equippedItems)
            {
                if (item != null)
                {
                    switch (item.Stat)
                    {
                        case Stat.Health:
                            BonusFullHP += item.StatValue;
                            break;
                        case Stat.Attack:
                            BonusAttack += item.StatValue;
                            break;
                        case Stat.Defense:
                            BonusDefense += item.StatValue;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void GetEXP(int exp)   //경험치 획득
        {
            Experience += exp;
            if (Experience >= ExpThresholds[Level - 1])
            {
                if (Level == MaxLevel)      //이미 만랩인 경우
                {
                    Experience = ExpThresholds[Level - 1];
                    return;
                }
                int overflow = Experience - ExpThresholds[Level - 1];   //경험치 초과량
                LevelUP();
                Experience = overflow;
            }
        }

        public void LevelUP()   //레벨 업
        {
            BaseAttack += 1;
            BaseDefense += 1;
            Console.Clear();
            Console.WriteLine("\n레벨업!");
            Console.WriteLine($"레벨 {Level++} -> {Level}");
            Utils.Pause(true);
        }

        public void ChangeGold(int gold)
        {
            Gold += gold;
        }
        public void BuyItem(ITradable item)
        {
            ChangeGold(-item.Price);
            item.OnTrade();
            Inventory.AddItem(item);
        }

        public void SellItem(ITradable item, int gold)
        {
            ChangeGold(gold);
            item.OnTrade();
            Inventory.RemoveItem(item);
        }

        public void ShowInventory()
        {
            Inventory.ShowItems();
        }

        public void ShowPlayerInfo()
        {
            Console.Clear();
            Console.WriteLine("<상태 보기>");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine($"\nLv. {Level}");
            Console.WriteLine($"경험치 :  {Experience} / {ExpThresholds[Level - 1]}");
            Console.WriteLine($"{Name} ({Utils.JobDisplayNames[Job]})");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Defense}");
            Console.WriteLine($"체력 : {CurrentHP}/{FullHP}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        public PlayerData GetPlayerData()
        {
            return new PlayerData(Name, Job, Level, MaxLevel, Experience, ExpThresholds, BaseFullHP, CurrentHP,
                BaseAttack, BaseDefense, Gold);
        }
    }
}
