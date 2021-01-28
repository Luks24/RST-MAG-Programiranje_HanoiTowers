using HanoiTowersLukaKidric.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanoiTowersLukaKidric.TypeModels
{
    class HanoiTypeK4eModel : HanoiTypeModel
    {
        public HanoiTypeK4eModel(int numDiscs, int numPegs, HanoiType type) : base(numDiscs, numPegs, type)
        {
            this.numDiscs = numDiscs;
            this.numPegs = numPegs;
            this.type = type;
        }
        /*public int ShortestPathForSmallDimension(int searchMode, out string path)
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


            if (this.type == HanoiType.K4e_01)
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
                        MakeMoveForSmallDimension_K4e(tmpState);

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

        }*/
        public override void MakeMoveForSmallDimension(byte[] state)
        {
            bool[] K4eCanMoveArray = new bool[this.numPegs];
            ResetArray(K4eCanMoveArray);
            byte[] K4eNewState;

            for (int i = 0; i < numDiscs; i++)
            {
                if (K4eCanMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        foreach (byte j in new byte[] { 1, 2, 3 })
                        {
                            if (K4eCanMoveArray[j])
                            {
                                K4eNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    K4eNewState[x] = state[x];
                                K4eNewState[i] = j;
                                long K4eCurrentState = StateToLong(K4eNewState);
                                if (!setPrev.Contains(K4eCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(K4eCurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 1)
                    {
                        foreach (byte j in new byte[] { 0, 2, 3 })
                        {
                            if (K4eCanMoveArray[j])
                            {
                                K4eNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    K4eNewState[x] = state[x];
                                K4eNewState[i] = j;
                                long K4eCurrentState = StateToLong(K4eNewState);
                                if (!setPrev.Contains(K4eCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(K4eCurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 2)
                    {
                        foreach (byte j in new byte[] { 0, 1 })
                        {
                            if (K4eCanMoveArray[j])
                            {
                                K4eNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    K4eNewState[x] = state[x];
                                K4eNewState[i] = j;
                                long K4eCurrentState = StateToLong(K4eNewState);
                                if (!setPrev.Contains(K4eCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(K4eCurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 3)
                    {
                        foreach (byte j in new byte[] { 0, 1 })
                        {
                            if (K4eCanMoveArray[j])
                            {
                                K4eNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    K4eNewState[x] = state[x];
                                K4eNewState[i] = j;
                                long K4eCurrentState = StateToLong(K4eNewState);
                                if (!setPrev.Contains(K4eCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(K4eCurrentState);
                                    }
                                }
                            }
                        }
                    }
                }
                K4eCanMoveArray[state[i]] = false;
            }
        }
    }
}
