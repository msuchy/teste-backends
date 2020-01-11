using System;
using System.IO;
using Solution.Domain;
using Solution.Repository;

namespace Solution.App
{
    class Program
    {
        static void Main(string[] args)
        {
            IProposalRepository repository = new ProposalRepository();

            for (var index = 0; index < 13; index++)
            {
                var strIndex = index.ToString().PadLeft(3, '0');
                var inputLines = File.ReadAllLines($"Assets/input{strIndex}.txt");
                var outputLines = File.ReadAllLines($"Assets/output{strIndex}.txt");

                var result = Solution.Instance(repository).ProcessMessages(inputLines);
                if (outputLines[0] == result)
                    Console.WriteLine($"Test #{index + 1}/#13 - Passed");
                else
                    Console.WriteLine($"Test #{index + 1}/#13 - Failed");
            }
        }
    }
}
