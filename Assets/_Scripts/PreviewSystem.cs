using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;

    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GameObject previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    [SerializeField] private Material previewMaterialInstance;

    [SerializeField] private int rotationObjectIndex = 1;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        // 머터리얼 
        previewMaterialInstance = new Material(previewMaterialPrefab);
        // 인디케이터 끄기 
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, int rotationObjectIndex, Vector2Int size)
    { 
        this.rotationObjectIndex = rotationObjectIndex;
        rotationObjectIndex = 1;

        // 오브젝트 생성
        previewObject = Instantiate(prefab);

        // PreviewPreview
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }

        // PrepareCursor
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }

        cellIndicator.SetActive(true);
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        // MovePreview
        previewObject.transform.position = new Vector3(
            position.x,
            position.y + previewYOffset,
            position.z);



        // MoveCursor
        cellIndicator.transform.position = position;

        // ApplyFeedback
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
        previewMaterialInstance.color = c;
    }

}
