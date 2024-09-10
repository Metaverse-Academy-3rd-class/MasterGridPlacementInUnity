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

    // ������ ����
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int iD, int placedObjectIndex) // ���� ���� ������, ������Ʈ�� ������, ������Ʈ�� ���̵�, ���� ����Ʈ�� ���̵� 
    {
        // �켱 ����ؼ� ������ ��ŭ�� ���� ����Ʈ�� ����
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        PlacementData data = new PlacementData(positionToOccupy, iD, placedObjectIndex);

        // ����� ���� ó��
        foreach (var pos in positionToOccupy)
        {
            if (PlacementDataDictionary.ContainsKey(pos))
            {
                throw new Exception("�ߺ� �Դϴ�.");
            }

            PlacementDataDictionary[pos] = data;
        }
    }

    // ������Ʈ�� ��ü ���Ե� �׸��� ��ǥ ����Ʈ�� ����
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();

        // �������� �������� �����Ϳ� ����
        // ����� �����Ͽ� ��ü�� �׸��� ����Ʈ�� ����
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    // ��ġ�� �������� �Ǵ�
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int size)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, size);

        // ��ġ�� ���� ����
        foreach (var pos in positionToOccupy)
        {
            if (PlacementDataDictionary.ContainsKey(pos))
            {
                Debug.Log($"�ߺ��� ������ �Դϴ� {pos}");
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
