using Mirror;
using System.Collections;
using UnityEngine;

public class StockMarket : NetworkBehaviour
{
    private int m_Fee = 30;

    private int m_WoodPrice = 100;
    private int m_FoodPrice = 100;
    private int m_StonePrice = 100;

    private float m_IndexFundResidental = 20f;
    private float m_IndexFundLumber = 40f;
    private float m_IndexFundFood = 20f;
    private float m_IndexFundMining = 20f;
    private float m_IndexFund = 20f;

    public int Fee 
    {
        get => m_Fee; 
        set => m_Fee = value; 
    }

    public int WoodPrice 
    {
        get => m_WoodPrice; 
        set => m_WoodPrice = value; 
    }

    public int FoodPrice 
    { 
        get => m_FoodPrice;
        set => m_FoodPrice = value; 
    }

    public int StonePrice 
    { 
        get => m_StonePrice;
        set => m_StonePrice = value; 
    }

    public float IndexFundResidental 
    { 
        get => m_IndexFundResidental; 
        set => m_IndexFundResidental = value;
    }

    public float IndexFundLumber 
    {
        get => m_IndexFundLumber;
        set => m_IndexFundLumber = value;
    }

    public float IndexFundFood 
    {
        get => m_IndexFundFood; 
        set => m_IndexFundFood = value;
    }

    public float IndexFundMining
    {
        get => m_IndexFundMining;
        set => m_IndexFundMining = value; 
    }

    public float IndexFund 
    { 
        get => m_IndexFund;
        set => m_IndexFund = value;
    }
    public override void OnStartServer()
    {
        StartCoroutine(TimedFundUpdate());
    }

    private IEnumerator TimedFundUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateFund(int value, Trend trend)
    {
    }
}
