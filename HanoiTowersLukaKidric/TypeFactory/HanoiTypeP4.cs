using HanoiTowersLukaKidric.Core;
using HanoiTowersLukaKidric.TypeModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric.TypeFactory
{
    class HanoiTypeP4 : HanoiTowerFactory
    {
        private HanoiTypeP4Model P4 = null;
        public HanoiTypeP4(int numDiscs, int numPegs, HanoiType type)
        {
            P4 = new HanoiTypeP4Model(numDiscs, numPegs, type);
        }

        public override int ProcessHanoiTowers(int searchMode, out string path)
        {

            return P4.ShortestPathForSmallDimension(searchMode, out path);
        }
    }
}
