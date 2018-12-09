using System;
using System.Collections.Generic;

namespace BlackjackAPP
{
    public enum Suit { Пики, Трефы, Червы, Бубны }
    public enum Name { Два = 2, Три, Четыре, Пять, Шесть, Семь, Восемь, Девять, Десять, Валет, Дама, Король, Туз }
    
    public class Card
    {
        public Suit Suit { get; set; }
        public Name Name { get; set; }
        public int Value { get; set; }

        public Card(Suit Suit, Name Name, int Value, bool Visibility = false)
        {
            this.Suit = Suit;
            this.Name = Name;
            this.Value = Value;

        }
        public string ToString() //представление в консоли
        {
            return Name.ToString() + " " + Suit.ToString();
        }
    }

    public class Deck
    {
        private List<Card> deck;
        public int round = 1; //Может потом сделаю фичу

        public Deck()
        {
            CreateDeck();
            Shuffle();
        }
        //
        public void CreateDeck()
        {
            deck = new List<Card>();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j < 15; j++)
                {
                    deck.Add(new Card((Suit)i, (Name)j, ((j < 10) ? j : 10)));
                    //Вот тут баг можно было оставить
                }
            }
        }
        //https://stackoverflow.com/questions/273313/randomize-a-listt
        public void Shuffle()
        {
            Random rng = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card card = deck[k];
                deck[k] = deck[n];
                deck[n] = card;
            }
        }

        public Card GetCard()
        {
            if (deck.Count <= 0)
            {
                this.CreateDeck();
                this.Shuffle();
                this.round++; //Фича пока не реализована
            }

            Card card = deck[deck.Count - 1]; //возьмем верхнюю, мы же перемешали
            deck.RemoveAt(deck.Count - 1);
            return card;
        }
    }
}
