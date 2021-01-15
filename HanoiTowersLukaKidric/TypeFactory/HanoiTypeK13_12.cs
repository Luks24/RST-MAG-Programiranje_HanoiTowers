using HanoiTowersLukaKidric.Core;
using HanoiTowersLukaKidric.TypeModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric
{
    class HanoiTypeK13_12 : HanoiTowerFactory
    {
        private HanoiTypeK13_12Model K13_12 = null;
        public HanoiTypeK13_12(int numDiscs, int numPegs, HanoiType type)
        {
            K13_12 = new HanoiTypeK13_12Model(numDiscs, numPegs, type);
        }

        public override int ProcessHanoiTowers(int searchMode, out string path)
        {

            return K13_12.ShortestPathForSmallDimension(searchMode, out path);
        }
    }
}
