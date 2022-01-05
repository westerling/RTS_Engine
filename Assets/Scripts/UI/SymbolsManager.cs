using UnityEngine;

public class SymbolsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HealthBar = null;

    public GameObject HealthBar 
    {
        get => m_HealthBar;
    }
}
