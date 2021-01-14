using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanoiTowersLukaKidric
{
    public enum HanoiType
    {
        K13_01,
        K13_12,
        K13e_01,
        K13e_12,
        K13e_23,
        K13e_30,
        P4_01,
        P4_12,
        P4_23,
        P4_31,
        C4_01,
        C4_12,
        K4e_01,
        K4e_12,
        K4e_23,
    }
    class HanoiTowerSelection
    {
        private readonly int numDiscs;
        private readonly int numPegs;
        private readonly HanoiType type;

        private HashSet<long> setIgnore; // The states which should not be considered, because they are equivalent
        private HashSet<long> setPrev;
        private HashSet<long> setCurrent;
        private Queue<long> setNew;
        private byte[] stateArray;
        private bool[] canMoveArray;
        private byte[] newState;
        private long currentState;
        private short currentDistance;

        public HanoiTowerSelection(int numDiscs, int numPegs, HanoiType type)
        {
            this.numDiscs = numDiscs;
            this.numPegs = numPegs;
            this.type = type;
        }

        // Ta funkcija prejme tip hanoiskega stolpa
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

        long finalState = 0;

        /// <summary>
        /// Computes the length of a shortest path from the initial state to the final state. Only for small dimensions.
        /// 
        /// If searchMode = 0, the algorithm performs breadth-first search through the graph of states. 
        /// Each state represents a number in 4-base. For big dimensions integers do not suffice, so we work with longs.
        /// For higher dimensions we need to save the set of vertices in the farthest set, from which we continue search, in a file.
        /// 
        /// For searchMode = 1, we do it with Iterative DFS.
        /// </summary>
        public int ShortestPathForSmallDimension(int searchMode, out string path)
        {
            if (this.type != HanoiType.K13_01 && this.type != HanoiType.K13_12
                && this.type != HanoiType.K13e_01 && this.type != HanoiType.K13e_12 && this.type != HanoiType.K13e_23 && this.type != HanoiType.K13e_30
                && this.type != HanoiType.K4e_01 && this.type != HanoiType.K4e_12 && this.type != HanoiType.K4e_23
                && this.type != HanoiType.C4_01 && this.type != HanoiType.C4_12
                && this.type != HanoiType.P4_01 && this.type != HanoiType.P4_12 && this.type != HanoiType.P4_23 && this.type != HanoiType.P4_31)
                throw new NotImplementedException("The search for this type is not implemented yet.");

            // For each disc we have its peg
            stateArray = new byte[this.numDiscs];
            canMoveArray = new bool[this.numPegs];

            setIgnore = new HashSet<long>();
            setPrev = new HashSet<long>();
            setCurrent = new HashSet<long>();
            setNew = new Queue<long>();

            // Set initial and final states for each case
            {
                if (this.type == HanoiType.K13_01)
                {
                    stateArray = ArrayAllEqual(0);
                    finalState = FinalState();
                }
                else if (this.type == HanoiType.K13_12)
                {
                    stateArray = ArrayAllEqual(2);
                    finalState = FinalState();
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
                else
                {
                    throw new Exception("Hanoi type state is not defined here!");
                }
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
                    switch (type)
                    {
                        case HanoiType.K13_01:
                             {
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
                             }
                             break;
                        /*  case HanoiType.K13_12:
                             {
                                 bool toBreak = false;
                                 setCurrent.AsParallel().WithDegreeOfParallelism(5).ForAll(num =>  // Znotraj i-tega premika preveri vsa možn stanja in se premaknemo v vse možne pozicije
                                 {
                                     if (num == finalState)
                                     {
                                         toBreak = true;
                                     }

                                     byte[] tmpState = LongToState(num);
                                     MakeMoveForSmallDimension_K13(tmpState);

                                 });

                                 if (toBreak) return currentDistance;
                             }
                             break;   */

                        case HanoiType.K13e_01:
                        case HanoiType.K13e_12:
                        case HanoiType.K13e_23:
                        case HanoiType.K13e_30:
                            {
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
                            }
                            break;
                        case HanoiType.K4e_01:
                        case HanoiType.K4e_12:
                        case HanoiType.K4e_23:
                            {
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
                            }
                            break;
                        case HanoiType.C4_01:
                        case HanoiType.C4_12:
                            {
                                bool toBreak = false;
                                setCurrent.AsParallel().WithDegreeOfParallelism(5).ForAll(num =>  // Znotraj i-tega premika preveri vsa možn stanja in se premaknemo v vse možne pozicije
                                {
                                    if (num == finalState)
                                    {
                                        toBreak = true;
                                    }

                                    byte[] tmpState = LongToState(num);
                                    MakeMoveForSmallDimension_C4(tmpState);

                                });
                                if (toBreak) return currentDistance;
                            }
                            break;
                        case HanoiType.P4_01:
                        case HanoiType.P4_12:
                        case HanoiType.P4_23:
                        case HanoiType.P4_31:
                            {
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
                            }
                            break;
                    }

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
        /*
        private object queryGraph()
        {
            try
            {
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
                if (toBreak) {
                    return currentDistance;
                }
                else
                {
                    return currentDistance;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        private void InitIgnoredStates(HanoiType type)
        {
            switch (type)
            {
                case HanoiType.K13_01:
                    AddStateLeading3();
                    AddStateLeading1Then3();
                    break;
            }
        }

        private void AddStateLeading1Then3()
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

        private void AddStateLeading3()
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

        private void AddNewState(byte[] state, int disc, byte toPeg)
        {
            Queue<long> XsetNew;
            XsetNew = new Queue<long>();
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
                    XsetNew.Enqueue(XcurrentState);
                }
            }
        }

        private void MakeMoveForSmallDimension_K13(byte[] state)
        {
            ResetArray(canMoveArray);

            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        for (byte j = 1; j < numPegs; j++)
                        {
                            if (canMoveArray[j])
                            {
                                AddNewState(state, i, j);
                            }
                        }
                    }
                    else // From other vertices we can only move to center
                    {
                        if (canMoveArray[0])
                        {
                            AddNewState(state, i, 0);
                        }
                    }
                }
                canMoveArray[state[i]] = false;
            }
        }

        private void MakeMoveForSmallDimension_K13_01_Fast(byte[] state)
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

        private void MakeMoveForSmallDimension_K13e(byte[] state)
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

        private void MakeMoveForSmallDimension_K4e(byte[] state)
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

        private void MakeMoveForSmallDimension_C4(byte[] state)
        {
            bool[] C4CanMoveArray = new bool[this.numPegs];
            ResetArray(C4CanMoveArray);
            byte[] C4NewState;

            for (int i = 0; i < numDiscs; i++)
            {
                if (C4CanMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        foreach (byte j in new byte[] { 2, 3 })
                        {
                            if (C4CanMoveArray[j])
                            {
                                C4NewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    C4NewState[x] = state[x];
                                C4NewState[i] = j;
                                long C4CurrentState = StateToLong(C4NewState);
                                if (!setPrev.Contains(C4CurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(C4CurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 1)
                    {
                        foreach (byte j in new byte[] { 2, 3 })
                        {
                            if (C4CanMoveArray[j])
                            {
                                C4NewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    C4NewState[x] = state[x];
                                C4NewState[i] = j;
                                long C4CurrentState = StateToLong(C4NewState);
                                if (!setPrev.Contains(C4CurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(C4CurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 2)
                    {
                        foreach (byte j in new byte[] { 0, 1 })
                        {
                            if (C4CanMoveArray[j])
                            {
                                C4NewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    C4NewState[x] = state[x];
                                C4NewState[i] = j;
                                long C4CurrentState = StateToLong(C4NewState);
                                if (!setPrev.Contains(C4CurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(C4CurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 3)
                    {
                        foreach (byte j in new byte[] { 0, 1 })
                        {
                            if (C4CanMoveArray[j])
                            {
                                C4NewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    C4NewState[x] = state[x];
                                C4NewState[i] = j;
                                long C4CurrentState = StateToLong(C4NewState);
                                if (!setPrev.Contains(C4CurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Enqueue(C4CurrentState);
                                    }
                                }
                            }
                        }
                    }
                }
                C4CanMoveArray[state[i]] = false;
            }
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

        private long StateToLong(byte[] state)
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

        private long FinalState()
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

        private byte[] LongToState(long num)
        {
            byte[] tmpState = new byte[this.numDiscs];
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                tmpState[i] = (byte)(num % this.numPegs);
                num = num / this.numPegs;
            }
            return tmpState;
        }

        private string LongToStateString(long num)
        {
            string stateString = "";
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                stateString += (byte)(num % this.numPegs);
                num = num / this.numPegs;
            }
            return stateString;
        }

        private long StateAllEqual(int pegNumber)
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

        private byte[] ArrayAllEqual(byte pegNumber)
        {
            byte[] arr = new byte[this.numDiscs];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = pegNumber;
            return arr;
        }

        private void ResetArray(bool[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = true;
        }
    }
}

