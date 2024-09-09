using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectCheckListOS : ScriptableObject
{
    // �浹 ó���� ���� ����
    Dictionary<Vector3Int, PlacementData> placedObjects;

}

// ���� ������ ��ü
public class PlacementData
{
    public List<Vector3Int> occupiedPositions; // ������

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