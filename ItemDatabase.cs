using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpartaDungeon
{
  internal static class ItemDatabase
  {
    public static List<ItemData> Items { get; private set; } = new List<ItemData>();

    public static void LoadItems()
    {
      try //null 예외처리
      {
        string json = File.ReadAllText("EquipItemDatabase.json");

        var options = new JsonSerializerOptions
        {
          Converters = { new JsonStringEnumConverter() }
        };

        var loadedItems = JsonSerializer.Deserialize<List<ItemData>>(json, options);
        if (loadedItems != null)
        {
          Items = loadedItems;
        }
        else
        {
          Console.WriteLine("JSON 파일 오류 발생");
          Items = new List<ItemData>();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"아이템 데이터 로드 중 오류 발생: {ex.Message}");
        Items = new List<ItemData>();
      }
    }


    public static readonly ItemData BeninnerArmor = new ItemData(
        name: "수련자 갑옷",
        description: "수련에 도움을 주는 갑옷입니다.",
        price: 1000,
        itemType: ItemType.Equipment,
        equipmentType: EquipmentType.Armor,
        statType: StatType.Defense,
        stat: 5
        );

    public static readonly ItemData SteelArmor = new ItemData(
      name: "무쇠 갑옷",
      description: "무쇠로 만들어져 튼튼한 갑옷입니다.",
      price: 2000,
      itemType: ItemType.Equipment,
      equipmentType: EquipmentType.Armor,
      statType: StatType.Defense,
      stat: 9
      );

    public static readonly ItemData SpartanArmor = new ItemData(
      name: "스파르타의 갑옷",
      description: "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.",
      price: 3500,
      itemType: ItemType.Equipment,
      equipmentType: EquipmentType.Armor,
      statType: StatType.Defense,
      stat: 15
      );

    public static readonly ItemData RustySword = new ItemData(
        name: "낡은 검",
        description: "쉽게 볼 수 있는 낡은 검 입니다.",
        price: 600,
         itemType: ItemType.Equipment,
        equipmentType: EquipmentType.Weapon,
        statType: StatType.Attack,
        stat: 2
         );

    public static readonly ItemData BronzeAxe = new ItemData(
       name: "청동 도끼",
       description: "어디선가 사용됐던 것 같은 도끼입니다.",
       price: 1500,
        itemType: ItemType.Equipment,
       equipmentType: EquipmentType.Weapon,
       statType: StatType.Attack,
       stat: 5
        );

    public static readonly ItemData SpartanSpear = new ItemData(
       name: "스파르타의 창",
       description: "스파르타의 전사들이 사용했다는 전설의 창입니다.",
       price: 3000,
        itemType: ItemType.Equipment,
       equipmentType: EquipmentType.Weapon,
       statType: StatType.Attack,
       stat: 7
        );
    public static readonly ItemData LegendSword = new ItemData(
      name: "전설의 검",
      description: "전설로만 전해져 온 최강의 검입니다.",
      price: 100000,
       itemType: ItemType.Equipment,
      equipmentType: EquipmentType.Weapon,
      statType: StatType.Attack,
      stat: 15
       );

  }
}
