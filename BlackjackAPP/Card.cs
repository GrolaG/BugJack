﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BlackjackAPP
{
    public enum Suit { Пики, Трефы, Червы, Бубны }
    public enum Name { [Description("2")]  Два = 2, [Description("3")]  Три, [Description("4")] Четыре,
        [Description("5")] Пять, [Description("6")] Шесть, [Description("7")] Семь, [Description("8")] Восемь,
        [Description("9")] Девять, [Description("10")] Десять, [Description("J")] Валет, [Description("Q")] Дама,
        [Description("K")] Король, [Description("A")] Туз }


    public class Card
    {
        public Suit Suit { get; set; }
        public Name Name { get; set; }
        public int Value { get; set; }

        public Card(Suit Suit, Name Name, int Value)
        {
            this.Suit = Suit;
            this.Name = Name;
            this.Value = Value;

        }
        public string ToString() //представление в консоли
        {
            
            return Printer.GetDescription(Name).ToString(); //Name.ToString(); // + " " + Suit.ToString();
        }
    }

    public class Deck
    {
        private List<Card> deck;
        private List<Card> returned = new List<Card>();
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
                }
            }
        }

        public void BackToDeck(Card[] cards)
        {
            returned.AddRange(cards);
        }
        public void CreateFromReturned()
        {
            foreach (Card card in returned)
            {
                deck.Add(card);
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
                this.CreateFromReturned();
                this.Shuffle();
                this.round++; //Фича пока не реализована
            }

            Card card = deck[deck.Count - 1]; //возьмем верхнюю, мы же перемешали
            deck.RemoveAt(deck.Count - 1);
            return card;
        }
    }
}
