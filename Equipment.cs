using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum EquipType
    {
        Weapon = 0,
        Armor = 1
    }
    internal class Equipment : ITradable  //장비 아이템을 구현하는 클래스
    {
        private ItemInfo ItemInfo;

        public string Name => ItemInfo.Name;
        public string Description => ItemInfo.Description;
        public int Price => ItemInfo.Price;
        public ItemType ItemType => ItemInfo.ItemType;
        public Stat Stat => ItemInfo.Stat ?? default; //스탯 종류(공격력, 방어력, ....)
        public int StatValue => ItemInfo.StatValue ?? 0;  //장비 스탯 계수
        public bool isEquipped { get; private set; } //플레이어 착용 여부
        public bool IsForSale { get; private set; }   //판매 여부
        public EquipType EquipType => ItemInfo.EquipType; //장비 종류


        public Equipment(ItemInfo itemInfo)
        {
            this.ItemInfo = itemInfo;
            isEquipped = itemInfo.IsEquipped;
            IsForSale = itemInfo.IsForSale;
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
        public void ShowInfoShop()  //정보 표시 - 상점
        {
            string typeFormatted = Utils.PadToWidth(Utils.EquipTypeDisplayNames[EquipType], 6);
            string nameFormatted = Utils.PadToWidth(Name, 15);
            string statFormatted = Utils.PadToWidth($"{Utils.StatDisplayNames[Stat]} +{StatValue}", 15);
            string descFormatted = Utils.PadToWidth(Description, 50);
            string priceFormatted;
            if (IsForSale)
            {
                priceFormatted = Utils.PadToWidth($"{Price} G", 8);
            }
            else
            {
                priceFormatted = Utils.PadToWidth("[판매 완료]", 8);
            }

            Console.WriteLine($"{typeFormatted} | {nameFormatted} | {statFormatted} | {descFormatted} | {priceFormatted}");
        }
        public void ShowInfoInventory()     //정보 표시 - 인벤토리
        {
            string typeFormatted = Utils.PadToWidth(Utils.EquipTypeDisplayNames[EquipType], 6);
            string nameFormatted = Utils.PadToWidth(Name, 15);
            string statFormatted = Utils.PadToWidth($"{Utils.StatDisplayNames[Stat]} +{StatValue}", 15);
            string descFormatted = Utils.PadToWidth(Description, 50);


            Console.WriteLine($"{typeFormatted} | {nameFormatted} | {statFormatted} | {descFormatted}");
        }

        public ItemInfo GetItemInfo()
        {
            return new ItemInfo(Name, ItemType, EquipType, Stat, StatValue, Description, Price, IsForSale, isEquipped);
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
