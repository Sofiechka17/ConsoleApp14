namespace pz_16
{
    internal class Program
    {
        //Объявление глобальных переменных
        static int mapSize = 25;
        static char[,] map = new char[mapSize, mapSize];
        static int playerY = mapSize / 2;
        static int playerX = mapSize / 2;
        static byte enemies = 10;
        static byte buffs = 5;
        static byte health = 5;
        static int PlayerHp = 50;
        static int EnemiesHp = 30;
        static int PlayerPower = 10;
        static int EnemiesPower = 5;
        static int NumSteps = 0;
        static int buffcount = 0;
        static int playerOldY;
        static int playerOldX;
        static int buffcounthelp = 0;
        static int enemycount = 0;

        enum Direction { Left, Right, Up, Down }

        static void Main(string[] args)//Основной метод
        {
            StartGame();//вывод начального экрана
            Move();//метод перемещения

        }

        static void GenerationMap()//Метод генерации карты
        {
            Random random = new Random();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize - 1; j++)
                {
                    map[i, j] = '_';

                }
            }

            map[playerX, playerY] = 'P';//Присвоение начальных координат
            int x;
            int y;

            while (enemies > 0)//Цикл создания врагов
            {
                x = random.Next(mapSize);
                y = random.Next(1, mapSize);
                if (map[x, y] == '_')
                {
                    map[x, y] = 'E';
                    enemies--;


                }
            }

            while (buffs > 0)//Цикл создания баффов
            {
                x = random.Next(mapSize);
                y = random.Next(1, mapSize);
                if (map[x, y] == '_')
                {
                    map[x, y] = 'B';
                    buffs--;
                }
            }

            while (health > 0)//Цикл создания аптечек
            {
                x = random.Next(mapSize);
                y = random.Next(1, mapSize);
                if (map[x, y] == '_')
                {
                    map[x, y] = 'H';
                    health--;
                }
            }


            UpdateMap();//обновление карты
        }

        static void Move()//метод передвижения
        {
            while (true)
            {
                if (PlayerHp > 0 && enemycount != 10)
                {
                    Console.SetCursorPosition(27, 0);
                    if (PlayerHp > 5)
                        Console.Write($"Здоровье игрока: {PlayerHp}");//вывод здоровья игрока
                    else
                        Console.Write($"Здоровье игрока: {"0" + PlayerHp}");
                    Console.SetCursorPosition(27, 1);
                    Console.Write($"Сила игрока: {PlayerPower}");//вывод силы удара игрока
                    Console.SetCursorPosition(27, 2);
                    Console.Write($"Количество шагов: {NumSteps}");//вывод общего количества шагов
                }
                playerOldX = playerX;//Присвоение значения старым координатам
                playerOldY = playerY;
                switch (Console.ReadKey().Key)//Свитч на изменение координат
                {
                    case ConsoleKey.UpArrow:
                        if (playerY == 0)
                            playerY = playerY;
                        else
                            playerY--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (playerY == 24)
                            playerY = playerY;
                        else
                            playerY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (playerX == 0)
                            playerX = playerX;
                        else
                            playerX--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (playerX == 24)
                            playerX = playerX;
                        else
                            playerX++;
                        break;
                    case ConsoleKey.Escape:
                        Escape(); //метод выхода из игры
                        break;
                }

                Health();//метод лечения
                Fight();//метод боевки
                Buff();//метод баффа
                buffcount++;//Подсчет длительности баффа

                if (buffcount == buffcounthelp + 21)//Проверка на завершении баффа
                {
                    PlayerPower = 10;
                }
                Console.CursorVisible = false;
                map[playerOldY, playerOldX] = '_';
                Console.SetCursorPosition(playerOldX, playerOldY);
                Console.Write('_');
                map[playerY, playerX] = 'P';
                Console.SetCursorPosition(playerX, playerY);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write('P');
                Console.ResetColor();

                if (playerX == playerOldX && playerY == playerOldY)
                    NumSteps = NumSteps;
                else
                    NumSteps++;//Счет шагов

                if (PlayerHp <= 0)
                    Fail(); //вывод экрана поражения

                if (enemycount == 10)
                    Win(); //вывод экрана выигрыша

            }
        }


        static void UpdateMap()
        {
            Console.Clear();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {

                    switch (map[i, j])//раскраска объектов
                    {
                        case 'H':
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(map[i, j]);
                            Console.ResetColor();
                            break;
                        case 'B':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(map[i, j]);
                            Console.ResetColor();
                            break;
                        case 'E':
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(map[i, j]);
                            Console.ResetColor();
                            break;
                        case 'P':
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(map[i, j]);
                            Console.ResetColor();
                            break;
                        default:
                            Console.Write(map[i, j]);
                            break;
                    }
                }
                Console.WriteLine(map[i, 0]);
            }
        }
        static void Fight()//метод боя
        {
            if (map[playerY, playerX] == 'E')
            {
                PlayerHp -= EnemiesPower;
                EnemiesHp -= PlayerPower;
                if (EnemiesHp > 0)
                {
                    Console.SetCursorPosition(playerOldX, playerOldY);
                    map[playerOldY, playerOldX] = 'E';
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('E');
                    Console.ResetColor();
                    playerOldX = playerX;
                    playerOldY = playerY;
                }
                else
                {
                    EnemiesHp = 30;
                    enemycount++;
                }
            }
        }

        static void Health()//Метод лечения
        {
            if (map[playerY, playerX] == 'H')
            {
                PlayerHp = 50;
            }
        }

        static void Buff() //Метод баффа
        {

            if (map[playerY, playerX] == 'B')
            {
                buffcounthelp = NumSteps;
                PlayerPower = 20;
            }
        }
        static void StartGame()// метод вывода начального экрана
        {
            Console.SetCursorPosition(37, 22);
            Console.WriteLine("W - начать новую игру");
            Console.SetCursorPosition(37, 23);
            Console.WriteLine("L - загрузить последнее сохранение");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.W:
                    GenerationMap();
                    break;
                case ConsoleKey.L:
                    Console.Clear();
                    LoadGame();
                    UpdateMap();
                    break;
            }
        }
        static void Fail()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Вы проиграли");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Количество шагов: " + NumSteps);
        }

        static void Win()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Вы выиграли");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Количество шагов: " + NumSteps);
        }

        static void Escape()//метод выхода из игры
        {

            Console.Clear();
            Console.SetCursorPosition(40, 22);
            Console.WriteLine("RR - вернуться в игру");
            Console.SetCursorPosition(40, 23);
            Console.WriteLine("E - сохранить и выйти");
            Console.SetCursorPosition(40, 24);
            Console.WriteLine("S - выйти без сохранения");
            Console.CursorVisible = false;

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.R:
                    UpdateMap();
                    break;
                case ConsoleKey.E:
                    Console.Clear();
                    SaveProgress();

                    break;
                case ConsoleKey.S:
                    Environment.Exit(0);
                    break;
            }
        }

        static void SaveProgress()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("save.txt"))
                { //сохранение данных в файл
                    writer.WriteLine(playerX);
                    writer.WriteLine(playerY);
                    writer.WriteLine(playerOldY);
                    writer.WriteLine(playerOldX);
                    writer.WriteLine(PlayerHp);
                    writer.WriteLine(EnemiesHp);
                    writer.WriteLine(PlayerPower);
                    writer.WriteLine(EnemiesPower);
                    writer.WriteLine(NumSteps);
                    writer.WriteLine(buffcount);
                    writer.WriteLine(buffcounthelp);
                    writer.WriteLine(enemycount);

                    for (int i = 0; i < mapSize; i++)//запись координат объктов на карте в файл
                    {
                        for (int j = 0; j < mapSize; j++)
                        {
                            writer.Write(map[i, j]);
                        }
                        writer.WriteLine();
                    }
                }
                Console.SetCursorPosition(40, 10);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Игра сохранена");
                Console.ResetColor();
                Console.ReadKey();
                StartGame();
            }
            catch (Exception ex)
            {
                Console.SetCursorPosition(mapSize + 48, 11);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка при сохранении игры: {ex.Message}");
                Console.ResetColor();
                StartGame();
            }

        }

        static void LoadGame() // загрузка сохраненной игры
        {
            using (StreamReader reader = new StreamReader("save.txt"))
            {
                playerX = int.Parse(reader.ReadLine());
                playerY = int.Parse(reader.ReadLine());
                playerOldY = int.Parse(reader.ReadLine());
                playerOldX = int.Parse(reader.ReadLine());
                PlayerHp = int.Parse(reader.ReadLine());
                EnemiesHp = int.Parse(reader.ReadLine());
                PlayerPower = int.Parse(reader.ReadLine());
                EnemiesPower = int.Parse(reader.ReadLine());
                NumSteps = int.Parse(reader.ReadLine());
                buffcount = int.Parse(reader.ReadLine());
                buffcounthelp = int.Parse(reader.ReadLine());
                enemycount = int.Parse(reader.ReadLine());
                for (int i = 0; i < mapSize; i++)
                {
                    string save = reader.ReadLine();
                    for (int j = 0; j < mapSize; j++)
                    {
                        map[i, j] = save[j];
                    }
                }
            }
        }
    }
}