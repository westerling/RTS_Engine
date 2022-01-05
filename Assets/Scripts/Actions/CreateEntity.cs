using UnityEngine;
using System;

[Serializable]
public class CreateEntity
{
    [SerializeField]
    private int m_Position;

    [SerializeField]
    private InteractableGameEntity m_Object;

    public InteractableGameEntity Object { get => m_Object; set => m_Object = value; }
    public int Position { get => m_Position; set => m_Position = value; }
}
