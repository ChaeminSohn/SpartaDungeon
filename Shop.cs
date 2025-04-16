using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpartaDungeon
{
    internal class Shop
    {
        List<ITradable> sellingItems;
        Player player;
        public Shop(Player player)
        {
            this.player = player;
            sellingItems = new List<ITradable>();

            foreach (ItemData itemData in ItemDatabase.Items)
            {
                sellingItems.Add(new Equipment(itemData));
            }
            // sellingItems.Add(new Equipment(ItemDatabase.BeninnerArmor));
            // sellingItems.Add(new Equipment(ItemDatabase.SteelArmor));
            // sellingItems.Add(new Equipment(ItemDatabase.SpartanArmor));
            // sellingItems.Add(new Equipment(ItemDatabase.RustySword));
            // sellingItems.Add(new Equipment(ItemDatabase.BronzeAxe));
            // sellingItems.Add(new Equipment(ItemDatabase.SpartanSpear));
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

                switch (Game.Instance.GetPlayerInput())
                {
                    case 0:
                        Game.Instance.Pause();
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
                        Game.Instance.Pause();
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
                int playerInput = Game.Instance.GetPlayerInput();

                if (playerInput > sellingItems.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("잘못된 입력입니다.");
                    Game.Instance.Pause();
                    continue;
                }

                else if (playerInput == 0) //인풋 0 : 나가기
                {
                    exit = true;
                    break;
                }
                else
                {
                    //돈이 충분한 경우
                    if (player.Gold >= sellingItems[playerInput - 1].Price)
                    {
                        //구매 확정 단계
                        Console.WriteLine($"\n아이템 가격 : {sellingItems[playerInput - 1].Price} G , 보유 골드 : {player.Gold} G");
                        Console.WriteLine("1. 구매");
                        Console.WriteLine("2. 다시 생각해본다");
                        switch (Game.Instance.GetPlayerInput())
                        {
                            case 1:
                                player.BuyItem(sellingItems[playerInput - 1]);
                                sellingItems.RemoveAt(playerInput - 1);
                                Console.WriteLine("\n구매 감사합니다!");
                                Game.Instance.Pause();
                                break;
                            case 2:
                                break;
                            default: //잘못된 입력
                                Console.WriteLine("잘못된 입력입니다.");
                                Game.Instance.Pause();
                                break;

                        }
                    }
                    //돈이 부족한 경우
                    else
                    {
                        Console.WriteLine("\n골드가 부족합니다.");
                        Game.Instance.Pause();
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
                List<ITradable> items = player.inventory.items;
                //아이템 목록 보여줌
                int index = 1;
                foreach (ITradable item in items)
                {
                    Console.Write($"- {index++}. ");
                    item.ShowInfo();
                }
                Console.WriteLine("\n0. 나가기");
                int playerInput = Game.Instance.GetPlayerInput();

                if (playerInput > items.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("잘못된 입력입니다.");
                    Game.Instance.Pause();
                    continue;
                }
                else if (playerInput == 0) //인풋 0 : 나가기
                {
                    exit = true;
                }
                else
                {
                    //판매 시 80% 절감
                    int sellPrice = (int)(items[playerInput - 1].Price * 0.8);
                    //판매 확정 단계
                    Console.WriteLine($"\n{items[playerInput - 1].Name} : {sellPrice} G");
                    Console.WriteLine("1. 판매");
                    Console.WriteLine("2. 다시 생각해본다");
                    switch (Game.Instance.GetPlayerInput())
                    {
                        case 1:
                            sellingItems.Add(items[playerInput - 1]);
                            player.SellItem(playerInput - 1, sellPrice);
                            Console.WriteLine("\n판매 감사합니다!");
                            Game.Instance.Pause();
                            break;
                        case 2:
                            break;
                        default: //잘못된 입력
                            Console.WriteLine("잘못된 입력입니다.");
                            Game.Instance.Pause();
                            break;

                    }
                }
            }
        }

    }
}
