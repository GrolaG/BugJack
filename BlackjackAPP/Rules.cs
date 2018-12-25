using System;

namespace BlackjackAPP
{
    public class RulesAndLogic
    {
        private static readonly int losingPoints = 22; //22 очка
        private static readonly int notMuch = 15; //граница в 15 очков

        public static void StartNewGame(ref Player player, ref Player dealer, ref Deck Deck)
        {
            dealer.AddCard(Deck.GetCard());
            dealer.AddCard(Deck.GetCard());
            player.AddCard(Deck.GetCard());
            player.AddCard(Deck.GetCard());
        }
        public static int GameResults(int dealerPoints, int playerPoints) //Коды завершения игр
        {
            if (playerPoints >= losingPoints)
            {
                return 1; //Проиграл
            }
            else if (dealerPoints >= losingPoints)
            {
                return 2; //Победа
            }
            else if (playerPoints > dealerPoints)
            {
                return 2; //Победа
            }
            else if (playerPoints == dealerPoints)
            {
                return 3; //Ничья
            }
            else
            {
                return 1; //Проиграл
            }
        }

        public static int MakeBid(ref Player player, int bidsInGame)
        {
            int bid = 0;
            while (true)
            {
                Printer.PrintScreenBeforeGameStarts(player, bidsInGame);
                Console.WriteLine("\nВведите желаемое количество минимальных ставок для начала игры. Минимальная ставка – 10 у.е.");
                int.TryParse(Console.ReadLine(), out bid);
                if (player.account.Withdraw(bid)) break;
                else Console.WriteLine("Недостаточно средств или ставка не сделана");
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
            }
            return bid;
        }
        public static void Reset(ref Deck Deck, ref Player player, ref Player dealer, ref int bidsInGame)
        {
            bidsInGame = 0;
            Deck.BackToDeck(player.NewRound());
            Deck.BackToDeck(dealer.NewRound());
            dealer.hideCards = true;
            Console.Clear();
        }


        public static void PlayerTakeCards(ref Player player, ref Deck Deck, ref int bidsInGame)
        {
            while (player.Score() < losingPoints)
            {
                Console.WriteLine("Какое действие совершить? 1 - Взять карту, 2 - Удвоить ставку и взять последнюю карту, 3 - Не брать карту");
                var t = Console.ReadKey();
                if (t.KeyChar == 49)
                {
                    player.AddCard(Deck.GetCard());
                }
                else if (t.KeyChar == 50)
                {
                    if (player.account.Value >= bidsInGame)
                    {
                        player.account.Withdraw(bidsInGame);
                        bidsInGame *= 2;
                        player.AddCard(Deck.GetCard());
                    }
                    else
                    {
                        Console.WriteLine("\nНедостаточно средств. У вас на счету {0}, а необходимо {1}", player.account.Value, bidsInGame);
                    }
                }
                else if (t.KeyChar == 51)
                {
                    break;
                }
                else Console.WriteLine("\nВведите корректное значение - 1, 2 или 3");
                Printer.PrintOnlyPlayerCardsLineAndPlayerScore(player);
            }            
        }

        public static int DealerDecision(int dealerPoints, int playerPoints)
        {
            if (playerPoints >= losingPoints)
            {
                return 0;
            }
            else if (dealerPoints >= losingPoints)
            {
                return 0;
            }
            else
            {
                if (playerPoints == 21)
                {
                    if (playerPoints == dealerPoints)
                    {
                        return 0;
                    }
                    else return 21;
                }
                else if (dealerPoints > playerPoints)
                {
                    return 0;
                }
                else if (dealerPoints == playerPoints && dealerPoints >= notMuch)
                {
                    return 0;
                }
                else //if (dealerPoints < playerPoints && dealerPoints <= notMuch)
                {
                    return 1;
                }
            }
        }

    }
}

