using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectCheckListOS : ScriptableObject
{
    // 충돌 처리를 위해 저장
    Dictionary<Vector3Int, PlacementData> placedObjects;

}

// 저장 데이터 객체
public class PlacementData
{
    public List<Vector3Int> occupiedPositions; // 포지션

    public int ID { get; private set; }

    public int PlacedObjectIndex { get; private set; }

    public void Init(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }

    /*
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;

    }
    */

}