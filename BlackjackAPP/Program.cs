using System;
using System.Linq;
using System.Runtime.InteropServices;
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
            Printer.Welcome();
            player.Name = Printer.Introduce();
            Printer.PrintScreenBeforeGameStarts(player, bidsInGame);
            StartNewGameWithNewChips();
            Console.WriteLine("\nТы проиграл))))");
            player.account.Value = 10;
            Main();
        }

        public static void StartNewGameWithNewChips()
        {
            while (player.account.Value > 0)
            {
                //Игрок делает ставку
                bidsInGame += RulesAndLogic.MakeBid(ref player, bidsInGame);
                //Раздача первых карт и вывод в консоль
                RulesAndLogic.StartNewGame(ref player,ref dealer,ref Deck);
                Printer.PrintScreen(player, dealer, bidsInGame);

                //Игрок набирает карты
                RulesAndLogic.PlayerTakeCards(ref player, ref Deck, ref bidsInGame);

                //Диллер действует по своему шаблону из табл. 1
                dealer.hideCards = false;
                while (RulesAndLogic.DealerDecision(dealer.Score(), player.Score()) != 0)
                {
                    Console.WriteLine("Дилер берет карту...");
                    System.Threading.Thread.Sleep(1000);
                    dealer.AddCard(Deck.GetCard());
                    Printer.PrintScreen(player, dealer, bidsInGame);
                }

                //Подведение итогов
                if (RulesAndLogic.GameResults(dealer.Score(), player.Score()) == 2) player.account.Deposit(bidsInGame * 2);
                else if (RulesAndLogic.GameResults(dealer.Score(), player.Score()) == 3) player.account.Deposit(bidsInGame);
                //Вывод информации
                Printer.PrintScreen(player, dealer, bidsInGame);
                Printer.GameResult(RulesAndLogic.GameResults(dealer.Score(), player.Score()));
                Printer.ContinueOrExit();

                //Позже допилю сохранение результатов т.к. выводить их не нужно :)
                if (player.account.Value > BestScore.GetBest(player.Name) && (player.account.Value > 10))
                {
                    BestScore.SetBest(player.Name, player.account.Value);
                }

                //Сброс данных
                RulesAndLogic.Reset(ref Deck, ref player, ref dealer, ref bidsInGame); 
            }
        }
    }
}
