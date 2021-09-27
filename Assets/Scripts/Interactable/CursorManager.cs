using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D[] m_Cursors;

    private CursorType m_CursorType = CursorType.Normal;

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
}
