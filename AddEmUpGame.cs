using System;
using System.IO;
using System.Collections.Generic;

namespace AddEmUpGame
{
    class AddEmUpGame
    {
        static void Main(string[] args)
        {
            string inputFile = "";
            string outputFile = "";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--in")
                {
                    inputFile = args[i + 1];
                }
                if (args[i] == "--out")
                {
                    outputFile = args[i + 1];
                }
            }

            if (inputFile == "" || outputFile == "")
            {
                Console.WriteLine("Exception: Missing --in or --out parameter");
                return;
            }
            string text = File.ReadAllText(inputFile);

            string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
     

            Dictionary<string, string[]> players = new Dictionary<string, string[]>();
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] parts = line.Split(':');
                    string name = parts[0];
                    string[] cards = parts[1].Split(',');

                    if (cards.Length != 5)
                    {
                        File.WriteAllText(outputFile, "Exception: Each player must have exactly 5 cards.");
                        return;
                    }

                    players.Add(name, cards);
                }
            }

            int maxSum = 0;
            string winner = "";
            Dictionary<string, int> suitScores = new Dictionary<string, int>();
            List<string> tiedPlayers = new List<string>();

            if (players.Count != 7)
            {
                File.WriteAllText(outputFile, "Exception: Incorrect number of players.");
                return;
            }

            foreach (var player in players)
            {
                int[] values = new int[5];
                int highestCardValue = 0;
                int suitScore = 0;
                for (int i = 0; i < 5; i++)
                {
                    string card = player.Value[i].Trim();
                    char rank = char.ToUpper(card[0]);
                    char suit;

                    if (card.Length == 3)
                    {
                        suit = char.ToUpper(card[2]);
                    }
                    else
                    {
                        suit = char.ToUpper(card[1]);
                    }

                    if (rank == 'J')
                    {
                        values[i] = 11;
                    }
                    else if (rank == 'Q')
                    {
                        values[i] = 12;
                    }
                    else if (rank == 'K')
                    {
                        values[i] = 13;
                    }
                    else if (rank == 'A')
                    {
                        values[i] = 11;
                    }
                    else if (rank == '1' && card[1] == '0')
                    {
                        values[i] = 10;
                    }
                    else
                    {
                        values[i] = int.Parse(rank.ToString());
                    }
                    if (values[i] > highestCardValue)
                    {
                        highestCardValue = values[i];

                        if (suit == 'D')
                        {
                            suitScore = 1;
                        }
                        else if (suit == 'H')
                        {
                            suitScore = 2;
                        }
                        else if (suit == 'S')
                        {
                            suitScore = 3;
                        }
                        else if (suit == 'C')
                        {
                            suitScore = 4;
                        }
                        else if (suit != 'C' && suit != 'D' && suit != 'H' && suit != 'S')
                        {
                            File.WriteAllText(outputFile, "Exception: Cards must be either Diamond, Heart, Spade, or Club." +
                                " Please change the input and try again");
                            return;
                        }

                    }
                }
                suitScores[player.Key] = suitScore;
                Array.Sort(values);
                int sum = values[2] + values[3] + values[4];

                if (sum > maxSum)
                {
                    maxSum = sum;
                    winner = player.Key;
                    tiedPlayers.Clear();
                    tiedPlayers.Add(player.Key);
                }

                else if (sum == maxSum)
                {
                    tiedPlayers.Add(player.Key);
                }

                if (tiedPlayers.Count == 1)
                {
                    File.WriteAllText(outputFile, winner + ":" + maxSum);
                }

                if (tiedPlayers.Count > 1)
                {
                    int maxSuitScore = 0;
                    foreach (string tiedPlayer in tiedPlayers)
                    {
                        int currentSuitScore = suitScores[tiedPlayer];
                        if (currentSuitScore > maxSuitScore)
                        {
                            maxSuitScore = currentSuitScore;
                            winner = tiedPlayer;
                            File.WriteAllText(outputFile, winner + ":" + maxSum);
                        }
                        else if (maxSuitScore == currentSuitScore)
                        {
                            File.WriteAllText(outputFile, string.Join(",", tiedPlayers) + ":" + maxSum);
                        }
                    }
                }
            }
        }
    }
}