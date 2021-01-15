using HanoiTowersLukaKidric.Core;
using HanoiTowersLukaKidric.TypeModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric.TypeFactory
{
    class HanoiTypeK4e : HanoiTowerFactory
    {
        private HanoiTypeK4eModel K4e = null;
        public HanoiTypeK4e(int numDiscs, int numPegs, HanoiType type)
        {
            K4e = new HanoiTypeK4eModel(numDiscs, numPegs, type);
        }

        public override int ProcessHanoiTowers(int searchMode, out string path)
        {

            return K4e.ShortestPathForSmallDimension(searchMode, out path);
        }
    }
}
