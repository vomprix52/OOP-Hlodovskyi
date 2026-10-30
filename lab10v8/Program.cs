using System.Diagnostics;

namespace Lab10
{
    public class Program
    {
        // Скільки разів повторюємо кожен запит для вимірювання середнього часу
        private const int Iterations = 1000;

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Тестові дані
            List<Team> teams = CreateTeams();
            List<Player> players = CreatePlayers();
            Console.WriteLine($"Команд: {teams.Count}, гравців: {players.Count}");
            Console.WriteLine($"Час = середнє значення з {Iterations} запусків, у мікросекундах (мкс)");
            Console.WriteLine();

            // 1. Гравці до 25 років з команди "Lions" (Join + Where)
            Console.WriteLine("1. Гравці віком до 25 років з команди \"Lions\"");

            // Синтаксис запитів
            var youngLionsQuery =
                from p in players
                join t in teams on p.TeamId equals t.Id
                where t.Name == "Lions" && p.Age < 25
                select p;

            // Синтаксис методів
            var youngLionsMethod = players
                .Join(teams, p => p.TeamId, t => t.Id, (p, t) => new { Player = p, Team = t })
                .Where(x => x.Team.Name == "Lions" && x.Player.Age < 25)
                .Select(x => x.Player);

            foreach (Player player in youngLionsQuery)
            {
                Console.WriteLine($"  {player}");
            }
            double time1Query = Measure(() => youngLionsQuery.ToList());
            double time1Method = Measure(() => youngLionsMethod.ToList());
            PrintTimes(time1Query, time1Method);

            // 2. Гравці разом з назвою команди (Join)
            Console.WriteLine("2. Гравці та їхні команди");

            // Синтаксис запитів
            var playersWithTeamsQuery =
                from p in players
                join t in teams on p.TeamId equals t.Id
                select new { PlayerName = p.Name, TeamName = t.Name };

            // Синтаксис методів
            var playersWithTeamsMethod = players
                .Join(teams, p => p.TeamId, t => t.Id, (p, t) => new { PlayerName = p.Name, TeamName = t.Name });

            foreach (var item in playersWithTeamsMethod)
            {
                Console.WriteLine($"  {item.PlayerName}: {item.TeamName}");
            }
            double time2Query = Measure(() => playersWithTeamsQuery.ToList());
            double time2Method = Measure(() => playersWithTeamsMethod.ToList());
            PrintTimes(time2Query, time2Method);

            // 3. Середній вік гравців у кожній команді (GroupJoin + Average)
            // GroupJoin повертає кожну команду, навіть якщо в неї немає гравців
            Console.WriteLine("3. Середній вік гравців у кожній команді");

            // Синтаксис запитів: join ... into
            var averageAgeQuery =
                from t in teams
                join p in players on t.Id equals p.TeamId into teamPlayers
                select new
                {
                    TeamName = t.Name,
                    Count = teamPlayers.Count(),
                    AverageAge = teamPlayers.Any() ? teamPlayers.Average(p => p.Age) : (double?)null
                };

            // Синтаксис методів: GroupJoin
            var averageAgeMethod = teams
                .GroupJoin(players, t => t.Id, p => p.TeamId, (t, teamPlayers) => new
                {
                    TeamName = t.Name,
                    Count = teamPlayers.Count(),
                    AverageAge = teamPlayers.Any() ? teamPlayers.Average(p => p.Age) : (double?)null
                });

            foreach (var item in averageAgeQuery)
            {
                if (item.AverageAge == null)
                {
                    Console.WriteLine($"  {item.TeamName}: немає гравців");
                }
                else
                {
                    Console.WriteLine($"  {item.TeamName}: {item.AverageAge:F1} р. (гравців: {item.Count})");
                }
            }
            double time3Query = Measure(() => averageAgeQuery.ToList());
            double time3Method = Measure(() => averageAgeMethod.ToList());
            PrintTimes(time3Query, time3Method);

            // 4. Найстарші гравці (Max + Where)
            // Спершу знаходимо максимальний вік, потім усіх гравців з таким віком
            Console.WriteLine("4. Найстарші гравці");

            // Синтаксис запитів
            List<Player> FindOldestQuery()
            {
                int maxAge = (from p in players select p.Age).Max();
                return (from p in players
                        where p.Age == maxAge
                        select p).ToList();
            }

            // Синтаксис методів
            List<Player> FindOldestMethod()
            {
                int maxAge = players.Max(p => p.Age);
                return players.Where(p => p.Age == maxAge).ToList();
            }

            foreach (Player player in FindOldestMethod())
            {
                Console.WriteLine($"  {player}");
            }
            double time4Query = Measure(() => FindOldestQuery());
            double time4Method = Measure(() => FindOldestMethod());
            PrintTimes(time4Query, time4Method);

            // 5. Порівняння часу всіх запитів
            Console.WriteLine("5. Порівняння часу (синтаксис запитів / синтаксис методів)");
            Console.WriteLine($"  Запит 1 (Join + Where):        {time1Query:F2} / {time1Method:F2} мкс");
            Console.WriteLine($"  Запит 2 (Join):                {time2Query:F2} / {time2Method:F2} мкс");
            Console.WriteLine($"  Запит 3 (GroupJoin + Average): {time3Query:F2} / {time3Method:F2} мкс");
            Console.WriteLine($"  Запит 4 (Max + Where):         {time4Query:F2} / {time4Method:F2} мкс");
        }

        // Вимірює середній час виконання запиту в мікросекундах.
        // Усередині action має бути ToList(), інакше через відкладене виконання запит не виконається.
        private static double Measure(Action action)
        {
            // Прогрівальний запуск: перший виклик повільніший через JIT-компіляцію
            action();

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < Iterations; i++)
            {
                action();
            }
            stopwatch.Stop();

            return stopwatch.Elapsed.TotalMilliseconds * 1000 / Iterations;
        }

        // Виводить час обох варіантів запиту
        private static void PrintTimes(double queryTime, double methodTime)
        {
            Console.WriteLine($"Час (синтаксис запитів): {queryTime:F2} мкс");
            Console.WriteLine($"Час (синтаксис методів): {methodTime:F2} мкс");
            Console.WriteLine();
        }

        // 10 команд. У Falcons навмисно немає гравців, щоб показати роботу GroupJoin.
        private static List<Team> CreateTeams()
        {
            return new List<Team>
            {
                new Team(1, "Lions", "Київ"),
                new Team(2, "Tigers", "Львів"),
                new Team(3, "Eagles", "Одеса"),
                new Team(4, "Wolves", "Харків"),
                new Team(5, "Bears", "Дніпро"),
                new Team(6, "Sharks", "Запоріжжя"),
                new Team(7, "Falcons", "Вінниця"),
                new Team(8, "Panthers", "Полтава"),
                new Team(9, "Hawks", "Рівне"),
                new Team(10, "Bulls", "Луцьк")
            };
        }

        // 25 гравців. Двоє мають однаковий максимальний вік (36), щоб показати пошук усіх найстарших.
        private static List<Player> CreatePlayers()
        {
            return new List<Player>
            {
                new Player(1, "Іван Коваль", 1, 19),
                new Player(2, "Олег Мельник", 1, 22),
                new Player(3, "Андрій Шевчук", 1, 24),
                new Player(4, "Петро Бондар", 1, 25),
                new Player(5, "Максим Ткаченко", 1, 31),
                new Player(6, "Дмитро Кравчук", 2, 21),
                new Player(7, "Сергій Олійник", 2, 26),
                new Player(8, "Василь Лисенко", 2, 36),
                new Player(9, "Юрій Мороз", 3, 23),
                new Player(10, "Богдан Павленко", 3, 29),
                new Player(11, "Роман Савчук", 3, 33),
                new Player(12, "Тарас Гончар", 4, 20),
                new Player(13, "Віталій Руденко", 4, 25),
                new Player(14, "Микола Захарченко", 4, 36),
                new Player(15, "Артем Клименко", 5, 18),
                new Player(16, "Євген Марченко", 5, 28),
                new Player(17, "Ігор Поліщук", 6, 24),
                new Player(18, "Владислав Кузьменко", 6, 30),
                new Player(19, "Олександр Литвин", 6, 34),
                new Player(20, "Денис Остапенко", 8, 22),
                new Player(21, "Ярослав Козак", 8, 27),
                new Player(22, "Назар Білик", 9, 19),
                new Player(23, "Степан Гаврилюк", 9, 32),
                new Player(24, "Остап Дячук", 10, 26),
                new Player(25, "Антон Сидоренко", 10, 29)
            };
        }
    }
}

// Висновок про продуктивність:
// Найшвидшим виявився запит 4 (Max + Where): це два прості проходи по одному списку без побудови допоміжних структур.
// Запити 1 і 2 з Join працюють у кілька разів повільніше, бо Join спершу будує таблицю пошуку за ключем і створює нові об'єкти для кожної пари.
// Найдорожчим став запит 3 (GroupJoin + Average): крім таблиці пошуку, для кожної команди збирається група гравців і по ній ще рахуються Count, Any та Average.
// Синтаксис запитів і синтаксис методів дають майже однаковий час, бо компілятор перетворює синтаксис запитів на виклики тих самих методів; невелика різниця між ними є похибкою вимірювання.
