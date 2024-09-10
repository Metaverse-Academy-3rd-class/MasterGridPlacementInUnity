using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlacementDataDic 
{
    [SerializedDictionary("Vector3Int", "PlacementData")]
    public SerializedDictionary<Vector3Int, PlacementData> PlacementDataDictionary;

    // 데이터 저장
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int iD, int placedObjectIndex) // 현재 생성 포지션, 오브젝트의 사이즈, 오브젝트의 아이디, 생성 리스트의 아이디 
    {
        // 우선 계산해서 사이즈 만큼에 백터 리스트를 구함
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        PlacementData data = new PlacementData(positionToOccupy, iD, placedObjectIndex);

        // 저장시 예외 처리
        foreach (var pos in positionToOccupy)
        {
            if (PlacementDataDictionary.ContainsKey(pos))
            {
                throw new Exception("중복 입니다.");
            }

            PlacementDataDictionary[pos] = data;
        }
    }

    // 오브젝트에 전체 포함된 그리드 좌표 리스트르 구함
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();

        // 현재점을 기준으로 데이터에 넣음
        // 사이즈를 포함하여 전체의 그리드 리스트를 구함
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    // 배치가 가능한지 판단
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int size)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, size);

        // 배치시 가능 여부
        foreach (var pos in positionToOccupy)
        {
            if (PlacementDataDictionary.ContainsKey(pos))
            {
                Debug.Log($"중복된 포지션 입니다 {pos}");
                return false;
            }
        }
        return true;
    }

    [System.Serializable]
    public class PlacementData
    {
        public List<Vector3Int> occupiedPositions;
        public int ID;
        public int PlacedObjectIndex;

        public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
        {
            this.occupiedPositions = occupiedPositions;
            ID = iD;
            PlacedObjectIndex = placedObjectIndex;
        }
    }
}
