using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpartaDungeon
{
    internal class Shop
    {
        List<ITradable> sellingItems;       //모든 판매 아이템 목록
        ItemType[] itemTypes = (ItemType[])Enum.GetValues(typeof(ItemType));    //모든 아이템 타입을 담는 배열
        Player player;
        public Shop(Player player, ItemConfig itemConfig)
        {
            this.player = player;
            sellingItems = new List<ITradable>();

            foreach (ItemData itemData in itemConfig.Equipments)
            {
                sellingItems.Add(new Equipment(itemData));
            }
        }

        public void ShowShop()
        {
            bool exit = false;
            while (!exit)
            {
                //상점 인터페이스 표시
                Console.Clear();
                Console.WriteLine("<상점>");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine("\n[아이템 목록]\n");
                //아이템 목록 보여줌
                foreach (ITradable item in sellingItems)
                {
                    Console.Write("- ");
                    item.ShowInfo();
                }
                //플레이어 입력 받기
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");

                switch (Utils.GetPlayerInput(true))
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        BuyItems();
                        break;
                    case 2:
                        SellItems();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

        public void BuyItems() //아이템 구매 UI
        {
            bool exit = false;
            while (!exit)
            {

                Console.Clear();
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 살 수 있습니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{player.Gold} G");

                Console.WriteLine("\n[아이템 목록]");
                //아이템 목록 보여줌
                int index = 1;
                foreach (ITradable item in sellingItems)
                {
                    Console.Write($"- {index++}. ");
                    item.ShowInfo();
                }
                Console.WriteLine("\n0. 나가기");
                int playerInput = Utils.GetPlayerInput(true);

                if (playerInput > sellingItems.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    continue;
                }

                else if (playerInput == 0) //인풋 0 : 나가기
                {
                    exit = true;
                    break;
                }
                else
                {
                    ITradable selectedItem = sellingItems[playerInput - 1];   //선택된 아이템
                    if (!selectedItem.IsForSale)  //이미 판매된 아이템인 경우
                    {
                        Console.WriteLine("\n이미 판매된 아이템입니다.");
                        Utils.Pause(false);
                    }
                    //돈이 충분한 경우
                    else if (player.Gold >= selectedItem.Price)
                    {
                        //구매 확정 단계
                        Console.WriteLine($"\n아이템 가격 : {selectedItem.Price} G , 보유 골드 : {player.Gold} G");
                        Console.WriteLine("1. 구매");
                        Console.WriteLine("2. 다시 생각해본다");
                        switch (Utils.GetPlayerInput(true))
                        {
                            case 1:
                                player.BuyItem(selectedItem);
                                Console.WriteLine("\n구매 감사합니다!");
                                Utils.Pause(true);
                                break;
                            case 2:
                                break;
                            default: //잘못된 입력
                                Console.WriteLine("잘못된 입력입니다.");
                                Utils.Pause(false);
                                break;

                        }
                    }
                    //돈이 부족한 경우
                    else
                    {
                        Console.WriteLine("\n골드가 부족합니다.");
                        Utils.Pause(false);
                    }

                }

            }
        }

        public void SellItems() //아이템 판매 UI
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("선택한 아이템을 팔 수 있습니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{player.Gold} G");

                Console.WriteLine("\n[아이템 목록]");
                //플레이어가 보유중인 아이템 목록
                List<ITradable> items = player.Inventory.items;
                //아이템 목록 보여줌
                int index = 1;
                foreach (ITradable item in items)
                {
                    Console.Write($"- {index++}. ");
                    item.ShowInfo();
                }
                Console.WriteLine("\n0. 나가기");
                int playerInput = Utils.GetPlayerInput(true);

                if (playerInput > items.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    continue;
                }
                else if (playerInput == 0) //인풋 0 : 나가기
                {
                    exit = true;
                }
                else
                {
                    ITradable selectedItem = items[playerInput - 1];
                    //판매 시 80% 절감
                    int sellPrice = (int)(selectedItem.Price * 0.8);
                    //판매 확정 단계
                    Console.WriteLine($"\n{selectedItem.Name} : {sellPrice} G");
                    Console.WriteLine("1. 판매");
                    Console.WriteLine("2. 다시 생각해본다");
                    switch (Utils.GetPlayerInput(true))
                    {
                        case 1:
                            player.SellItem(selectedItem, sellPrice);
                            Console.WriteLine("\n판매 감사합니다!");
                            Utils.Pause(true);
                            break;
                        case 2:
                            break;
                        default: //잘못된 입력
                            Console.WriteLine("잘못된 입력입니다.");
                            Utils.Pause(false);
                            break;

                    }
                }
            }
        }

    }
}
