using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    enum ItemType
    {
        Equipment,  //장비 아이템
        Accessory,  //장신구 아이템
        Usable,     //소비 아이템
    }
    internal interface ITradable //거래 가능한 아이템 구현 시 사용
    {
        String Name { get; }
        String Description { get; }
        int Price { get; }
        ItemType ItemType { get; }

        public void ShowInfo();
    }
}
