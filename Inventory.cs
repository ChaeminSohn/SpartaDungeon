using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Inventory
    {
        int inventorySpace = 8;
        public List<ITradable> items { get; private set; } //보유중인 아이템
        public List<Equipment> equipments { get; private set; } //장비 아이템
        public Equipment[] equippedItems { get; private set; } //플레이어가 장비중인 아이템
        public event Action? OnEquipChanged;    //플레이어 장비 변환 이벤트
        public Inventory()
        {
            items = new List<ITradable>(inventorySpace);
            equipments = new List<Equipment>();
            equippedItems = new Equipment[2];   //0 : 무기 1 :방어구
            foreach (ITradable item in items)
            {
                switch (item.ItemType)
                {
                    case ItemType.Equipment:  //장비 아이템
                        equipments.Add((Equipment)item);
                        break;
                    case ItemType.Usable:  //소비 아이템   
                        break;
                    case ItemType.Other:  //기타 아이템  
                        break;
                    default:
                        break;
                }
            }
        }

        public void AddItem(ITradable item)  //인벤토리에 아이템 추가
        {
            items.Add(item);
            switch (item.ItemType)   //아이템 분류 과정
            {
                case ItemType.Equipment:

                    equipments.Add((Equipment)item);
                    break;
                default:
                    break;
            }
        }

        public void RemoveItem(ITradable item)
        {
            items.Remove(item);
            switch (item.ItemType)   //아이템 분류 과정
            {
                case ItemType.Equipment:
                    equipments.Remove((Equipment)item);
                    break;
                default:
                    break;
            }
        }
        public void ShowItems()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("<인벤토리>");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("\n[아이템 목록]");
                foreach (ITradable item in items)
                {
                    Console.Write("- ");
                    item.ShowInfo();
                }
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");


                switch (Utils.GetPlayerInput(true))
                {
                    case 1:
                        ControlEquipments();
                        break;
                    case 0:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

        public void ControlEquipments()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.Write("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

                Console.WriteLine("\n[아이템 목록]");
                for (int i = 0; i < equipments.Count; i++)
                {
                    if (equipments[i].isEquipped)
                    {
                        Console.Write($"- {i + 1} [E]");
                        equipments[i].ShowInfo();
                    }
                    else
                    {
                        Console.Write($"- {i + 1}   ");
                        equipments[i].ShowInfo();
                    }
                }

                Console.WriteLine("\n0. 나가기");
                Console.Write("\n장착/해제할 아이템 번호를 입력하세요. ");

                int playerInput = Utils.GetPlayerInput(false);

                if (playerInput == 0) //인풋 0 : 나가기
                {
                    exit = true;
                }
                else if (playerInput > equipments.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("\n잘못된 입력입니다.");
                    Utils.Pause(false);
                }
                else
                {
                    Equipment selected = equipments[playerInput - 1];
                    int equipIndex = (int)selected.EquipType;

                    if (selected.isEquipped)    //이미 장착된 경우
                    {
                        selected.UnEquip();     //장착 해제
                        equippedItems[equipIndex] = null;
                    }
                    else
                    {
                        //같은 종류 장비가 이미 장착되어 있으면 해제
                        if (equippedItems[equipIndex] != null)
                        {
                            equippedItems[equipIndex].UnEquip();
                        }

                        selected.Equip();
                        equippedItems[equipIndex] = selected;
                    }
                    OnEquipChanged?.Invoke();
                }
            }

        }

        public void ShowEquipedItems()  //현재 장비중인 아이템 목록을 보여줌
        {

        }

    }
}
