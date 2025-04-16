﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Inventory
    {
        int inventorySpace;
        public List<ITradable> items { get; private set; } //보유중인 아이템
        public List<Equipment> equipments { get; private set; } //장비 아이템
        public List<Equipment> equippedItems { get; private set; } //장비중인 아이템

        public event Action? OnEquipChanged;
        public Inventory(int space)
        {
            inventorySpace = space;
            items = new List<ITradable>(inventorySpace);
            equipments = new List<Equipment>();  
            equippedItems = new List<Equipment>();
            foreach(ITradable item in items)
            {
                switch (item.ItemType)
                {
                    case ItemType.Equipment:  //장비 아이템인 경우
                        equipments.Add((Equipment)item);    //장비 아이템 리스트에 할당
                        break;
                    default:
                        break;
                }
            }
        }

        public void AddItem(ITradable item)  //인벤토리에 아이템 추가
        {
            items.Add(item);    
            switch(item.ItemType)   //아이템 분류 과정
            {
                case ItemType.Equipment:
                    equipments.Add((Equipment)item);
                    break;
                default:
                    break;
            }
        }
        public void ShowItems()
        {
            Console.Clear();
            Console.WriteLine("<인벤토리>");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("\n[아이템 목록]");
            foreach(ITradable item in items) 
            {
                Console.Write("- ");
                item.ShowInfo();
            }
            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");


            switch(Game.Instance.GetPlayerInput()) 
            {
                case 1:
                    ControlEquipments();
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Game.Instance.Pause();
                    ShowItems();
                    break;
            }
        }

        public void ControlEquipments()
        {
            Console.Clear();
            Console.Write("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            Console.WriteLine("\n[아이템 목록]");
            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].isEquipped)
                {
                    Console.Write($"- {i+1} [E]");
                    equipments[i].ShowInfo();
                }
                else
                {
                    Console.Write($"- {i + 1}   ");
                    equipments[i].ShowInfo();
                }
            }

            Console.WriteLine("\n0. 나가기");
            Console.Write("장착/해제할 아이템 번호를 입력하세요. ");
            
            int playerInput = Game.Instance.GetPlayerInput(); 

            if(playerInput > equipments.Count || playerInput == -1)
            {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                Console.WriteLine("잘못된 입력입니다.");
                Game.Instance.Pause();
                ControlEquipments();
            }

            else if (playerInput == 0) //인풋 0 : 나가기
            {
                return;
            }
            else 
            {
                //아이템이 이미 장착된 경우
                if (equipments[playerInput - 1].isEquipped)
                {
                    equipments[playerInput - 1].UnEquip(); //장착 해제
                    equippedItems.Remove(equipments[playerInput - 1]);
                }
                else  //아이템이 장착되지 않은 경우
                {
                    equipments[playerInput - 1].Equip();  //장착
                    equippedItems.Add(equipments[playerInput - 1]);
                }
                OnEquipChanged?.Invoke(); //장비 바뀜 이벤트 호출
            }
          
        }

    }
}
