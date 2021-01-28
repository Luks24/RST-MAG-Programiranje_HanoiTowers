using HanoiTowersLukaKidric.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanoiTowersLukaKidric
{
    public class HanoiTypeK13_01Model : HanoiTypeModel
    {
        public HanoiTypeK13_01Model(int numDiscs, int numPegs, HanoiType type) : base(numDiscs, numPegs, type)
        {
            this.numDiscs = numDiscs;
            this.numPegs = numPegs;
            this.type = type;
        }
       /* public int ShortestPathForSmallDimension(int searchMode, out string path)
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

            
                stateArray = ArrayAllEqual(0);
                finalState = FinalState();             


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
                        MakeMoveForSmallDimension_K13_01_Fast(tmpState);

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
        */
        public override void MakeMoveForSmallDimension(byte[] state)
        {
            bool[] K13CanMoveArray = new bool[this.numPegs];
            ResetArray(K13CanMoveArray);


            for (int i = 0; i < numDiscs - 2; i++)
            {
                if (K13CanMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        for (byte j = 1; j < numPegs; j++)
                        {
                            if (K13CanMoveArray[j])
                            {
                                AddNewState(state, i, j);
                            }
                        }
                    }
                    else // From other vertices we can only move to center
                    {
                        if (K13CanMoveArray[0])
                        {
                            AddNewState(state, i, 0);
                        }
                    }
                }
                K13CanMoveArray[state[i]] = false;
            }
            // The second biggest:
            if (state[numDiscs - 2] == 0 && state[numDiscs - 1] == 0)
            {
                if (K13CanMoveArray[0] && K13CanMoveArray[2])
                {
                    AddNewState(state, numDiscs - 2, 2);
                }
                if (K13CanMoveArray[0] && K13CanMoveArray[3])
                {
                    AddNewState(state, numDiscs - 2, 3);
                }
                K13CanMoveArray[0] = false;
            }
            else if (state[numDiscs - 2] == 0 && state[numDiscs - 1] == 1)
            {
                if (K13CanMoveArray[0] && K13CanMoveArray[1])
                {
                    AddNewState(state, numDiscs - 2, 1);
                }
                K13CanMoveArray[0] = false;
            }
            else if (state[numDiscs - 2] > 1 && state[numDiscs - 1] == 1)
            {
                if (K13CanMoveArray[state[numDiscs - 2]] && K13CanMoveArray[0])
                {
                    AddNewState(state, numDiscs - 2, 0);
                }
                K13CanMoveArray[state[numDiscs - 2]] = false;
            }
            // Biggest disk is moved only once
            if (state[numDiscs - 1] == 0)
            {
                if (K13CanMoveArray[0] && K13CanMoveArray[1])
                {
                    AddNewState(state, numDiscs - 1, 1);
                    //Console.WriteLine("The biggest is moved!\n");
                }
            }
        }
    }
}

