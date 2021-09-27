using System;

[Serializable]
public class Cost
{

    public int gold;
    public int wood;
    public int food;
    public int stone;

    public Cost(int gold, int wood, int food, int stone)
    {
        this.gold = gold;
        this.wood = wood;
        this.food = food;
        this.stone = stone;
    }
}