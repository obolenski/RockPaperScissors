using System;
using System.Linq;
using System.Security.Cryptography;

namespace RockPaperScissors
{
    class Program
    {
        static bool ArgsLegit(string[] x)
        {
            if (x.Distinct().Count() != x.Count())
            {
                return false;
            }
            if (x.Length < 3)
            {
                return false;
            }
            if (x.Length % 2 == 0)
            {
                return false;
            }
            return true;
        }

        static void Main(string[] args)
        {

            if (!ArgsLegit(args))
            {
                Console.WriteLine("Pass an odd number of three or more unique arguments.");
                return;
            }

            RandomNumberGenerator RNG = RandomNumberGenerator.Create();
            byte[] key = new Byte[16];
            RNG.GetBytes(key);

            int computerMove = RandomNumberGenerator.GetInt32(args.Length);
            HMAC hmac = new HMACSHA256(key);
            byte[] hash = hmac.ComputeHash(BitConverter.GetBytes(computerMove));

            Console.WriteLine("HMAC:" + BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant());

            bool showMenu = true;
            string playerInput;
            int playerMove = 0;

            while (showMenu)
            {
                Console.WriteLine("Available moves:");
                foreach (string arg in args)
                {
                    Console.WriteLine((Array.IndexOf(args, arg) + 1) + ": " + arg);
                }
                Console.WriteLine("0: Exit");

                playerInput = Console.ReadLine();
                if (Int32.TryParse(playerInput, out playerMove) && playerMove <= args.Length)
                {
                    showMenu = false;
                }
            }

            if (playerMove == 0)
            {
                return;
            }

            Console.WriteLine("Your move: " + args[playerMove - 1]);
            Console.WriteLine("Computer's move: " + args[computerMove]);


            bool computerWins = false;
            for (int i = playerMove; i < playerMove + (args.Length / 2); i += 1)
            {
                if (args[i % args.Length] == args[(computerMove) % args.Length])
                {
                    computerWins = true;
                }
            }

            if (playerMove - 1 == computerMove)
            {
                Console.WriteLine("Draw.");
            }
            else if (computerWins)
            {
                Console.WriteLine("Computer wins.");
            }
            else
            {
                Console.WriteLine("You win!");
            }

            Console.WriteLine("HMAC key:" + BitConverter.ToString(key).Replace("-", "").ToUpperInvariant());
        }
    }
}
