using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Game
    {
        ItemConfig itemConfig;
        Inventory inventory;
        Player player;
        Shop shop;
        Dungeon dungeon;
        bool isGameOver;

        string savePath = "saveData.json"; //저장 파일 경로
        public Game()
        {
            if (!ConfigLoader.TryLoad<ItemConfig>("equipments_config.json", out var config))
            {
                Console.WriteLine("장비 데이터를 불러오지 못했습니다.");
            }
            itemConfig = config;
        }

        void NewGameSetting()
        {
            bool exit = false;
            string playerName = string.Empty;
            Job playerJob = default;

            while (!exit)   //이름 설정
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 걸 환영합니다!");
                Console.WriteLine("플레이어 이름을 입력해주세요.\n");
                string input = Console.ReadLine();

                // 입력이 null이거나 공백인 경우
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("이름은 비워둘 수 없습니다. 다시 입력해주세요.");
                    Utils.Pause(false);
                    continue;
                }

                // 너무 짧거나 긴 이름인 경우
                if (input.Length < 2 || input.Length > 12)
                {
                    Console.WriteLine("이름은 2자 이상 12자 이하여야 합니다.");
                    Utils.Pause(false);
                    continue;
                }
                playerName = input;
                exit = true;
            }

            exit = false;

            while (!exit)   //직업 설정
            {
                Console.Clear();
                Console.WriteLine("직업을 선택하세요.\n");
                foreach (Job job in Enum.GetValues(typeof(Job)))
                {
                    Console.WriteLine($"{(int)job + 1}. {job}");
                }

                Console.WriteLine();

                switch (Utils.GetPlayerInput(false))
                {
                    case 1:
                        playerJob = Job.Warrior;
                        exit = true;
                        break;
                    case 2:
                        playerJob = Job.Mage;
                        exit = true;
                        break;
                    case 3:
                        playerJob = Job.Archer;
                        exit = true;
                        break;
                    default:
                        break;
                }
            }

            inventory = new Inventory(8);
            player = new Player(playerName, playerJob, inventory);
            shop = new Shop(player, itemConfig);
            dungeon = new Dungeon();
            isGameOver = false;
        }

        public void GameStart()
        {
            if (File.Exists(savePath))  //세이브 파일이 존재할 경우
            {
                Console.WriteLine("세이브 파일이 존재합니다. 이어서 게임을 시작합니다.");
                Utils.Pause(false);
            }
            else    //세이브 파일이 존재하지 않을 경우
            {
                Console.WriteLine("세이브 파일이 없습니다. 새로운 게임을 시작합니다..");
                NewGameSetting();
                Utils.Pause(false);
            }

            while (!isGameOver)
            {
                TownAction();
            }
            GameOver();
        }

        void SaveData()
        {
            string json = JsonSerializer.Serialize(player);
        }


        void LoadData()
        {

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


            switch (Utils.GetPlayerInput(true))
            {
                case 1:
                    player.ShowPlayerInfo();
                    Utils.Pause(true);
                    Console.Clear();
                    break;
                case 2:
                    inventory.ShowItems();
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
                    Utils.Pause(false);
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

                switch (Utils.GetPlayerInput(true))
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        if (player.Gold >= 500)
                        {
                            player.RecoverHP(player.FullHP);
                            player.ChangeGold(-500);
                            Console.WriteLine("\n푹 쉬었습니다.");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Utils.Pause(true);
                        }
                        else
                        {
                            Console.WriteLine("\n골드가 부족합니다.");
                            Utils.Pause(false);
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
                Console.WriteLine($"2. 보통 던전     | 방어력 {dungeon.DefenseLevel[1]} 이상 권장");
                Console.WriteLine($"3. 어려운 던전   | 방어력 {dungeon.DefenseLevel[2]} 이상 권장");
                Console.WriteLine("0. 나가기");

                switch (Utils.GetPlayerInput(true))
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
                            Utils.Pause(true);
                            player.GetEXP(1);   //쉬움 던전 : 경험치 1 획득
                        }
                        else    //던전 실패 시
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Utils.Pause(true);
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
                            Utils.Pause(true);
                            player.GetEXP(2);   //쉬움 던전 : 경험치 2 획득
                        }
                        else    //던전 실패 시
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Utils.Pause(true);
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
                            Utils.Pause(true);
                            player.GetEXP(3);   //쉬움 던전 : 경험치 3 획득
                        }
                        else    //던전 실패 시
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Utils.Pause(true);
                        }
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
            if (player.CurrentHP <= 0)
            {
                isGameOver = true;
                GameOver();
            }
        }

        void GameOver()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("You Died");
                Console.WriteLine("\n1. 다시 시작하기");
                Console.WriteLine("\n2. 게임 종료");
                switch (Utils.GetPlayerInput(true))
                {
                    case 1:
                        exit = true;
                        GameStart();
                        break;
                    case 2:
                        exit = true;
                        break;
                    default:
                        break;
                }
            }


        }

        // public int GetPlayerInput()  //플레이어의 입력을 숫자로 반환
        // {
        //     Console.WriteLine("\n원하시는 행동을 입력해주세요.");
        //     Console.Write(">>");
        //     playerInput = Console.ReadKey();
        //     int selectedIndex = -1;

        //     //0~9 숫자가 입력된 경우
        //     if (playerInput.Key >= ConsoleKey.D0 && playerInput.Key <= ConsoleKey.D9)
        //     {
        //         selectedIndex = (int)playerInput.Key - (int)ConsoleKey.D0;
        //     }
        //     else if (playerInput.Key >= ConsoleKey.NumPad0 && playerInput.Key <= ConsoleKey.NumPad9)
        //     {
        //         selectedIndex = (int)playerInput.Key - (int)ConsoleKey.NumPad0;
        //     }
        //     return selectedIndex; //-1이 반환될 경우, 잘못된 입력
        // }


        // public void Pause()
        // {
        //     Console.WriteLine("\n아무 키나 누르면 계속합니다...");
        //     Console.ReadKey();
        // }
    }
}
