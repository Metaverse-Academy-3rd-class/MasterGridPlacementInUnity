using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[System.Serializable]
public class PlacementDataDic 
{
    [SerializedDictionary("Vector3Int", "PlacementData")]
    public SerializedDictionary<Vector3Int, PlacementData> PlacementDataDictionary;

    // ������ ����
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int iD, int placedObjectIndex, int driectionIndex, Vector2Int dynamicObjectSize) // ���� ���� ������, ������Ʈ�� ������, ������Ʈ�� ���̵�, ���� ����Ʈ�� ���̵� 
    {
        // �켱 ����ؼ� ������ ��ŭ�� ���� ����Ʈ�� ����
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, driectionIndex, dynamicObjectSize);

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
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize, int driectionIndex, Vector2Int dynamicObjectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();


        Debug.Log("CalculatePositions gridPosition = " + gridPosition); 
        Debug.Log("CalculatePositions dynamicObjectSize = " + dynamicObjectSize);


        if (dynamicObjectSize.x != dynamicObjectSize.y && (driectionIndex == 1 || driectionIndex == 3))
        {
            //float offSet = 
            gridPosition = new Vector3Int(gridPosition.x, 0, gridPosition.z - (dynamicObjectSize.x + (dynamicObjectSize.y - 2) - ((dynamicObjectSize.x - 1) * 2)));
        }

        objectSize = dynamicObjectSize;

        // �������� �������� �����Ϳ� ����
        // ����� �����Ͽ� ��ü�� �׸��� ����Ʈ�� ����
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));

                Debug.Log($"CalculatePositions returnVal ({x},{y}) = ({returnVal[x].x}, {returnVal[x].z})");
            }
        }



        return returnVal;
    }

  

    // ��ġ�� �������� �Ǵ�
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int size, int driectionIndex, Vector2Int dynamicObjectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, size, driectionIndex, dynamicObjectSize);

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
