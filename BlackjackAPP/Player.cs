using System;
using System.Collections.Generic;

namespace BlackjackAPP
{
    public class Player
    {
        public Account account = new Account(); //счёт игрока
        public List<Card> cards = new List<Card>(0); //карты
        public string Name { get; set; }
        public bool hideCards { get; set; }

        public Player(bool isDealer)
        {
            this.hideCards = isDealer;
        }

        public void AddCard(Card card) // дать игроку карту
        {
            cards.Add(card);
        }

        public void VievCards()
        {
            if (hideCards)
            {
                Console.WriteLine("Первая карта у дилера: {0}", cards[0].ToString());
            }
            else
            {
                Console.WriteLine("\nКарты в руке:");
                foreach (Card card in cards)
                {
                    Console.WriteLine("\t {0}", card.ToString());
                }
                Console.WriteLine("Сумма: {0}", Score());
            }
        }
        public int Score()
        {
            int score = 0;
            foreach (Card card in cards)
            {
                score += card.Value;
            }
            return score;
        }

        public void NewRound()
        {
            cards.Clear();
        }
        
        public void Balance()
        {
            Console.WriteLine("\n\t\t\t====================================");
            Console.WriteLine("\n\t\t\tУ игрока {0} на счету {1}", this.Name, this.account.Value * Account.step);
            Console.WriteLine("\n\t\t\t====================================");
        }
    }
}
