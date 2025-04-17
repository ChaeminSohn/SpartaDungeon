using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SpartaDungeon
{
    internal struct ItemData    //아이템 정보를 관리하는 구조체
    {
        //공용 필드
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ItemType ItemType { get; set; }

        //장비 전용 필드
        public EquipType? EquipType { get; set; }
        public Stat? Stat { get; set; }
        public int? StatValue { get; set; }

        public ItemData(string name, ItemType itemType, EquipType equipType, Stat stat, int statValue, string description, int price)
        {
            Name = name;
            EquipType = equipType;
            Stat = stat;
            StatValue = statValue;
            Description = description;
            Price = price;
            ItemType = itemType;
        }
    }
}
