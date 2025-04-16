using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Game
    {
        private static Game _instance;
        public static Game Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Game();
                }
                return _instance;
            }
        }

        ConsoleKeyInfo playerInput;
        Inventory inventory;
        Player player;
        Shop shop;
        Dungeon dungeon;
        bool isGameOver;

        public Game()
        {
            ItemDatabase.LoadItems();
            inventory = new Inventory(8);
            player = new Player("Chaemin", CharacterClass.Warrior, inventory);
            shop = new Shop(player);
            dungeon = new Dungeon();
            isGameOver = false;
        }


        public void GameStart()
        {
            while (!isGameOver)
            {
                TownAction();
            }
        }

        public void TownAction()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n\n");

            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장");
            Console.WriteLine("5. 휴식하기\n");


            switch (GetPlayerInput())
            {
                case 1:
                    player.ShowPlayerInfo();
                    Pause();
                    Console.Clear();
                    break;
                case 2:
                    inventory.ShowItems();
                    Pause();
                    Console.Clear();
                    break;
                case 3:
                    shop.ShowShop();
                    break;
                case 4:
                    DungeonAction();
                    break;
                case 5:
                    RestAction();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Pause();
                    break;
            }
        }

        void RestAction()
        {
            bool exit = false;
            while (!exit)
            {
                int health = player.CurrentHP;
                Console.Clear();
                Console.WriteLine("<휴식하기>");
                Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다." +
                    $" (보유 골드 : {player.Gold} G)");
                Console.WriteLine("\n1. 휴식하기");
                Console.WriteLine("0. 나가기");

                switch (GetPlayerInput())
                {
                    case 0:
                        exit = true;
                        Pause();
                        break;
                    case 1:
                        if (player.Gold >= 500)
                        {
                            player.RecoverHP(player.FullHP);
                            player.ChangeGold(-500);
                            Console.WriteLine("\n푹 쉬었습니다.");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Pause();
                        }
                        else
                        {
                            Console.WriteLine("\n골드가 부족합니다.");
                            Pause();
                        }
                        exit = true;
                        break;
                    default:
                        break;
                }
            }
        }

        void DungeonAction()
        {
            bool exit = false;
            while (!exit)
            {
                int health = player.CurrentHP;
                int gold = player.Gold;
                Console.Clear();
                Console.WriteLine("<던전 입장>");
                Console.WriteLine("도전할 던전의 난이도를 선택하세요.");
                Console.WriteLine($"\n1. 쉬운 던전     | 방어력 {dungeon.DefenseLevel[0]} 이상 권장");
                Console.WriteLine($"2. 쉬운 던전     | 방어력 {dungeon.DefenseLevel[1]} 이상 권장");
                Console.WriteLine($"3. 어려운 던전    | 방어력 {dungeon.DefenseLevel[2]} 이상 권장");
                Console.WriteLine("0. 나가기");

                switch (GetPlayerInput())
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        if (dungeon.EnterDungeon(player, 1))
                        {   //던전 성공 시
                            Console.Clear();
                            Console.WriteLine("<던전 클리어>");
                            Console.WriteLine("축하합니다!");
                            Console.WriteLine("쉬운 던전을 클리어 하셨습니다.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Console.WriteLine($"Gold {gold} -> {player.Gold}");
                            Pause();
                        }
                        else    //던전 실패 시
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Pause();
                        }
                        exit = true;
                        break;
                    case 2:
                        if (dungeon.EnterDungeon(player, 2))
                        {   //던전 성공 시
                            Console.Clear();
                            Console.WriteLine("<던전 클리어>");
                            Console.WriteLine("축하합니다!");
                            Console.WriteLine("보통 던전을 클리어 하셨습니다.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Console.WriteLine($"Gold {gold} -> {player.Gold}");
                            Pause();
                        }
                        else    //던전 실패 시
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Pause();
                        }
                        exit = true;
                        break;
                    case 3:
                        if (dungeon.EnterDungeon(player, 3))
                        {   //던전 성공 시
                            Console.Clear();
                            Console.WriteLine("<던전 클리어>");
                            Console.WriteLine("축하합니다!");
                            Console.WriteLine("어려운 던전을 클리어 하셨습니다.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Console.WriteLine($"Gold {gold} -> {player.Gold}");
                            Pause();
                        }
                        else    //던전 실패 시
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Pause();
                        }
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Pause();
                        break;
                }
            }
            if (player.CurrentHP <= 0)
            {
                isGameOver = true;
            }
        }
        public int GetPlayerInput()  //플레이어의 입력을 숫자로 반환
        {
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            playerInput = Console.ReadKey();
            int selectedIndex = -1;

            //0~9 숫자가 입력된 경우
            if (playerInput.Key >= ConsoleKey.D0 && playerInput.Key <= ConsoleKey.D9)
            {
                selectedIndex = (int)playerInput.Key - (int)ConsoleKey.D0;
            }
            else if (playerInput.Key >= ConsoleKey.NumPad0 && playerInput.Key <= ConsoleKey.NumPad9)
            {
                selectedIndex = (int)playerInput.Key - (int)ConsoleKey.NumPad0;
            }
            return selectedIndex; //-1이 반환될 경우, 잘못된 입력
        }

        public void Pause()
        {
            Console.WriteLine("\n아무 키나 누르면 계속합니다...");
            Console.ReadKey();
        }
    }
}
