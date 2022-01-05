using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image m_HealthBarImage = null;

    public Image HealthBarImage
    { 
        get => m_HealthBarImage; 
        set => m_HealthBarImage = value; 
    }
}
