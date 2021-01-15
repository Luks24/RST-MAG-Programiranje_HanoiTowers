using HanoiTowersLukaKidric.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric
{
    public class HanoiTowerModel
    {
        public  int numDiscs;
        public  int numPegs;
        public HanoiType type;

        public HashSet<long> setIgnore; // The states which should not be considered, because they are equivalent
        public HashSet<long> setPrev;
        public HashSet<long> setCurrent;
        public Queue<long> setNew;
        public byte[] stateArray;
        public short currentDistance;

        public static HanoiType SelectHanoiType()
        {
            Console.WriteLine(">> Select coloring type:");
            WriteHanoiTypes();
            return (HanoiType)Enum.Parse(typeof(HanoiType), Console.ReadLine());
        }
        // Ta funkcija izpiše vse tipe hanoiskih stolpov
        private static void WriteHanoiTypes()
        {
            foreach (string s in Enum.GetNames(typeof(HanoiType)))
            {
                Console.WriteLine("\t" + (int)Enum.Parse(typeof(HanoiType), s) + " - " + s);
            }
        }

        public void InitIgnoredStates(HanoiType type)
        {
            switch (type)
            {
                case HanoiType.K13_01:
                    AddStateLeading3();
                    AddStateLeading1Then3();
                    break;
            }
        }

        public void AddStateLeading1Then3()
        {
            byte[] newState;
            for (int i = 1; i < numDiscs; i++)
            {
                newState = new byte[numDiscs];
                newState[0] = 1;
                for (int j = 1; j <= i; j++)
                    newState[j] = 3;

                setIgnore.Add(StateToLong(newState));
            }
        }

        public void AddStateLeading3()
        {
            byte[] newState;
            for (int i = 0; i < numDiscs; i++)
            {
                newState = new byte[numDiscs];
                for (int j = 0; j <= i; j++)
                    newState[j] = 3;

                setIgnore.Add(StateToLong(newState));
            }
        }

        public void AddNewState(byte[] state, int disc, byte toPeg)
        {
            byte[] XnewState;
            XnewState = new byte[state.Length];
            for (int x = 0; x < state.Length; x++)
                XnewState[x] = state[x];
            XnewState[disc] = toPeg;
            long XcurrentState = StateToLong(XnewState);
            if (!setPrev.Contains(XcurrentState) && !setIgnore.Contains(XcurrentState))
            {
                lock (setNew)
                {
                    setNew.Enqueue(XcurrentState);
                }
            }
        }

        public long StateToLong(byte[] state)
        {
            long num = 0;
            long factor = 1;
            for (int i = state.Length - 1; i >= 0; i--)
            {
                num += state[i] * factor;
                factor *= this.numPegs;
            }
            return num;
        }

        public long FinalState()
        {
            long num = 0;
            long factor = 1;
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                num += factor;
                factor *= this.numPegs;
            }
            return num;
        }

        public byte[] LongToState(long num)
        {
            byte[] tmpState = new byte[this.numDiscs];
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                tmpState[i] = (byte)(num % this.numPegs);
                num = num / this.numPegs;
            }
            return tmpState;
        }

        public string LongToStateString(long num)
        {
            string stateString = "";
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                stateString += (byte)(num % this.numPegs);
                num = num / this.numPegs;
            }
            return stateString;
        }

        public long StateAllEqual(int pegNumber)
        {
            long num = 0;
            long factor = 1;
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                num += pegNumber * factor;
                factor *= this.numPegs;
            }
            return num;
        }

        public byte[] ArrayAllEqual(byte pegNumber)
        {
            byte[] arr = new byte[this.numDiscs];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = pegNumber;
            return arr;
        }

        public void ResetArray(bool[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = true;
        }



    }
}
