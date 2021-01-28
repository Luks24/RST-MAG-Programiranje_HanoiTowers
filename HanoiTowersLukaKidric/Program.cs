using HanoiTowersLukaKidric;
using HanoiTowersLukaKidric.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace PodatkovneStrukture
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uporabnik izbere tip hanoiskega stolpa, ki ga shranimo v spremnljivko
            Console.WriteLine("Hanoi example ");
            HanoiType type = HanoiTypeModel.SelectHanoiType();

            // Uporabnik napiše število diskov, ki jih parsamo v int
            Console.Write("Enter number of discs: ");
            int k = int.Parse(Console.ReadLine());

            Console.WriteLine($"Running case: {type} with {k} discs:");

            // Delali bodo samo s štirimi stolpi
            int numPegs = 4;
            // Prikaz izvajanja časa programa
            Stopwatch sw = Stopwatch.StartNew();
            // Ustvarimo novi objekt tipa Hanoi, kateremu podamo prej določene spremnljivke
            //HanoiTowerSelection hanBasic = new HanoiTowerSelection(k, numPegs, type);
            // Na našem objektu kličemo funkcijo za določanje najkrajše poti
           // HanoiTowerFactory factory = null;
            HanoiTowerFactory factory2 = new HanoiTowerFactory();
            int length = factory2.HanoiTowerFactoryGet(k, numPegs, type);
            //type.HanoiTowerFactory();
           /* switch (type)
            {
                case HanoiType.K13_01:
                    {
                        factory = new HanoiTypeK13_01(k, numPegs, type);                    
                    }
                    
                    break;
                case HanoiType.K13_12:
                    {
                        factory = new HanoiTypeK13_12(k, numPegs, type);
                    }
                    break;

                case HanoiType.K13e_01:
                case HanoiType.K13e_12:
                case HanoiType.K13e_23:
                case HanoiType.K13e_30:
                    {
                        factory = new HanoiTypeK13e(k, numPegs, type);
                    }
                    break;
                case HanoiType.K4e_01:
                case HanoiType.K4e_12:
                case HanoiType.K4e_23:
                    {
                        factory = new HanoiTypeK4e(k, numPegs, type);
                    }
                    break;
                case HanoiType.C4_01:
                case HanoiType.C4_12:
                    {
                        factory = new HanoiTypeC4(k, numPegs, type);
                    }
                    break;
                case HanoiType.P4_01:
                case HanoiType.P4_12:
                case HanoiType.P4_23:
                case HanoiType.P4_31:
                    {
                        factory = new HanoiTypeP4(k, numPegs, type);
                    }
                    break;
            }
            //var length = factory.ProcessHanoiTowers(0, out _);*/

            Console.WriteLine();
            Console.WriteLine($"\n\nDimension: {k} ; Steps:  {length} ; Time {sw.Elapsed.TotalSeconds}");
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Končano");
            Console.ReadLine();
        }
    }

}