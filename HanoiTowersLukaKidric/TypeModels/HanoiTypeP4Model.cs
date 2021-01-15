﻿using HanoiTowersLukaKidric.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanoiTowersLukaKidric.TypeModels
{
    class HanoiTypeP4Model : HanoiTowerModel
    {
        public HanoiTypeP4Model(int numDiscs, int numPegs, HanoiType type)
        {
            this.numDiscs = numDiscs;
            this.numPegs = numPegs;
            this.type = type;
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


            if (this.type == HanoiType.P4_01)
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
                        MakeMoveForSmallDimension_P4(tmpState);

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
        private void MakeMoveForSmallDimension_P4(byte[] state)
        {
            bool[] P4CanMoveArray = new bool[this.numPegs];
            ResetArray(P4CanMoveArray);
            byte[] P4NewState;

            for (int i = 0; i < numDiscs; i++)
            {
                if (P4CanMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        foreach (byte j in new byte[] { 3 })
                        {
                            if (P4CanMoveArray[j])
                            {
                                P4NewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    P4NewState[x] = state[x];
                                P4NewState[i] = j;
                                long P4CurrentState = StateToLong(P4NewState);
                                if (!setPrev.Contains(P4CurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(P4CurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 1)
                    {
                        foreach (byte j in new byte[] { 2 })
                        {
                            if (P4CanMoveArray[j])
                            {
                                P4NewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    P4NewState[x] = state[x];
                                P4NewState[i] = j;
                                long P4CurrentState = StateToLong(P4NewState);
                                if (!setPrev.Contains(P4CurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(P4CurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 2)
                    {
                        foreach (byte j in new byte[] { 1, 3 })
                        {
                            if (P4CanMoveArray[j])
                            {
                                P4NewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    P4NewState[x] = state[x];
                                P4NewState[i] = j;
                                long P4CurrentState = StateToLong(P4NewState);
                                if (!setPrev.Contains(P4CurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(P4CurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 3)
                    {
                        foreach (byte j in new byte[] { 0, 2 })
                        {
                            if (P4CanMoveArray[j])
                            {
                                P4NewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    P4NewState[x] = state[x];
                                P4NewState[i] = j;
                                long P4CurrentState = StateToLong(P4NewState);
                                if (!setPrev.Contains(P4CurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(P4CurrentState);
                                    }
                                }
                            }
                        }
                    }
                }
                P4CanMoveArray[state[i]] = false;
            }
        }
    }
}
