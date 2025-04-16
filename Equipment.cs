using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    enum EquipmentType
    {
        Armor,
        Weapon
    }
    internal class Equipment : ITradable  //장비 아이템을 구현하는 클래스
    {
        private ItemData itemData;

        public string Name => itemData.Name;
        public string Description => itemData.Description;
        public int Price => itemData.Price;
        public ItemType ItemType => itemData.ItemType;
        public StatType StatType => itemData.StatType ?? default; //스탯 종류(공격력, 방어력, ....)
        public int Stat => itemData.Stat ?? 0;  //장비 스탯 계수
        public bool isEquipped { get; private set; } //플레이어 착용 여부
        public EquipmentType EquipmentType => itemData.EquipmentType ?? default; //장비 종류
       
 
        public Equipment(ItemData itemData)
        {
             this.itemData = itemData;
        }

        public void ShowInfo()
        {
            string nameFormatted = Utils.PadToWidth(Name, 15);
            string statFormatted = Utils.PadToWidth($"{StatType} +{Stat}", 15);
            string descFormatted = Utils.PadToWidth(Description, 50);
            string priceFormatted = Utils.PadToWidth($"{Price} G", 8);

            Console.WriteLine($"{nameFormatted} | {statFormatted} | {descFormatted} | {priceFormatted}");
        }

        public void Equip()
        {
            isEquipped = true;
        }

        public void UnEquip()
        {
            isEquipped = false;
        }
    }
}
