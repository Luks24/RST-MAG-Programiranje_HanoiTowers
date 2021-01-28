using HanoiTowersLukaKidric.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanoiTowersLukaKidric
{
    public class HanoiTypeModel
    {
        public HanoiTypeModel(int numDiscs, int numPegs, HanoiType type)
        {
            this.numDiscs = numDiscs;
            this.numPegs = numPegs;
            this.type = type;
        }

        public  int numDiscs;
        public  int numPegs;
        public HanoiType type;

        public HashSet<long> setIgnore; // The states which should not be considered, because they are equivalent
        public HashSet<long> setPrev;
        public HashSet<long> setCurrent;
        public Queue<long> setNew;
        public byte[] stateArray;
        public short currentDistance;

        public virtual void MakeMoveForSmallDimension(byte[] state)
        {
            
        }


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

        public int ShortestPathForSmallDimension(int searchMode, out string path)
        {
            long finalState = 0;

            // For each disc we have its peg
            stateArray = new byte[this.numDiscs];
            //canMoveArray = new bool[this.numPegs];

            setIgnore = new HashSet<long>();
            setPrev = new HashSet<long>();
            setCurrent = new HashSet<long>();
            setNew = new Queue<long>();

            // Set initial and final states for each case
            if (this.type == HanoiType.K13_01)
            {
                stateArray = ArrayAllEqual(0);
                finalState = FinalState();
            }
            else if (this.type == HanoiType.K4e_01)
            {
                stateArray = ArrayAllEqual(0);
                finalState = StateAllEqual(1);
            }
            else if (this.type == HanoiType.K4e_12)
            {
                stateArray = ArrayAllEqual(1);
                finalState = StateAllEqual(2);
            }
            else if (this.type == HanoiType.K4e_23)
            {
                stateArray = ArrayAllEqual(2);
                finalState = StateAllEqual(3);
            }
            else if (this.type == HanoiType.C4_01)
            {
                stateArray = ArrayAllEqual(0);
                finalState = StateAllEqual(1);
            }
            else if (this.type == HanoiType.C4_12)
            {
                stateArray = ArrayAllEqual(1);
                finalState = StateAllEqual(2);
            }
            else if (this.type == HanoiType.K13_12)
            {
                stateArray = ArrayAllEqual(2);
                finalState = FinalState();
            }
            else if (this.type == HanoiType.P4_01)
            {
                stateArray = ArrayAllEqual(0);
                finalState = StateAllEqual(1);
            }
            else if (this.type == HanoiType.P4_12)
            {
                stateArray = ArrayAllEqual(1);
                finalState = StateAllEqual(2);
            }
            else if (this.type == HanoiType.P4_23)
            {
                stateArray = ArrayAllEqual(2);
                finalState = StateAllEqual(3);
            }
            else if (this.type == HanoiType.P4_31)
            {
                stateArray = ArrayAllEqual(3);
                finalState = StateAllEqual(1);
            }
            else if (this.type == HanoiType.K13e_01)
            {
                stateArray = ArrayAllEqual(0);
                finalState = StateAllEqual(1);
            }
            else if (this.type == HanoiType.K13e_12)
            {
                stateArray = ArrayAllEqual(1);
                finalState = StateAllEqual(2);
            }
            else if (this.type == HanoiType.K13e_23)
            {
                stateArray = ArrayAllEqual(2);
                finalState = StateAllEqual(3);
            }
            else if (this.type == HanoiType.K13e_30)
            {
                stateArray = ArrayAllEqual(3);
                finalState = StateAllEqual(0);
            }
            else
            {
                throw new Exception("Hanoi type state is not defined here!");
            }


            currentDistance = 0;
            long initialState = StateToLong(stateArray);
            setCurrent.Add(initialState);

            path = "";


            if (searchMode == 0)
            {
                long maxCardinality = 0;
                long maxMemory = 0;
                InitIgnoredStates(type);

                while (true) // Analiziramo posamezen korak (i-tega premika)
                {
                    if (maxCardinality < setCurrent.Count)
                        maxCardinality = setCurrent.Count;


                    bool toBreak = false;
                    setCurrent.AsParallel().WithDegreeOfParallelism(5).ForAll(num =>  // Znotraj i-tega premika preveri vsa možn stanja in se premaknemo v vse možne pozicije
                    {
                        if (num == finalState)
                        {
                            toBreak = true;
                        }

                        byte[] tmpState = LongToState(num);
                        this.MakeMoveForSmallDimension(tmpState);

                    });

                    if (toBreak) return currentDistance;





                    long mem = GC.GetTotalMemory(false);
                    if (maxMemory < mem)
                    {
                        maxMemory = mem;
                    }

                    // Ko se premaknemo iz vseh trenutnih stanj,
                    // pregledamo nova trenutna stanja
                    setPrev = setCurrent;
                    setCurrent = new HashSet<long>();
                    int elts = setNew.Count;
                    for (int i = 0; i < elts; i++)
                    {
                        setCurrent.Add(setNew.Dequeue());
                    }

                    setNew = new Queue<long>();

                    currentDistance++;

                    Console.WriteLine("Current distance: " + currentDistance + "     Maximum cardinality: " + maxCardinality);
                    Console.WriteLine("Memory allocation: " + mem / 1000000 + "MB  \t\t Maximum memory: " + maxMemory / 1000000 + "MB");
                    Console.CursorTop -= 2;
                }
            }
            return -2;

        }



    }
}
