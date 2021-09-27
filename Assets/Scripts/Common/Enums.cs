public enum Resource
{
    Food = 0,
    Gold = 1,
    Stone = 2,
    Wood = 3
}

public enum FoodType
{
    Farm,
    Forage,
    Wild,
    Domested
}

public enum Domain  
{
    Ground,
    Sea,
    Air
}

public enum SelectedEntity
{
    Unit,
    Army,
    Building,
    Resource,
    Nature,
    Mixed,
    None
}

public enum AttackStyle
{
    None = 0,
    Melee = 1,
    Pierce = 2,
}

public enum EntityType
{
    None,
    Mixed,
    Villager,
    Archer,
    Swordsman,
    Infantry,
    Pikeman,
    Scout,
    Mage,
    Cavalry,
    Siege,
    House,
    Tower,
    Dropoff,
    Training,
    Transport,
    Warship,
    Caravan,
    Upgrade,
    Resource
}

public enum Task
{
    Idle = 0,
    Move = 1,
    Build = 2,
    Attack = 3,
    Collect = 4,
    Deliver= 5
}

public enum StanceType
{
    Defensive = 0,
    Offensive = 1,
    Flee = 2
}

public enum AttributeType
{
    Attack,
    HitPoints,
    LineOfSight,
    MeleeArmor,
    PierceArmor,
    Range,
    RateOfFire,
    Speed,
    Accuracy,
    ProjectileSpeed,
    Delay,
    Garrison,
    Population,
    Farmer,
    Forager,
    GoldMiner,
    Hunter,
    Lumberjack,
    StoneMiner,
    CarryCapacity,
    Training,
    CostGold,
    CostFood,
    CostStone,
    CostWood
}

public enum Faction
{
    Birka = 0,
    Gistad = 1,
    Skara = 2,
    Uppsala = 3
}

public enum UpgradeEffect
{
    None = 0,
    Addition = 1,
    Multiplication = 2,
    NewValue = 3
}

public enum ResearchType
{
    Building = 0,
    Unit = 1,
    Upgrade = 2
}

public enum CursorType
{
    Normal = 0,
    Attack = 1,
    Build = 2,
    CollectFood = 3,
    Mine = 4,
    Axe = 5,
}

public enum InfoPanelType
{
    None,
    Single,
    Army,
    Forager,
    Research
}

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}