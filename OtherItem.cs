using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class OtherItem : ITradable    //기타 아이템
    {
        private ItemInfo itemInfo;
        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public int Price => throw new NotImplementedException();

        public ItemType ItemType => ItemType.Other;

        public bool IsForSale => throw new NotImplementedException();


        public OtherItem(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
        }
        public ItemInfo GetItemInfo()
        {
            throw new NotImplementedException();
        }

        public void OnTrade()
        {
            throw new NotImplementedException();
        }

        public void ShowInfo(bool showPrice)
        {

        }
    }
}
