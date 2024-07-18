using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GridGroup : MonoBehaviour
{
    private Vector3 thisVec;
    public E_GroupType groupType;
    private List<BrickBase> cubeUp = new List<BrickBase>();
    private List<BrickBase> cubeDown = new List<BrickBase>();
    private bool horizon = true;


    public void CreateGroup(E_GroupType type)
    {
        transform.position = GridManager.instance.grid.GetNearestPointOnGrid(transform.position);
        thisVec = transform.position;
        groupType = type;
        if (groupType == E_GroupType.GRPLine10BotToTop)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 grisPos = GridManager.instance.grid.GetNearestPointOnGrid(thisVec);

                int depth = GridManager.instance.CheckGrid(grisPos);

                BrickBase _go = Instantiate(PrefabStorage.Instance.BrickBase);
                GridManager.instance.ListCubeInLevel.Add(_go);
                _go.transform.position = grisPos + new Vector3(0, 0.2f, 0) * i;
                _go.transform.parent = GridManager.instance.transform;
                _go.InitData(grisPos, depth - 1);
                _go.gameObject.SetActive(true);
            }
        }
        else if (groupType == E_GroupType.GRPLine10LeftToRight)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 grisPos = GridManager.instance.grid.GetNearestPointOnGrid(thisVec);
                int depth = GridManager.instance.CheckGrid(grisPos);

                BrickBase _go = Instantiate(PrefabStorage.Instance.BrickBase);
                GridManager.instance.ListCubeInLevel.Add(_go);
                _go.transform.position = grisPos + new Vector3(0.25f, 0, 0) * i;
                _go.transform.parent = GridManager.instance.transform;
                _go.InitData(grisPos, depth - 1);
                _go.gameObject.SetActive(true);
            }
        }
        else if (groupType == E_GroupType.GRPLine10RightToLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 grisPos = GridManager.instance.grid.GetNearestPointOnGrid(thisVec);
                int depth = GridManager.instance.CheckGrid(grisPos);

                BrickBase _go = Instantiate(PrefabStorage.Instance.BrickBase);
                GridManager.instance.ListCubeInLevel.Add(_go);
                _go.transform.position = grisPos + new Vector3(-0.25f, 0, 0) * i;
                _go.transform.parent = GridManager.instance.transform;
                _go.InitData(grisPos, depth - 1);
                _go.gameObject.SetActive(true);
            }
        }
        else if (groupType == E_GroupType.TUTORIAL)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 grisPos = GridManager.instance.grid.GetNearestPointOnGrid(thisVec);
                int depth = GridManager.instance.CheckGrid(grisPos);


                BrickBase _go = Instantiate(PrefabStorage.Instance.BrickBase);
                GridManager.instance.ListCubeInLevel.Add(_go);
                _go.transform.position = grisPos + new Vector3(0, 0.2f, 0) * i;
                _go.transform.parent = GridManager.instance.transform;
                _go.InitData(grisPos, depth - 1);
                _go.gameObject.SetActive(true);
            }
        }
    }

}
