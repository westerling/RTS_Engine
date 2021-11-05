
using System.Collections.Generic;

public static class Globals
{
    public static int FlatTerrainLayerMask = 1 << 16;

    public static int TerrainLayerMask = 1 << 12;

    public static int BuildingLayerMask = 1 << 13;

    public static int FovLayerMask = 1 << 17;

    public static int WallPlacementLayerMask = TerrainLayerMask | BuildingLayerMask | FovLayerMask;

}
