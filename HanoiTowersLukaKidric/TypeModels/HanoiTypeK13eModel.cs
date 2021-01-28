using HanoiTowersLukaKidric.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanoiTowersLukaKidric.TypeModels
{
    public class HanoiTypeK13eModel : HanoiTypeModel
    {
        
        public HanoiTypeK13eModel(int numDiscs, int numPegs, HanoiType type) : base(numDiscs, numPegs, type)
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


            if (this.type == HanoiType.K13e_01)
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
                        MakeMoveForSmallDimension_K13e(tmpState);

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
        public override void  MakeMoveForSmallDimension(byte[] state)
        {
            bool[] K13eCanMoveArray = new bool[this.numPegs];
            ResetArray(K13eCanMoveArray);
            byte[] K13eNewState;

            for (int i = 0; i < numDiscs; i++)
            {
                if (K13eCanMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        for (byte j = 1; j < numPegs; j++)
                        {
                            if (K13eCanMoveArray[j])
                            {
                                K13eNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    K13eNewState[x] = state[x];
                                K13eNewState[i] = j;
                                long K13eCurrentState = StateToLong(K13eNewState);
                                // Zaradi takih preverjanj potrebujemo hitro iskanje!
                                if (!setPrev.Contains(K13eCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(K13eCurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 1)
                    {
                        if (K13eCanMoveArray[0])
                        {
                            K13eNewState = new byte[state.Length];
                            for (int x = 0; x < state.Length; x++)
                                K13eNewState[x] = state[x];
                            K13eNewState[i] = 0;
                            long K13eCurrentState = StateToLong(K13eNewState);
                            if (!setPrev.Contains(K13eCurrentState))
                            {
                                lock (setNew)
                                {
                                    setNew.Enqueue(K13eCurrentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 2)
                    {
                        foreach (byte j in new byte[] { 0, 3 })
                        {
                            if (K13eCanMoveArray[j])
                            {
                                K13eNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    K13eNewState[x] = state[x];
                                K13eNewState[i] = j;
                                long K13eCurrentState = StateToLong(K13eNewState);
                                if (!setPrev.Contains(K13eCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(K13eCurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 3)
                    {
                        foreach (byte j in new byte[] { 0, 2 })
                        {
                            if (K13eCanMoveArray[j])
                            {
                                K13eNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    K13eNewState[x] = state[x];
                                K13eNewState[i] = j;
                                long K13eCurrentState = StateToLong(K13eNewState);
                                if (!setPrev.Contains(K13eCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(K13eCurrentState);
                                    }
                                }
                            }
                        }
                    }
                }
                K13eCanMoveArray[state[i]] = false;
            }
        }
    }
}
