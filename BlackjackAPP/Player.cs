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

        public string VievDealer()
        {
            if (hideCards)
            {
                return "" + "|" + cards[0].ToString() + "|" + "|?|";
            }
            else
            {
                string str = "";
                foreach (Card card in cards)
                {
                    str += " |" + card.ToString() + "|";
                }
                return str;
            }

        }
        public string VievPlayer()
        {
            string str = "";
            foreach (Card card in cards)
            {
                str += " |" + card.ToString() + "|";
            }
            return str;
        }
        public int Score()
        {
            if (hideCards)
            {
                return cards[0].Value;
            }
            int score = 0;
            foreach (Card card in cards)
            {
                if (card.Name.ToString() != "Туз") score += card.Value;                
            }
            foreach (Card card in cards)
            {
                if (card.Name.ToString() == "Туз")
                {
                    if (score < 21) score += 11;
                    else score += 1;
                }
            }
            return score;
        }

        public Card[] NewRound()
        {
            Card[] r = cards.ToArray();
            cards.Clear();
            return r;            
        }
       
    }
}
