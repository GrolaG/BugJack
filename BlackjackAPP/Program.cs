using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackAPP
{
    class Program
    {
        static Deck Deck = new Deck();
        static Player player = new Player(false);
        static Player dealer = new Player(true);
        static int bidsInGame = 0;

        static void Main()
        {
            Console.WriteLine("Добро пожаловать в виртуальную игру Blackjack");
            Console.WriteLine("Для начала игры введите '1'\nДля выхода введите '2'");
            while (true)
            {
                var t = Console.ReadKey();
                if (t.KeyChar == 49)
                {
                    break;
                }
                else if (t.KeyChar == 50)
                {
                    Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine("\nВведите корректное значение - 1 или 2");
                }
            }
            InitName();
            Game();
            Console.WriteLine("\nТы проиграл))))");
            player.account.Value = 10;
            Main();
        }
        static void InitName()
        {
            if (player.Name == null)
            {
                Console.WriteLine("\nВведите ваше имя (оно будет образано до 20 символов в случае если оно окажется больше");
                string name = Console.ReadLine();
                if (name.Length > 20)
                {
                    name = new string(name.Take(20).ToArray());
                }
                player.Name = name;
            }
        }


        //Игра
        public static int MakeBid()
        {
            int bid = 0;
            while (true)
            {
                Console.WriteLine("\nВведите количество ставок по 10 у.е.");
                int.TryParse(Console.ReadLine(), out bid);

                if (player.account.Withdraw(bid) && (bid > 0)) break;
                else Console.WriteLine("Недостаточно средств или ставка не сделана");
            }
            return bid;
        }
        public static void Reset()
        {
            bidsInGame = 0;
            player.NewRound();
            dealer.NewRound();
            dealer.hideCards = true;
        }

        public static void Game()
        {
            while (player.account.Value > 0)
            {
                player.Balance();
                //Делает ставку
                bidsInGame += MakeBid();
                //Раздача карт и вывод в консоль
                {
                    dealer.AddCard(Deck.GetCard());
                    dealer.AddCard(Deck.GetCard());
                    player.AddCard(Deck.GetCard());
                    player.AddCard(Deck.GetCard());
                    dealer.VievCards();
                    player.VievCards();
                }


                //Игрок набирает карты
                bool take = true;
                while (take == true)
                {
                    if (player.Score() == 21)
                    {
                        Console.WriteLine("У вас:");
                        player.VievCards();
                        take = false;
                    }
                    else
                    {
                        if (player.Score() > 21)
                        {                            
                            Console.WriteLine("У вас:");
                            player.VievCards();
                            take = false;
                        }
                        else
                        {
                            Console.WriteLine("Какое действие совершить? 1 - Взять карту, 2 - Удвоить ставку и взять последнюю карту, 3 - Не брать карту");
                            while (take == true)
                            {
                                var t = Console.ReadKey();
                                if (t.KeyChar == 49)
                                {
                                    player.AddCard(Deck.GetCard());
                                    player.VievCards();
                                    break;
                                }
                                if (t.KeyChar == 50)
                                {
                                    if (player.account.Value >= bidsInGame)
                                    {
                                        player.account.Withdraw(bidsInGame);
                                        bidsInGame *= 2;
                                        player.AddCard(Deck.GetCard());
                                        Console.WriteLine("У вас:");
                                        player.VievCards();
                                        take = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nНедостаточно средств. У вас на счету {0}, а необходимо {1}", player.account.Value, bidsInGame);
                                    }
                                    break;
                                }
                                if (t.KeyChar == 51)
                                {
                                    player.VievCards();
                                    Console.WriteLine("Набор карт окончен. Ход передан дилеру");
                                    take = false;
                                }
                                else Console.WriteLine("\nВведите корректное значение - 1, 2 или 3");
                            }
                        }
                    }
                }

                Console.WriteLine("\n\t\t\t====================================");
                Console.WriteLine("\n\t\t\tХод передан дилеру");
                Console.WriteLine("\n\t\t\t====================================");
                //Диллер действует по своему шаблону из табл. 1
                dealer.hideCards = false;
                dealer.VievCards();

                //Логика дилера из таблицы
                while (take == false)
                {
                    if (player.Score() > 21)
                    {
                        Console.WriteLine("Дилер не взял карт, игрок проиграл ставку.");
                        take = true;
                    }
                    else
                    {
                        if (player.Score() == 21)
                        {
                            Console.WriteLine("Игрок выигрывает этот кон");
                            player.account.Deposit(bidsInGame * 2);
                            take = true;
                        }
                        else if (dealer.Score() == player.Score() && player.Score() > 15)
                        {
                            Console.WriteLine("Дилер не взял карт, ничья. Ставка возвращается.");
                            take = true;
                            player.account.Deposit(bidsInGame);
                        }
                        else if (dealer.Score() > player.Score() && dealer.Score() < 22)
                        {
                            Console.WriteLine("Дилер не взял карт, игрок проиграл ставку.");
                            take = true;
                        }
                        else if (dealer.Score() > 21)
                        {
                            Console.WriteLine("Игрок выигрывает этот кон");
                            player.account.Deposit(bidsInGame * 2);
                            dealer.VievCards();
                            take = true;
                        }
                        else
                        {
                            Console.WriteLine("Дилер берет карту...");
                            dealer.AddCard(Deck.GetCard());
                        }
                    }
                }

                //Дилер закончил добор карт.
                Console.WriteLine("\n\t\t\t====================================");
                Console.WriteLine("\n\t\t\tИтоги");
                Console.WriteLine("\n\t\t\t====================================");
                Console.WriteLine("У игрока:");
                player.VievCards();
                Console.WriteLine("У дилера:");
                dealer.VievCards();

                Reset(); //Сброс данных
            }

        }
    }
    


}
