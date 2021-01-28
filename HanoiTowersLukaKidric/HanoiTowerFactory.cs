using HanoiTowersLukaKidric.Core;
using HanoiTowersLukaKidric.TypeModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanoiTowersLukaKidric
{
    public  class HanoiTowerFactory
    {
        
        public int HanoiTowerFactoryGet(int numDiscs, int numPegs, HanoiType type)
        {
            HanoiTypeModel factory = null;
            switch (type)
            {
                case HanoiType.K13_01:
                    {
                         factory = new HanoiTypeK13_01Model(numDiscs, numPegs, type);
                    }

                    break;
                case HanoiType.K13_12:
                    {
                         factory = new HanoiTypeK13_12Model(numDiscs, numPegs, type);
                    }
                    break;

                case HanoiType.K13e_01:
                case HanoiType.K13e_12:
                case HanoiType.K13e_23:
                case HanoiType.K13e_30:
                    {
                         factory = new HanoiTypeK13eModel(numDiscs, numPegs, type);
                    }
                    break;
                case HanoiType.K4e_01:
                case HanoiType.K4e_12:
                case HanoiType.K4e_23:
                    {
                        factory = new HanoiTypeK4eModel(numDiscs, numPegs, type);
                    }
                    break;
                case HanoiType.C4_01:
                case HanoiType.C4_12:
                    {
                        factory = new HanoiTypeC4Model(numDiscs, numPegs, type);
                    }
                    break;
                case HanoiType.P4_01:
                case HanoiType.P4_12:
                case HanoiType.P4_23:
                case HanoiType.P4_31:
                    {
                        factory = new HanoiTypeP4Model(numDiscs, numPegs, type);
                    }
                    break;
            }
            var length = factory.ShortestPathForSmallDimension(0, out _);
            return length;
            

        }
        public  int numDiscs;
        public  int numPegs;
        public HanoiType type;

    }
}
