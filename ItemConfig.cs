using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpartaDungeon
{
  internal class ItemConfig
  {
    public List<ItemData> Equipments { get; set; } = new List<ItemData>();  //장비 아이템 리스트
    public List<ItemData> Usables { get; set; } = new List<ItemData>();  //소비 아이템 리스트
    public List<ItemData> Others { get; set; } = new List<ItemData>();  //기타 아이템 리스트
  }
}
