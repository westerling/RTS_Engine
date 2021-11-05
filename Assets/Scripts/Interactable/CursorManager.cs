using System.Collections;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D[] m_Cursors;

    [SerializeField]
    private GameObject m_MoveIcon;

    private CursorType m_CursorType = CursorType.Normal;

    [SerializeField]
    private GameObject[] m_SelectionIndicators = null;

    public GameObject[] SelectionIndicators
    { 
        get => m_SelectionIndicators; 
        set => m_SelectionIndicators = value; 
    }

    private void Start()
    {
        Cursor.SetCursor(m_Cursors[0], Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorStyle(CursorType cursorType)
    {
        if (cursorType == m_CursorType)
        {
            return;
        }

        switch(cursorType)
        {
            case CursorType.Attack:
                Cursor.SetCursor(m_Cursors[1], Vector2.zero, CursorMode.Auto);
                break;
            case CursorType.Axe:
                Cursor.SetCursor(m_Cursors[2], Vector2.zero, CursorMode.Auto);
                break;
            case CursorType.Build:
            case CursorType.Mine:
                Cursor.SetCursor(m_Cursors[3], Vector2.zero, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(m_Cursors[0], Vector2.zero, CursorMode.Auto);
                break;
        }

        m_CursorType = cursorType;
    }

    public void Flashtarget(GameObject target)
    {
        StopAllCoroutines();
        m_MoveIcon.SetActive(false);
        if (target.TryGetComponent(out Interactable interactable))
        {
            interactable.Flash();
        }
    }

    public void SetCommandCursor(Vector3 point)
    {
        StopAllCoroutines();
        StartCoroutine(CommandCursorCoroutine(point));
    }

    IEnumerator CommandCursorCoroutine(Vector3 point)
    {
        m_MoveIcon.SetActive(true);
        m_MoveIcon.transform.position = point;
        yield return new WaitForSeconds(1);
        m_MoveIcon.SetActive(false);
    }
}
