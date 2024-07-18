using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;

    public Vector3 GetNearestPointOnGrid(float x, float y, float z)
    {
        Vector3 pos = new Vector3(x, y, z) - transform.localPosition;

        int xCount = Mathf.RoundToInt(pos.x / size);
        int yCount = Mathf.RoundToInt(pos.y / size);
        int zCount = Mathf.RoundToInt(pos.z / size);

        Vector3 result = new Vector3(xCount * size, yCount * size, zCount * size);
        result += transform.localPosition;

        return result;
    }

    public Vector3 GetNearestPointOnGrid(Vector3 pos)
    {
        pos -= transform.localPosition;

        int xCount = Mathf.RoundToInt(pos.x / size);
        int yCount = Mathf.RoundToInt(pos.y / size);
        int zCount = Mathf.RoundToInt(pos.z / size);

        Vector3 result = new Vector3(xCount * size, yCount * size, zCount * size);
        result += transform.localPosition;
        return result;
    }
}
