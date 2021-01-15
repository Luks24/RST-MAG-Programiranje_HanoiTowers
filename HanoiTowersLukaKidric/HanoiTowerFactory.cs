using HanoiTowersLukaKidric.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric
{
    public abstract class HanoiTowerFactory
    {
        public  int numDiscs;
        public  int numPegs;
        public HanoiType type;
        public abstract int ProcessHanoiTowers(int searchMode, out string path);
    }
}
