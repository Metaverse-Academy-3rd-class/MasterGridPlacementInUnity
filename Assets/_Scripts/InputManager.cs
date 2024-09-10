using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
 
    private Vector3 lastPosition;

    [SerializeField] LayerMask placementLayermask;

    public event Action onClicked, onExit;

    private void Update()
    {
        // 마우스를 누른후에 
        if (Input.GetMouseButtonDown(0))
        { 
            onClicked?.Invoke();
        }

        // esc를 누른후에 
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            onExit?.Invoke();
        }
    }

    public Vector3 GetSelectedMapPosition()
    { 
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    // 포인터 위치에 UI요소가 있는지 확인
    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
