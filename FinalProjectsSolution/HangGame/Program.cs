using HangGame.Game;
using HangGame.Models;
using HangGame.Services;

namespace HangGame
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine(" Hang — Word-Guess Game ");

                var words = WordsPool.GetDefaultWords();
                var statsRepo = new XmlStatsRepository();

                Console.Write("Enter your name: ");
                string playerName = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(playerName))
                {
                    Console.WriteLine("Name cannot be empty. Using 'Player'.");
                    playerName = "Player";
                }

                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nOptions:");
                    Console.WriteLine("1) Play a game");
                    Console.WriteLine("2) Show TOP 10 players");
                    Console.WriteLine("3) Exit");
                    Console.Write("Choose option: ");

                    var opt = Console.ReadLine()?.Trim();
                    switch (opt)
                    {
                        case "1":
                            PlaySingleGame(words, statsRepo, playerName);
                            break;

                        case "2":
                            ShowTopPlayers(statsRepo);
                            break;

                        case "3":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid option. Choose 1, 2, or 3.");
                            break;
                    }
                }

                Console.WriteLine("Goodbye!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }
        }

        private static void PlaySingleGame(List<string> words, XmlStatsRepository statsRepo, string playerName)
        {
            var rnd = new Random();
            string secret = words[rnd.Next(words.Count)].ToLowerInvariant();
            var game = new HangmanGame(secret);

            Console.WriteLine("\nA new word has been chosen.");
            Console.WriteLine($"You have {game.MaxLetterGuesses} attempts to open letters.");

            while (true)
            {
                Console.WriteLine($"\nWord: {FormatMasked(game.MaskedWord)}");
                Console.WriteLine($"Guessed letters: {string.Join(", ", game.GuessedLetters)}");
                Console.WriteLine($"Wrong guesses: {game.WrongUniqueGuesses}/{game.MaxLetterGuesses}");

                if (game.IsFullyRevealed)
                {
                    Console.WriteLine("You revealed all letters! You win!");
                    int score = game.CalculateScore();
                    Console.WriteLine($"Score: {score}");
                    statsRepo.UpdatePlayerRecord(playerName, score);
                    break;
                }

                if (game.WrongUniqueGuesses >= game.MaxLetterGuesses)
                {
                    Console.WriteLine("Used 6 wrong letters — you lose.");
                    Console.WriteLine($"The word was: {secret}");
                    statsRepo.UpdatePlayerRecord(playerName, 0);
                    break;
                }

                Console.Write("Enter a letter or write 'guess' in console to guess full word: ");
                string input = Console.ReadLine()?.Trim();

                if (input == "guess")
                {
                    Console.Write("Enter the full word: ");
                    string attempt = Console.ReadLine()?.Trim();

                    if (game.GuessWord(attempt))
                    {
                        Console.WriteLine("Correct! You win!");
                        int score = game.CalculateScore();
                        Console.WriteLine($"Score: {score}");
                        statsRepo.UpdatePlayerRecord(playerName, score);
                    }
                    else
                    {
                        Console.WriteLine($"Wrong! The word was: {secret}");
                        statsRepo.UpdatePlayerRecord(playerName, 0);
                    }
                    break;
                }

                if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
                {
                    Console.WriteLine("Invalid input. Only a single letter allowed.");
                    continue;
                }

                char letter = char.ToLower(input[0]);

                if (game.GuessedLetters.Contains(letter))
                {
                    Console.WriteLine("Already guessed. No penalty.");
                    continue;
                }

                var positions = game.GuessLetter(letter);

                if (positions.Count == 0)
                    Console.WriteLine($"Letter '{letter}' not in word.");
                else
                    Console.WriteLine($"Letter '{letter}' found at positions: {string.Join(", ", positions.Select(p => p + 1))}");
            }
        }

        private static void ShowTopPlayers(XmlStatsRepository repo)
        {
            Console.WriteLine("\n--- TOP 10 PLAYERS ---");

            var top = repo.GetTopPlayers(10);
            if (top.Count == 0)
            {
                Console.WriteLine("No records yet.");
                return;
            }

            Console.WriteLine("{0,-3} {1,-20} {2,6}", "#", "Name", "Score");

            int i = 1;
            foreach (var p in top)
            {
                Console.WriteLine($"{i,-3} {p.Name,-20} {p.HighestScore,6}");
                i++;
            }
        }

        private static string FormatMasked(string masked)
        {
            return string.Join(" ", masked.ToCharArray());
        }
    }
}
