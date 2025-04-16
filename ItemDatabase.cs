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

    public static void LoadItems()  //json 파일을 통해 아이템 데이터 불러오기
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
  }
}
