using HanoiTowersLukaKidric.Core;
using HanoiTowersLukaKidric.TypeModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric.TypeFactory
{
    class HanoiTypeC4 : HanoiTowerFactory
    {
        private HanoiTypeC4Model C4 = null;
        public HanoiTypeC4(int numDiscs, int numPegs, HanoiType type)
        {
            C4 = new HanoiTypeC4Model(numDiscs, numPegs, type);
        }

        public override int ProcessHanoiTowers(int searchMode, out string path)
        {

            return C4.ShortestPathForSmallDimension(searchMode, out path);
        }
    }
}
