/*
// Conditions: 52 cards, only 2 players, simulate card game of War

Rules:
Starting a standard 52-card deck, the deck is shuffled and the cards are dealt to each player. Both players receive 26 cards, face down.
The game consists of a series of ‘battles’ in which each player draws a single off the top of their deck and places it face up on the table. If both cards have different values, the player with the highest value card wins the battle. The winner of the battle takes all cards from the table and places them onto the bottom of their deck, in no particular order.
A battle in which both players lay down a card of equal value is referred to as ‘war’ (thus the name of the game). In this event, each player places two additional cards face-down on the table, followed by a third card face-up. The winner is then determined as before by higher-valued card. If perchance these cards are also identical (a ‘double war’), the process repeats itself until a winner is determined.
The player that eventually accumulates all 52 cards, leaving the opponent with no cards, is the winner. In the event of war, if a player doesn't have enough cards left for the war to commence (two cards down and one up), she/he looses the battle and therefore the game.
Reference of rules and C#: https://www.codeproject.com/Articles/37502/War-Card-Game-Simulation-in-C

Approx time taken: 1.5 hrs
*/

#include "Source.h"

void PopulateDeck(std::vector<int> &Deck)
{
	Deck.clear(); // Remove any entries if they already exist

	for (unsigned int i = 0; i < 13; i++) // For each rank
	{
		for (unsigned int j = 0; j < 4; j++) // For each suit
		{
			Deck.push_back(i); // 13*4 = 52
		}
	}

	auto RandEng = std::default_random_engine{ (unsigned int)time(0) }; // Shuffle deck using std::shuffle and pseudo-random seed
	std::shuffle(Deck.begin(), Deck.end(), RandEng);
}

// Returns 0 if player 1 wins, 1 if player 2 wins, -1 if it's a draw again
int CompareUpturnedCards(std::vector<int> &Player1, std::vector<int> &Player2)
{
	int P1Sum = 0, P2Sum = 0;
	for (unsigned int i = 0; i < Player1.size(); i++) P1Sum += Player1[i];
	for (unsigned int i = 0; i < Player2.size(); i++) P2Sum += Player2[i];

	if (P1Sum == P2Sum) return -1;
	if (P1Sum > P2Sum) return 0;
	return 1;
}

int main()
{
	// Populate and shuffle deck of cards
	PopulateDeck(Cards);
	printf("Have %i cards\n", Cards.size());

	// Player 1 starts with no cards 
	//Upturned1.clear();
	//Downturned1.clear();

	// Player 2 starts with no cards
	//Upturned2.clear();
	//Downturned2.clear();

	// Deal half of the cards to each player
	//// Player 1 setup
	unsigned int Size = Cards.size() / 2;
	for (unsigned int i = 0; i < Size; i++)
	{
		Downturned1.push_back(Cards[0]); // take one
		Cards.erase(Cards.begin()); // erase one
	}

	//// Player 2 setup
	Size = Cards.size();
	for (unsigned int i = 0; i < Size; i++)
	{
		Downturned2.push_back(Cards[i]);  // assign rest of the cards 
	}
	Cards.clear(); // and then erase from the non-existant drawing deck

	// Value(s) for how many times a 'draw' occurs in one round and current round
	unsigned int DrawIteration = 0, Round = 1;
	
	// Run main simulation until no simulated cards remain in either player's deck
	while (true)
	{
		// Check for win
		if (Downturned2.size() == 0)
		{
			printf("Player 1 has won.\n");
			break;
		}
		if (Downturned1.size() == 0)
		{
			printf("Player 2 has won.\n");
			break;
		}

		// Print round
		printf("Round %i\n", Round);
		Round++;

		// Player 1's pick
		Upturned1.push_back(Downturned1[0]); // Pick up card
		printf("Player 1 drew card %i (\"%s\")\n", Downturned1[0] + TextAlignment, CardRank_Text[Downturned1[0]]); // Applied adjustment to also output the text of the card at the time you pick it
		Downturned1.erase(Downturned1.begin()); // Erase card from P1's deck
		
		// Player 2's pick
		Upturned2.push_back(Downturned2[0]); // Pick up card
		printf("Player 2 drew card %i (\"%s\")\n", Downturned2[0] + TextAlignment, CardRank_Text[Downturned2[0]]); 
		Downturned2.erase(Downturned2.begin()); // Erase card from P2's deck

		// Compare player's cards
		int RoundResult = CompareUpturnedCards(Upturned1, Upturned2);
		if (RoundResult == -1) // Draw again
		{
			DrawIteration++;
			printf("P1 P2 Draw, waging war in next round\n");
		}

		if (RoundResult == 0) // Player 1 wins the round
		{
			for (unsigned int i = 0; i < Upturned2.size(); i++) // Put all of Player 2's upturned cards into Player 1's deck
			{
				Downturned1.push_back(Upturned2[i]);
				printf("Added %i to P1's deck\n", Upturned2[i] + TextAlignment);
			}
			Upturned2.clear();

			for (unsigned int i = 0; i < Upturned1.size(); i++) // Put all of Player 1's upturned cards into Player 1's deck
			{
				Downturned1.push_back(Upturned1[i]);
				printf("Added %i to P1's deck\n", Upturned1[i] + TextAlignment);
			}
			Upturned1.clear();

			for (unsigned int i = 0; i < DrawIteration; i++) // If there was a draw, there will be 2 additional won cards per time from the opposite deck (if applicable)
			{
				for (unsigned int j = 0; j < 2; j++) // Take 2 cards placed down by P2, your 2 cards will already be in stack
				{
					if (Downturned2.size())
					{
						Downturned1.push_back(Downturned2[0]); // Grab card
						printf("P1 WINS DRAW - Grabbed %i from P2\n", Downturned2[0] + TextAlignment);
						Downturned2.erase(Downturned2.begin()); // Erase card from deck
					}
					else printf("P1 WINS DRAW - P2 has no more cards to take.\n");
				}
			}
			DrawIteration = 0;
		}

		if (RoundResult == 1) // Player 2 wins the round
		{
			for (unsigned int i = 0; i < Upturned1.size(); i++) // Put all of Player 1's upturned cards into Player 2's deck
			{
				Downturned2.push_back(Upturned1[i]);
				printf("Added %i to P2's deck\n", Upturned1[i] + TextAlignment);
			}
			Upturned1.clear();

			for (unsigned int i = 0; i < Upturned2.size(); i++) // Put all of Player 2's upturned cards into Player 2's deck
			{
				Downturned2.push_back(Upturned2[i]);
				printf("Added %i to P2's deck\n", Upturned2[i] + TextAlignment);
			}
			Upturned2.clear();

			for (unsigned int i = 0; i < DrawIteration; i++) // If there was a draw, there will be 2 additional won cards per time from the opposite deck (if applicable)
			{
				for (unsigned int j = 0; j < 2; j++) // Take 2 cards placed down by P1, your 2 cards will already be in stack
				{
					if (Downturned1.size())
					{
						Downturned2.push_back(Downturned1[0]); // Grab card
						printf("P2 WINS DRAW - Grabbed %i from P1\n", Downturned1[0] + TextAlignment);
						Downturned1.erase(Downturned1.begin()); // Erase card from deck
					}
					else printf("P2 WINS DRAW - P1 has no more cards to take.\n");
				}
			}
			DrawIteration = 0;
		}
	}
	system("pause"); // end program and pause
	return 0;
}