using HanoiTowersLukaKidric.Core;
using HanoiTowersLukaKidric.TypeModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric.TypeFactory
{
    class HanoiTypeK13e : HanoiTowerFactory
    {
        private HanoiTypeK13eModel K13e = null;
        public HanoiTypeK13e(int numDiscs, int numPegs, HanoiType type)
        {
            K13e = new HanoiTypeK13eModel(numDiscs, numPegs, type);
        }

        public override int ProcessHanoiTowers(int searchMode, out string path)
        {

            return K13e.ShortestPathForSmallDimension(searchMode, out path);
        }
    }
}
