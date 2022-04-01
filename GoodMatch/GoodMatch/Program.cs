using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace GoodMatch
{
    class Program
    {   static int GetPercentage(string name1,string name2)
        {  //changing all characters to lowercase and removing blank spaces
            string newName1 =name1.ToLower().Trim();
            string newName2 =name2.ToLower().Trim();
            if (newName1.Length == 0 || newName2.Length == 0)
            {
                Console.WriteLine("Error both names must have a value");
            }
            else
            {
                //concatenating both names with mactches e.g name1 matches name2
                StringBuilder concatedNames = new StringBuilder(newName1 + "matches" + newName2);

                StringBuilder percentageNumber = new StringBuilder();
                int count = 0;
                //loop will count matching char and deleting until con
                while (concatedNames.Length > 0)
                {

                    count = concatedNames.ToString().Count(f => (f == concatedNames[0]));
                    //delete counted char and store new string new string
                    string Temp = concatedNames.ToString().Replace(concatedNames[0].ToString(), string.Empty);
                    //clear old string
                    concatedNames.Clear();
                    //add the new string
                    concatedNames.Append(Temp);
                    //store the counted matching char 
                    percentageNumber.Append(count);
                    //reset the count
                    count = 0;

                }
                //converting the stringbuilder to long
                string stringNumber = percentageNumber.ToString();
                long number = Int64.Parse(stringNumber);
                //the programs below reduce percentage to two digits
                long sum = 0;
                StringBuilder percentage = new StringBuilder(number.ToString());
                StringBuilder TempPercentage = new StringBuilder();
                //outer while loop keeps looping until percentage is two digits
                while (percentage.Length > 2)
                {   //inner loop adds first digit and the last digit stores the sum to percentage
                    TempPercentage.Clear();
                    TempPercentage.Append(percentage);
                    percentage.Clear();
                    while (TempPercentage.Length > 0)
                    {
                        string stringToNumber = TempPercentage.ToString();
                        long Number = Int64.Parse(stringToNumber);
                        if (TempPercentage.Length == 1)
                        {
                            sum = Number;
                            TempPercentage.Clear();
                        }
                        else
                        {   //getting first digit
                            long digits = (long)Math.Log10(Number);
                            long firstdigit = (long)(Number / Math.Pow(10, digits));
                            //getting the last digit
                            long lastdigit = Number % 10;
                            //adding the both digits
                            sum = firstdigit + lastdigit;
                            //removing digits after adding them
                            TempPercentage.Remove(TempPercentage.Length - 1, 1);
                            TempPercentage.Remove(0, 1);
                        }
                        //adding the asum to percentage
                        percentage.Append(sum);

                    }

                }
                //converting final percentage to int
                string finalPercentage = percentage.ToString();
                int intPercentage = Int32.Parse(finalPercentage);
                return intPercentage;
            }

            return 0;


        }
        static void Main(string[] args)
        {
            GetPlayerResult();
            Console.ReadLine();
        }
        static void GetPlayerResult()
        {   //getting data from csv
            var lines = File.ReadAllLines("players.csv");
            // A list to store data from csv
            var list = new List<Player>();
            //List to male player
            var malelist = new List<Player>();
            //List to store female players
            var femalelist = new List<Player>();
            //loop to store player objects to list
            foreach (var line in lines){
                var values = line.Split(",");
                var player = new Player() { Name = values[0], Gender = values[1] };
                list.Add(player);

            }
            //printing the original list
            Console.WriteLine("************************************************************");
            Console.WriteLine("     Original List of Players   ");
            list.ForEach(x => Console.WriteLine($"{x.Name}\t{x.Gender}"));
            //storing male players into different list
            malelist = list.Where(x => x.Gender=="m").ToList();
            //Removing diplucate male names
            malelist = malelist.GroupBy(x => x.Name).Select(x => x.First()).ToList();
            //storing female players into different list
            femalelist = list.Where(x => x.Gender == "f").ToList();
            //Removing diclucate female names
            femalelist = femalelist.GroupBy(x => x.Name).Select(x => x.First()).ToList();
            Console.WriteLine("************************************************************");
            Console.WriteLine("         Grouped Players    ");
            //printing new data of players
            Console.WriteLine("   Male players ");
            malelist.ForEach(x => Console.WriteLine($"{x.Name}\t{x.Gender}"));
            Console.WriteLine("   Female players ");
            femalelist.ForEach(x => Console.WriteLine($"{x.Name}\t{x.Gender}"));
            //creating a loop to each player and get percentage
            int outterloop;
            int innerloop;
            if (malelist.Count() >= femalelist.Count())
            {
                outterloop = malelist.Count();
                innerloop = femalelist.Count();
            }
            else
            {
                outterloop = femalelist.Count();
                innerloop = malelist.Count();
            }
            Console.WriteLine("************************************************************");
            Console.WriteLine("    Results   ");
            //creating new output.txt file to store result
            File.WriteAllText("output.txt", "");
            for (int i = 0; i < outterloop; i++)
            {
                for (int j=0; j < innerloop; j++)
                {   
                    int a = GetPercentage(malelist[i].Name, femalelist[j].Name);
                    if (a > 80)
                    {   
                        Console.WriteLine(malelist[i].Name + " " + femalelist[j].Name + " " + a + " good matche");
                        File.AppendAllText("output.txt", $"{malelist[i].Name} , {femalelist[j].Name}  {a } good matche" + Environment.NewLine);
                    }
                    else
                    {
                        Console.WriteLine(malelist[i].Name + " " + femalelist[j].Name + " " + a );
                        File.AppendAllText(@"output.txt", $"{malelist[i].Name} , {femalelist[j].Name}  {a } " + Environment.NewLine);

                    }
                }
            }



        }
    }
    public class Player{
        public string Name { get; set; }
        public string Gender { get; set; }
    }


}
