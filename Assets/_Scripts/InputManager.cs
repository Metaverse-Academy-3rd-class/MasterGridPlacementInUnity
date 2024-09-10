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
        // ���콺�� �����Ŀ� 
        if (Input.GetMouseButtonDown(0))
        { 
            onClicked?.Invoke();
        }

        // esc�� �����Ŀ� 
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

    // ������ ��ġ�� UI��Ұ� �ִ��� Ȯ��
    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
