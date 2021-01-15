using HanoiTowersLukaKidric.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric
{
    class HanoiTypeK13_01 : HanoiTowerFactory
    {
        private HanoiTypeK13_01Model K13_01 = null;
        public HanoiTypeK13_01(int numDiscs, int numPegs, HanoiType type)
        {
            K13_01 = new HanoiTypeK13_01Model(numDiscs, numPegs, type);
        }

        public override int ProcessHanoiTowers(int searchMode, out string path)
        {

            return K13_01.ShortestPathForSmallDimension(searchMode, out path);
        }
        
    }
}
