using HanoiTowersLukaKidric;
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
            HanoiType type = HanoiTowerSelection.SelectHanoiType();

            // Uporabnik napiše število diskov, ki jih parsamo v int
            Console.Write("Enter number of discs: ");
            int k = int.Parse(Console.ReadLine());

            Console.WriteLine($"Running case: {type} with {k} discs:");

            // Delali bodo samo s štirimi stolpi
            int numPegs = 4;
            // Prikaz izvajanja časa programa
            Stopwatch sw = Stopwatch.StartNew();
            // Ustvarimo novi objekt tipa Hanoi, kateremu podamo prej določene spremnljivke
            HanoiTowerSelection hanBasic = new HanoiTowerSelection(k, numPegs, type);
            // Na našem objektu kličemo funkcijo za določanje najkrajše poti
            int length = hanBasic.ShortestPathForSmallDimension(0, out _);

            Console.WriteLine();
            Console.WriteLine($"\n\nDimension: {k} ; Steps:  {length} ; Time {sw.Elapsed.TotalSeconds}");
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Končano");
            Console.ReadLine();
        }
    }

}