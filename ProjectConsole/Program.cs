using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ProjectConsole
{
    internal class Program
    {
        struct Position
        {
            public int x;
            public int y;
        }

        enum MoveDirection
        {
            Up, Down, Left, Right
        }

        enum GameState {
            Alive, Dead, Clear, Pause
        }

        //static async Task Main(string[] args)
        static void Main(string[] args)
        {
            GameState gameState = GameState.Alive;

            /* 해결 과제
             * 1. 남은 목표물 수 표시(업데이트) - 해결
             * 2. 총알이 발사되고 있는 동안 사용자의 입력 제한(오류) - 해결
             * 3. 남은 시간 표시(업데이트/가능여부 확인필요) - 미해결(향후 확인 필요)
             * 4. 목표물 파괴시 비프음 추가(업데이트) - 해결
             * 5. 시작화면 및 종료화면 재시작 기능 추가(업데이트) - 해결
             */

            Position playerPos;
            char[,] map;
            int targetCount;
            bool isGameStart;

            isGameStart = GameStart();
            if (isGameStart) {
                while (gameState == GameState.Alive) {
                    targetCount = GameSetting(out playerPos, out map);
                    Console.Clear();
                    //await countTime(gameState);
                    while (gameState == GameState.Alive)
                    {
                        ConsoleKey key = ConsoleKey.None;
                        RenderScreen(playerPos, map, targetCount);
                        ClearInputBuffer();
                        key = GetUserInput();
                        gameState = StatusUpdate(key, ref playerPos, map, ref targetCount);
                    }
                    gameState = GameEnd(gameState);
                }
            }
        }

        //static async Task countTime(GameState gameState) {
        //    int time = 0;
        //    while (gameState== GameState.Alive) {
        //        PrintTimeBoard(time);
        //        time++;
        //        await Task.Delay(1000);
        //    }
        //}

        //static void PrintTimeBoard(int time) {
        //    Console.SetCursorPosition(20, 10);

        //    string[] scoreBoard = new string[] {
        //        "===============",
        //        "                   ",
        //        "   TIME : "+time.ToString("D2"),
        //        "                   ",
        //        "==============="
        //    };
        //    for (int y = 0; y < scoreBoard.GetLength(0); y++)
        //    {
        //        Console.SetCursorPosition(20, 10 + y);
        //        Console.Write(scoreBoard[y]);
        //    }
        //}

        // 게임 초기 세팅
        static int GameSetting(out Position playerPos, out char[,] map)
        {
            // 게임 설정
            Console.CursorVisible = false;

            // 플레이어 위치 설정
            playerPos.x = 7;
            playerPos.y = 14;

            // 맵 설정
            map = new char[,]
            {
                { '▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩' },
                { '▩','A','A','A','A','A',' ',' ','A','A','A','A','A','A','A','▩' },
                { '▩','A','A','A','A','A','A',' ','A','A','A','A','A','A','A','▩' },
                { '▩','A',' ','A','A','A','A',' ',' ','A','A','A','A','A','A','▩' },
                { '▩',' ',' ','A','A','A','A','A',' ',' ','A',' ','A','A','A','▩' },
                { '▩',' ',' ',' ','A','A','A','A',' ',' ',' ',' ',' ','A','A','▩' },
                { '▩',' ',' ',' ',' ','A','A','A','A',' ',' ',' ',' ','A',' ','▩' },
                { '▩',' ',' ',' ',' ',' ','A','A','A',' ',' ',' ',' ','A',' ','▩' },
                { '▩',' ',' ',' ',' ',' ','A','A',' ',' ',' ',' ',' ',' ',' ','▩' },
                { '▩',' ',' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ',' ',' ','▩' },
                { '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                { '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                { '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                { '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                { '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                { '▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩' }
                // test map
                //{ '▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','A','A','A','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','A','A','A','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','A','A','A','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','A','A','A','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','A','A','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','A',' ','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','A',' ','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                //{ '▩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','▩' },
                //{ '▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩','▩' }
            };

            return GetTargetCount(map);
        }

        // 게임 시작 화면
        static bool GameStart() {
            bool isStart = false;
            bool isCorrectInput = false;

            Console.WriteLine("=========================");
            Console.WriteLine("                         ");
            Console.WriteLine("                         ");
            Console.WriteLine("    Very Easy Galaga     ");
            Console.WriteLine("                         ");
            Console.WriteLine("                         ");
            Console.WriteLine("    Space: Game Start    ");
            Console.WriteLine("     Esc: Game Exit      ");
            Console.WriteLine("                         ");
            Console.WriteLine("                         ");
            Console.WriteLine("=========================");

            while (!isCorrectInput) {
                ConsoleKey userPress = Console.ReadKey(true).Key;
                if (userPress == ConsoleKey.Spacebar)
                {
                    isStart = true;
                    isCorrectInput = true;
                }
                else if (userPress == ConsoleKey.Escape)
                {
                    isStart = false;
                    isCorrectInput = true;
                }
            }
            Console.Clear();
            return isStart;
        }

        static int GetTargetCount(char[,] map) {
            int targetCount = 0;
            foreach (char tile in map)
            {
                if (tile == 'A')
                {
                    targetCount++;
                }
            }
            return targetCount;
        }

        // 화면 업데이트
        static void RenderScreen(Position playerPos, char[,] map, int targetCount)
        {
            Console.SetCursorPosition(0, 0);
            PrintMap(map);
            PrintScoreBoard(targetCount);
            PrintPlayer(playerPos);
        }

        // 맵 출력
        static void PrintMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {   
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Console.Write(map[y, x]);
                }
                Console.WriteLine();
            }
        }

        // 플레이어 출력
        static void PrintPlayer(Position playerPos)
        {
            Console.SetCursorPosition(playerPos.x, playerPos.y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('▲');
            Console.ResetColor();
        }

        static void PrintScoreBoard(int targetCount) {
            string[] scoreBoard = new string[] {
                "===============",
                "                   ",
                "   LEFT : "+targetCount.ToString("D2"),
                "                   ",
                "==============="
            }; 
            for (int y = 0; y < scoreBoard.GetLength(0); y++)
            {
                Console.SetCursorPosition(20, 1+y);
                Console.Write(scoreBoard[y]);
            }
        }

        // 사용자 입력 저장
        static ConsoleKey GetUserInput()
        {
            return Console.ReadKey(true).Key;
        }

        // 게임 상태 업데이트
        static GameState StatusUpdate(ConsoleKey key, ref Position playerPos, char[,] map, ref int targetCount)
        {
            GameState state = GameState.Alive;
            try
            {
                switch (key)
                {
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        ref char targetPosLeft = ref map[playerPos.y, playerPos.x - 1];
                        //ref char boxTargetPosLeft = ref map[playerPos.y, playerPos.x - 2];
                        state = PlayerMove(MoveDirection.Left, ref targetPosLeft, ref playerPos);
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        ref char targetPosRight = ref map[playerPos.y, playerPos.x + 1];
                        //ref char boxTargetPosRight = ref map[playerPos.y, playerPos.x + 2];
                        state = PlayerMove(MoveDirection.Right, ref targetPosRight, ref playerPos);
                        break;
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        ref char targetPosUp = ref map[playerPos.y - 1, playerPos.x];
                        //ref char boxTargetPosUp = ref map[playerPos.y - 2, playerPos.x];
                        state = PlayerMove(MoveDirection.Up, ref targetPosUp, ref playerPos);
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        ref char targetPosDown = ref map[playerPos.y + 1, playerPos.x];
                        //ref char boxTargetPosDown = ref map[playerPos.y + 2, playerPos.x];
                        state = PlayerMove(MoveDirection.Down, ref targetPosDown, ref playerPos);
                        break;
                    case ConsoleKey.Spacebar:
                        BulletFire(playerPos, map);
                        break;
                }
            }
            catch (Exception e) {
                // 사용자가 위쪽 혹은 왼쪽 벽밖으로 이동하려고 시도할시 boxposition에서 IndexOutOfRangeException 발생함
            }
            if (state!= GameState.Dead && IsClear(map, ref targetCount)) {
                state = GameState.Clear;
            }
            return state;
        }

        // 플레이어 이동
        static GameState PlayerMove(MoveDirection direction, ref char targetPos, ref Position playerPos)
        {
            bool canMove, isDead = false;
            if (targetPos == 'A')
            {
                canMove = true;
                isDead = true;
            }
            else if (targetPos == ' ')
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }

            if (canMove)
            {
                switch (direction)
                {
                    case MoveDirection.Up:
                        playerPos.y--;
                        break;
                    case MoveDirection.Down:
                        playerPos.y++;
                        break;
                    case MoveDirection.Left:
                        playerPos.x--;
                        break;
                    case MoveDirection.Right:
                        playerPos.x++;
                        break;
                    default:
                        break;
                }
            }
            return (isDead) ? GameState.Dead : GameState.Alive;
        }

        // 총알 발사
        static void BulletFire(Position playerPos, char[,] map) {
            bool isBulletBroken = false;
            Position bulletPos;
            bulletPos.x = playerPos.x;
            bulletPos.y = playerPos.y - 1;

            Console.SetCursorPosition(bulletPos.x, bulletPos.y);
            Console.Write('*');
            Thread.Sleep(200);
            isBulletBroken = CheckBulletState(bulletPos.x, bulletPos.y, map, true);

            while (!isBulletBroken) {
                isBulletBroken = CheckBulletState(bulletPos.x, bulletPos.y-1, map);
                bulletPos.y--;
                Thread.Sleep(200);
            }
        }

        // 총알 상태 및 출력 업데이트(총알 파괴 여부)
        static bool CheckBulletState(int x, int y, char[,] map, bool isFirst=false)
        {
            if (!isFirst) {
                Console.SetCursorPosition(x, y + 1);
                Console.Write(' ');
            }            

            if (map[y, x] == 'A')
            {
                Console.Beep();
                Console.SetCursorPosition(x, y);
                Console.Write(' ');

                map[y, x] = ' ';

                return true;
            }
            else if (map[y, x] == '▩')
            {
                return true;
            }
            else
            {
                Console.SetCursorPosition(x, y);
                Console.Write('*');

                return false;
            }
        }

        // 클리어 여부 확인
        static bool IsClear(char[,] map, ref int targetCount)
        {
            targetCount = GetTargetCount(map);
            
            return (targetCount == 0);
        }

        // 게임 종료 실행
        static GameState GameEnd(GameState gameState)
        {
            bool isStart = false;
            if (gameState == GameState.Dead)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("=========================");
                Console.WriteLine("                         ");
                Console.WriteLine("                         ");
                Console.WriteLine("       GAME OVER         ");
                Console.WriteLine("                         ");
                Console.WriteLine("                         ");
                Console.WriteLine("     Space: Restart      ");
                Console.WriteLine("        Esc: Exit        ");
                Console.WriteLine("                         ");
                Console.WriteLine("                         ");
                Console.WriteLine("=========================");

                bool isCorrectInput = false;
                while (!isCorrectInput)
                {
                    ConsoleKey userPress = Console.ReadKey(true).Key;
                    if (userPress == ConsoleKey.Spacebar)
                    {
                        isStart = true;
                        isCorrectInput = true;
                    }
                    else if (userPress == ConsoleKey.Escape)
                    {
                        isStart = false;
                        isCorrectInput = true;
                    }
                }
            }
            else if (gameState == GameState.Clear)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("=========================");
                Console.WriteLine("                         ");
                Console.WriteLine("                         ");
                Console.WriteLine("      GAME CLEAR!!!      ");
                Console.WriteLine("                         ");
                Console.WriteLine("                         ");
                Console.WriteLine("     Space: Restart      ");
                Console.WriteLine("        Esc: Exit        ");
                Console.WriteLine("                         ");
                Console.WriteLine("                         ");
                Console.WriteLine("=========================");

                bool isCorrectInput = false;
                while (!isCorrectInput)
                {
                    ConsoleKey userPress = Console.ReadKey(true).Key;
                    if (userPress == ConsoleKey.Spacebar)
                    {
                        isStart = true;
                        isCorrectInput = true;
                    }
                    else if (userPress == ConsoleKey.Escape)
                    {
                        isStart = false;
                        isCorrectInput = true;
                    }
                }
            }
            else 
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("=========================");
                Console.WriteLine("                         ");
                Console.WriteLine("                         ");
                Console.WriteLine("         Oops...         ");
                Console.WriteLine("   Something is wrong.   ");
                Console.WriteLine("                         ");
                Console.WriteLine("        Any press        ");
                Console.WriteLine("      to game close      ");
                Console.WriteLine("                         ");
                Console.WriteLine("                         ");
                Console.WriteLine("=========================");

                ConsoleKey userPress = Console.ReadKey(true).Key;
                isStart = true;
            }
            if (isStart)
            {
                return GameState.Alive;
            }
            else 
            {
                return GameState.Dead;
            }
        }

        static void ClearInputBuffer() {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}
