using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Minimap : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private RectTransform minimapRect = null;

    [SerializeField]
    private float mapScale = 5f;

    [SerializeField]
    private float offset = -5;

    private Transform playerCameraTransform;

    public void OnPointerDown(PointerEventData eventData)
    {
        MoveCamera();
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveCamera();
    }

    private void Update()
    {
        if (playerCameraTransform != null)
        {
            return;
        }

        if (NetworkClient.connection.identity == null)
        {
            return;
        }

        playerCameraTransform = NetworkClient.connection.identity.GetComponent<RtsPlayer>().MainCameraTransform;
    }

    private void MoveCamera()
    {
        var mousePos = Mouse.current.position.ReadValue();

        if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            minimapRect,
            mousePos,
            null, 
            out Vector2 localPoint))
        {
            return;
        }

        var lerp = new Vector2(
            (localPoint.x - minimapRect.rect.x) / minimapRect.rect.width, 
            (localPoint.y - minimapRect.rect.y) / minimapRect.rect.height);

        var newCameraPos = new Vector3(
            Mathf.Lerp(-mapScale, mapScale, lerp.x),
            playerCameraTransform.position.y,
            Mathf.Lerp(-mapScale, mapScale, lerp.y));

        playerCameraTransform.position = newCameraPos + new Vector3(0, 0, offset);
    }


}
