using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] Grid grid;
    [SerializeField] private GameObject gridVisualization;

    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] PreviewSystem preview;

    public PlacementDataDic furnitureData, floorData;


    public List<GameObject> placedGameobjects = new List<GameObject>();

    [SerializeField] private int selectedObjectIndex = -1;
    [SerializeField] private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [Header("���콺�� ������")]
    [SerializeField] private Vector3 mousePosition;
    [Header("�׸����� ������")]
    [SerializeField] private Vector3Int gridPosition;
    [Header("������Ʈ �����Ǹ� �� ������Ʈ�� ������")]
    [SerializeField] private Vector3 createObjectPosition;

    public bool placementValidity;

    private void Start()
    {
        StopPlacement();
        floorData = new PlacementDataDic();
        furnitureData = new PlacementDataDic();
    }

    // ��ġ ����
    public void StartPlacement(int ID)
    {
        // ���� ���õ� �ε��� �ֱ�
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        // �ε����� ���������� �������� �ʾҴٸ� (���� ó��)
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        // �׸��� ����Ʈ�� �Ҵ�.
        gridVisualization.SetActive(true);

        // ������ �׸���
        preview.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size
            );

        // ���콺 ��ư Ŭ���Ŀ�
        inputManager.onClicked += Placestructure;
        inputManager.onExit += StopPlacement;
    }

    // ��ġ ��� ���� 
    public void StopPlacement()
    {
        // ���õ� ������Ʈ�� 
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();

        inputManager.onClicked -= Placestructure;
        inputManager.onExit -= StopPlacement;

        //������ ������Ʈ�� �������� �ٽ� ������
        createObjectPosition = Vector3.zero;
    }

    /*
    public void SetObjectRotationment(int id)
    {
        StopPlacement();
        StartPlacement(id);
    }
    */


    // ���콺 Ŭ���Ŀ� 
    // ���콺 ��ǥ�� �� ��ǥ�� ��ȯ�Ͽ� 
    // ������Ʈ �����Ŀ� �� ��ǥ�� �ٽ� ���� ��ǥ�� ��ȯ�Ͽ� 
    // ������Ʈ�� �������� �����մϴ�.
    private void Placestructure()
    {
        //������ ��ġ�� UI�� �ִٸ� ���� ���� (���� ó��)
        if (inputManager.IsPointerOverUI())
        {
            return;
        }


        /// �߿� �κ� !!
        /// // grid.WorldToCell �� ���忡 �������� �������� �׸��忡 ���� �������� ������
        /// // grid.CellToWorld �� �̹� ��ȯ�� �� �������� ���� ���������� �ٽ� ���� ���� 
        /// // �̰� �̳� �߿� !!

        ////////////////////////
        // ���콺 Ŭ���Ŀ� �ϴ� �۾� 
        ////////////////////////
        ///
        // ���콺�� ������ ��������
        mousePosition = inputManager.GetSelectedMapPosition();
        // ũ������ ������ �������� 
        gridPosition = grid.WorldToCell(mousePosition);

        //
        placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        // ��ġ�� �ȵǿ�
        if (placementValidity == false)
        {
            return;
        }

        //////////////////////////
        // ������ �������� �������� ������Ʈ�� �����մϴ�.
        /////////////////////////

        // ���� ��ġ ���ۿ��� ���õ� ������Ʈ �ε����� �������� ������Ʈ�� ���� 
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        //������ ������Ʈ�� �������� �׸��� �������� �����ͼ� �ֱ�
        newObject.transform.position = grid.CellToWorld(gridPosition);
        
        //������ ������Ʈ ����Ʈ�� �����Ͽ� ����
        placedGameobjects.Add(newObject);
        //������ ������Ʈ�� �������� �켱 ������ 
        createObjectPosition = newObject.transform.position;

        // �������� ������Ʈ���� ���� 
        PlacementDataDic selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        // �������� ������Ʈ���� �����Ͽ� ������ ������..
        // ���������� �����Ͽ� ���߿� ������ ������ �̹����� ���ü� �ֵ���
        // �浹 ó�� �ϱ� ���� ����
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameobjects.Count - 1);

        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }


    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // �������� ������Ʈ���� ����
        PlacementDataDic selectData = database.objectsData[selectedObjectIndex].ID == 0? floorData : furnitureData;
        return selectData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }



    private void Update()
    {
        // ���õ� �ε����� ���ٸ� �������� ����
        if (selectedObjectIndex < 0)
            return;

        // ���콺�� ������ ��������
        mousePosition = inputManager.GetSelectedMapPosition();
        // ���콺�� ���������� �׸��� ������ ã��
        gridPosition = grid.WorldToCell(mousePosition);
        
        // �ٸ� ���� �̵��ߴٸ�
        if(lastDetectedPosition != gridPosition)
        {
            placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            // ���콺 ������ ����
            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }




    }

}
