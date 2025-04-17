using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    enum EquipType
    {
        Weapon = 0,
        Armor = 1
    }
    internal class Equipment : ITradable  //장비 아이템을 구현하는 클래스
    {
        private ItemData itemData;

        public string Name => itemData.Name;
        public string Description => itemData.Description;
        public int Price => itemData.Price;
        public ItemType ItemType => itemData.ItemType;
        public Stat Stat => itemData.Stat ?? default; //스탯 종류(공격력, 방어력, ....)
        public int StatValue => itemData.StatValue ?? 0;  //장비 스탯 계수
        public bool isEquipped { get; private set; } //플레이어 착용 여부
        public bool IsForSale { get; private set; } = true;   //판매 여부
        public EquipType EquipType => itemData.EquipType ?? default; //장비 종류


        public Equipment(ItemData itemData)
        {
            this.itemData = itemData;
        }

        public void OnTrade()   //상점에서 구매/판매 시 호출
        {
            if (IsForSale)
            {
                IsForSale = false;
            }
            else
            {
                UnEquip();
                IsForSale = true;
            }
        }
        public void ShowInfo()
        {
            string typeFormatted = Utils.PadToWidth(Utils.EquipTypeDisplayNames[EquipType], 6);
            string nameFormatted = Utils.PadToWidth(Name, 15);
            string statFormatted = Utils.PadToWidth($"{Utils.StatDisplayNames[Stat]} +{StatValue}", 15);
            string descFormatted = Utils.PadToWidth(Description, 50);
            string priceFormatted = Utils.PadToWidth($"{Price} G", 8);

            Console.WriteLine($"{typeFormatted} | {nameFormatted} | {statFormatted} | {descFormatted} | {priceFormatted}");
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
