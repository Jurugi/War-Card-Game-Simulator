/*
// Conditions: 52 cards, only 2 players, simulate card game of War

Rules:
Starting a standard 52-card deck, the deck is shuffled and the cards are dealt to each player. Both players receive 26 cards, face down.
The game consists of a series of ‘battles’ in which each player draws a single off the top of their deck and places it face up on the table. If both cards have different values, the player with the highest value card wins the battle. The winner of the battle takes all cards from the table and places them onto the bottom of their deck, in no particular order.
A battle in which both players lay down a card of equal value is referred to as ‘war’ (thus the name of the game). In this event, each player places two additional cards face-down on the table, followed by a third card face-up. The winner is then determined as before by higher-valued card. If perchance these cards are also identical (a ‘double war’), the process repeats itself until a winner is determined.
The player that eventually accumulates all 52 cards, leaving the opponent with no cards, is the winner. In the event of war, if a player doesn't have enough cards left for the war to commence (two cards down and one up), she/he looses the battle and therefore the game.
Reference of rules and C#: https://www.codeproject.com/Articles/37502/War-Card-Game-Simulation-in-C

Approx time taken (converting from C++ version): 0.5 hrs
*/

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warSim2
{
    static class DeckManager
    {
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            for (var i = 0; i < list.Count; i++)
                list.Swap(i, rnd.Next(i, list.Count));
        }

        public static void PopulateDeck(List<int> Deck)
        {
            Deck.Clear(); // Remove any entries if they already exist

            for (int i = 0; i < 13; i++) // For each rank
            {
                for (int j = 0; j < 4; j++) // For each suit
                {
                    Deck.Add(i); // 13*4 = 52
                }
            }

            Random rng = new Random();
            Shuffle(Deck, rng);
        }

        public static int CompareUpturnedCards(List<int> Player1, List<int> Player2) // Returns 0 if player 1 wins, 1 if player 2 wins, -1 if it's a draw again
        {
            int P1Sum = 0, P2Sum = 0;
            for (int i = 0; i < Player1.Count(); i++) P1Sum += Player1[i];
            for (int i = 0; i < Player2.Count(); i++) P2Sum += Player2[i];

            if (P1Sum == P2Sum) return -1;
            if (P1Sum > P2Sum) return 0;
            return 1;
        }

        public static int TextAlignment = 2; // Used to align enum/text of the cards (e.g. card number 0 actually means '2', card number 13 would be 'Ace', etc.)
        public static string[] CardRank_Text = new string[] { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace", }; // Used to output the "Rank" of the card as a string without using compiler intrinsics
    }

    class Program
    {
        static void Main(string[] args)
        {

            // Initialize new card holders
            List<int> Cards = new List<int>(); // Cards not drawn from the deck

            List<int> Upturned1 = new List<int>(); // Drawn and turned up (Player 1)
            List<int> Downturned1 = new List<int>(); // Drawn and turned down (Player 1's deck)

            List<int> Upturned2 = new List<int>(); // Drawn and turned up (Player 2)
            List<int> Downturned2 = new List<int>(); // Drawn and turned down (Player 2's deck)

            // Populate and shuffle deck of cards
            DeckManager.PopulateDeck(Cards);
            Console.Write("Have {0} cards\n", Cards.Count());

            // Deal half of the cards to each player
            //// Player 1 setup
            int Main = 0;
            int Size = Cards.Count() / 2;
            for (int i = 0; i < Size; i++)
            {
                Downturned1.Add(Cards[0]); // take one
                Cards.RemoveAt(0); // erase one
                Main++;
            }

            //// Player 2 setup
            Size = Cards.Count();
            for (int i = 0; i < Size; i++)
            {
                Downturned2.Add(Cards[i]);  // assign rest of the cards 
            }
            Cards.Clear(); // and then erase from the non-existant drawing deck

            // Value(s) for how many times a 'draw' occurs in one round and current round
            int DrawIteration = 0, Round = 1;

            // Run main simulation until no simulated cards remain in either player's deck
            while (true)
            {
                // Check for win
                if (Downturned2.Count() == 0)
                {
                    Console.Write("Player 1 has won.\n");
                    break;
                }
                if (Downturned1.Count() == 0)
                {
                    Console.Write("Player 2 has won.\n");
                    break;
                }

                // Print round
                Console.Write("Round {0}\n", Round);
                Round++;

                // Player 1's pick
                Upturned1.Add(Downturned1[0]); // Pick up card
                Console.Write("Player 1 drew card {0} ({1})\n", Downturned1[0] + DeckManager.TextAlignment, DeckManager.CardRank_Text[Downturned1[0]]);
                Downturned1.RemoveAt(0); // Erase card from P1's deck

                // Player 2's pick
                Upturned2.Add(Downturned2[0]); // Pick up card
                Console.Write("Player 2 drew card {0} ({1})\n", Downturned2[0] + DeckManager.TextAlignment, DeckManager.CardRank_Text[Downturned2[0]]);
                Downturned2.RemoveAt(0); // Erase card from P2's deck

                // Compare player's cards
                int RoundResult = DeckManager.CompareUpturnedCards(Upturned1, Upturned2);
                if (RoundResult == -1) // Draw again
                {
                    DrawIteration++;
                    Console.Write("P1 P2 Draw, waging war in next round\n");
                }

                if (RoundResult == 0) // Player 1 wins the round
                {
                    for (int i = 0; i < Upturned2.Count(); i++) // Put all of Player 2's upturned cards into Player 1's deck
                    {
                        Downturned1.Add(Upturned2[i]);
                        Console.Write("Added {0} to P1's deck\n", Upturned2[i] + DeckManager.TextAlignment);
                    }
                    Upturned2.Clear();

                    for (int i = 0; i < Upturned1.Count(); i++) // Put all of Player 1's upturned cards into Player 1's deck
                    {
                        Downturned1.Add(Upturned1[i]);
                        Console.Write("Added {0} to P1's deck\n", Upturned1[i] + DeckManager.TextAlignment);
                    }
                    Upturned1.Clear();

                    for (int i = 0; i < DrawIteration; i++) // If there was a draw, there will be 2 additional won cards per time from the opposite deck (if applicable)
                    {
                        for (int j = 0; j < 2; j++) // Take 2 cards placed down by P2, your 2 cards will already be in stack
                        {
                            if (Downturned2.Count() > 0)
                            {
                                Downturned1.Add(Downturned2[0]); // Grab card
                                Console.Write("P1 WINS DRAW - Grabbed {0} from P2\n", Downturned2[0] + DeckManager.TextAlignment);
                                Downturned2.RemoveAt(0); // Erase card from deck
                            }
                            else Console.Write("P1 WINS DRAW - P2 has no more cards to take.\n");
                        }
                    }
                    DrawIteration = 0;
                }

                if (RoundResult == 1) // Player 2 wins the round
                {
                    for (int i = 0; i < Upturned1.Count(); i++) // Put all of Player 1's upturned cards into Player 2's deck
                    {
                        Downturned2.Add(Upturned1[i]);
                        Console.Write("Added {0} to P2's deck\n", Upturned1[i] + DeckManager.TextAlignment);
                    }
                    Upturned1.Clear();

                    for (int i = 0; i < Upturned2.Count(); i++) // Put all of Player 2's upturned cards into Player 2's deck
                    {
                        Downturned2.Add(Upturned2[i]);
                        Console.Write("Added {0} to P2's deck\n", Upturned2[i] + DeckManager.TextAlignment);
                    }
                    Upturned2.Clear();

                    for (int i = 0; i < DrawIteration; i++) // If there was a draw, there will be 2 additional won cards per time from the opposite deck (if applicable)
                    {
                        for (int j = 0; j < 2; j++) // Take 2 cards placed down by P1, your 2 cards will already be in stack
                        {
                            if (Downturned1.Count() > 0)
                            {
                                Downturned2.Add(Downturned1[0]); // Grab card
                                Console.Write("P2 WINS DRAW - Grabbed {0} from P1\n", Downturned1[0] + DeckManager.TextAlignment);
                                Downturned1.RemoveAt(0); // Erase card from deck
                            }
                            else Console.Write("P2 WINS DRAW - P1 has no more cards to take.\n");
                        }
                    }
                    DrawIteration = 0;
                }
            }
        }
    }
}
