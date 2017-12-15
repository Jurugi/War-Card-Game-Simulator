#ifndef __Source__h__
#define __Source__h__

#include <vector> // card holders
#include <random> // std::default_random_engine
#include <algorithm> // std::shuffle
#include <time.h> // time(0) for srand

#define TextAlignment 2 // Used to align enum/text of the cards (e.g. card number 0 actually meant '2', card number 13 would be 'Ace', etc.)
const char* CardRank_Text[] = { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King",  "Ace", }; // Used to output the "Rank" of the card as a string without using compiler intrinsics

std::vector<int> Cards; // Cards not drawn from the deck

std::vector<int> Upturned1; // Drawn and turned up (Player 1)
std::vector<int> Downturned1; // Drawn and turned down (Player 1)

std::vector<int> Upturned2; // Drawn and turned up (Player 2)
std::vector<int> Downturned2; // Drawn and turned down (Player 2)


#endif