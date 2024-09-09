using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData 
{
    // 충돌 처리를 위해 저장
    Dictionary<Vector3Int, PlacementData> placedObjects = new ();

    public void AddOjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    { 
        //포지션 계산
        List<Vector3Int> posisitonToOccupy = CalculatePositions(gridPosition, objectSize);
        //PlacementData data = new PlacementData(posisitonToOccupy, ID, placedObjectIndex);
        

        foreach (var pos in posisitonToOccupy)
        {
            // 중복 체크
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception("이미 포함되어 있습니다.");
            }

            //placedObjects[pos] = data; // 저장
        }
    }

    // 그리드 포지션에 사이즈 더해서 계산하기
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new ();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    // 배치 가능 여부 
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        foreach (var pos in positionToOccupy)
        {
            // 중복이라면 
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }

        // 중복없음
        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
        {
            return -1;
        }
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
       
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }

       
    }
}

/*
// 저장 데이터 객체
public class PlacementData
{
    public List<Vector3Int> occupiedPositions; // 포지션

    public int ID {  get; private set; }

    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;

    }

}
*/

