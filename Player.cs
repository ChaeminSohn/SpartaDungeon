using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{

    enum CharacterClass    //직업 종류
    {
        Warrior,
        Mage,
        Archer
    }
    internal class Player : IBattleUnit
    {
        public string Name { get; private set; }
        public int Level { get; private set; }
        public int BaseFullHP { get; private set; }
        public int BonusFullHP { get; private set; }
        public int FullHP => BaseFullHP + BonusFullHP;
        public int CurrentHP { get; private set; }
        public int BaseAttack { get; private set; }
        public int BonusAttack { get; private set; }
        public int Attack => BaseAttack + BonusAttack;
        public int BaseDefense { get; private set; }
        public int BonusDefense { get; private set; }
        public int Defense => BaseDefense + BonusDefense;
        public CharacterClass characterClass { get; private set; }
        public int Gold { get; private set; }
        public Inventory inventory { get; private set; }
        public Player(string name, CharacterClass characterClass, Inventory inventory)
        {
            Name = name;
            this.characterClass = characterClass;
            Level = 1;
            BaseFullHP = 100;
            CurrentHP = BaseFullHP;
            BaseAttack = 10;
            BaseDefense = 5;
            Gold = 1500;
            this.inventory = inventory;
            inventory.OnEquipChanged += UpdatePlayerStats;
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
            foreach (Equipment item in inventory.equippedItems)
            {
                switch (item.StatType)
                {
                    case StatType.Health:
                        BonusFullHP += item.Stat;
                        break;
                    case StatType.Attack:
                        BonusAttack += item.Stat;
                        break;
                    case StatType.Defense:
                        BonusDefense += item.Stat;
                        break;
                    default:
                        break;
                }
            }
        }

        public void ChangeGold(int gold)
        {
            Gold += gold;
        }
        public void BuyItem(ITradable item)
        {
            ChangeGold(-item.Price);
            inventory.AddItem(item);
        }

        public void SellItem(int index, int gold)
        {
            ChangeGold(gold);
            inventory.items.RemoveAt(index);
        }

        public void ShowInventory()
        {
            inventory.ShowItems();
        }

        public void ShowPlayerInfo()
        {
            Console.Clear();
            Console.WriteLine("<상태 보기>");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine($"\nLv. {Level}");
            Console.WriteLine($"{Name} ({characterClass})");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Defense}");
            Console.WriteLine($"체력 : {CurrentHP}/{FullHP}");
            Console.WriteLine($"Gold : {Gold} G");
        }
    }
}
