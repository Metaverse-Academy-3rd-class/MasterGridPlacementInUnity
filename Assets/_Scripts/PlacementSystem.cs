using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] Grid grid;
    [SerializeField] private GameObject gridVisualization;

    [SerializeField] private ObjectsDatabaseSO database;


    public PlacementDataDic furnitureData, floorData;

    [SerializeField] private Renderer previewRenderer;

    public List<GameObject> placedGameobjects = new List<GameObject>();

    [SerializeField] private int selectedObjectIndex = -1;

    [Header("마우스의 포지션")]
    [SerializeField] private Vector3 mousePosition;
    [Header("그리드의 포지션")]
    [SerializeField] private Vector3Int gridPosition;
    [Header("오브젝트 생성되면 그 오브젝트의 포지션")]
    [SerializeField] private Vector3 createObjectPosition;

    public bool placementValidity;

    private void Start()
    {
        StopPlacement();
        floorData = new PlacementDataDic();
        furnitureData = new PlacementDataDic();


    }

    // 배치 시작
    public void StartPlacement(int ID)
    {
        // 현재 선택된 인덱스 넣기
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        // 인덱스가 정상적으로 생성되지 않았다면 (예외 처리)
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        // 그리드 이펙트를 켠다.
        gridVisualization.SetActive(true);
        // 그리드 포지션 인디게이트를 켠다.
        cellIndicator.SetActive(true);

        // 마우스 버튼 클릭후에
        inputManager.onClicked += Placestructure;
        inputManager.onExit += StopPlacement;
    }

    // 배치 모드 끄기 
    private void StopPlacement()
    {
        // 선택된 오브젝트가 
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);

        inputManager.onClicked -= Placestructure;
        inputManager.onExit -= StopPlacement;

        //생성된 오브젝트의 포지션을 다시 제거함
        createObjectPosition = Vector3.zero;
    }


    // 마우스 클릭후에 
    // 마우스 좌표를 셀 좌표로 변환하여 
    // 오브젝트 생성후에 셀 좌표를 다시 월드 좌표로 변환하여 
    // 오브젝트의 포지션을 생성합니다.
    private void Placestructure()
    {
        //포인터 위치에 UI가 있다면 하지 않음 (예외 처리)
        if (inputManager.IsPointerOverUI())
        {
            return;
        }


        /// 중요 부분 !!
        /// // grid.WorldToCell 은 월드에 포지션을 기준으로 그리드에 셀의 포지션을 가져옴
        /// // grid.CellToWorld 는 이미 변환된 셀 포지션을 월드 포지션으로 다시 변경 해줌 
        /// // 이거 겁나 중요 !!

        ////////////////////////
        // 마우스 클릭후에 하는 작업 
        ////////////////////////
        ///
        // 마우스의 포지션 가져오기
        mousePosition = inputManager.GetSelectedMapPosition();
        // 크리드의 포지션 가져오기 
        gridPosition = grid.WorldToCell(mousePosition);

        //
        placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        // 배치가 안되요
        if (placementValidity == false)
        {
            return;
        }

        //////////////////////////
        // 가져온 포지션을 기준으로 오브젝트를 생성합니다.
        /////////////////////////

        // 최초 배치 시작에서 선택된 오브젝트 인덱스를 기준으로 오브젝트를 생성 
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        //생성된 오브젝트의 포지션을 그리드 포지션을 가져와서 넣기
        newObject.transform.position = grid.CellToWorld(gridPosition);
        
        //생성된 오브젝트 리스트에 저장하여 관리
        placedGameobjects.Add(newObject);
        //생성된 오브젝트의 포지션을 우선 저장함 
        createObjectPosition = newObject.transform.position;

        // 장판인지 오브젝트인지 구분 
        PlacementDataDic selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        // 장판인지 오브젝트인지 구분하여 데이터 저장함..
        // 지속적으로 저장하여 나중에 빨간색 프리뷰 이미지가 나올수 있도록
        // 충돌 처리 하기 위해 저장
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameobjects.Count - 1);
    }


    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // 장판인지 오브젝트인지 구분
        PlacementDataDic selectData = database.objectsData[selectedObjectIndex].ID == 0? floorData : furnitureData;

        //
        return selectData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void Update()
    {
        // 선택된 인덱스가 없다면 실행하지 않음
        if (selectedObjectIndex < 0)
            return;

        // 마우스의 포지션 가져오기
        mousePosition = inputManager.GetSelectedMapPosition();
        // 마우스의 포지션으로 그리드 포지션 찾기
        gridPosition = grid.WorldToCell(mousePosition);
        
        // 마우스 포지션 대입
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

}
