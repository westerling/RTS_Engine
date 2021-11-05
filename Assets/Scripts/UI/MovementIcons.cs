using UnityEngine;

public class MovementIcons : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Icons;

    [SerializeField]
    private GameObject m_MoveIcon;


    public GameObject[] Icons
    {
        get => m_Icons;
    }
    public GameObject MoveIcon 
    {
        get => m_MoveIcon; 
        set => m_MoveIcon = value; 
    }
}
