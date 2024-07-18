using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using Unity.Collections.LowLevel.Unsafe;

public class BrickBase : MonoBehaviour
{
    [SerializeField] public Vector2 thisVec;
    [SerializeField] public int depth = 0;
    [SerializeField] public E_TypeBrick _typeBrick;
    [SerializeField] public SpriteRenderer _SpriteRenderer;
    [SerializeField] public SpriteRenderer _SpriteRendererTile;
    [SerializeField] protected Collider2D _circleCollider2D;
    public Vector3 StartPosition;

    private Color ColorGray = new Color(0.7f, 0.7f, 0.7f);
    private Color ColorWhite = new Color(1f, 1f, 1f, 1f);
    private bool isSorted;
    public bool IsAddCompleted;
    public bool IsMoved;


    Tweener T_Move;

    private void Start()
    {
        StartPosition = transform.position;

        StartCheckGrid();
    }

    public void InitData(Vector2 vec, int _depth)
    {
        GridManager.instance.listGrid.Add(new Vector3(vec.x, vec.y, _depth));
        GridManager.instance.listGridPlace.Add(new Vector3(vec.x, vec.y, _depth));
        /*        GridManager.instance.listCube.Add(this);*/
        GridManager.instance.listGrid.Add(new Vector3(vec.x - 1, vec.y, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x, vec.y - 1, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x - 1, vec.y - 1, _depth));
        transform.position = new Vector3(transform.position.x, transform.position.y, _depth * -0.05f);
        thisVec = vec;
        depth = _depth;

        if (_depth < -1)
        {
            TurnBlack();
        }
        else
        {
            TurnWhite();
        }
    }

    public void initDataTypeStartGame(Sprite Icon, E_TypeBrick Type)
    {
        _SpriteRenderer.sprite = Icon;
        _typeBrick = Type;
    }

    /*    public void InitDataType()
        {
            BRICK brick = GameManager.instance.DataBrick.GetDataBrickByType(_typeBrick);
            if (brick != null)
            {
                _SpriteRenderer.sprite = brick.Icon;
            }
        }*/

    private void OnMouseDown()
    {
        if (!GameManager.instance.isInGame) return;
        isSorted = false;
        if (GameManager.instance.UiController.UiGamePlay.ListItemPicked.CheckIsFullItem()) return;
        if (IsMoved) return;

        if (!CheckGridOverlap()) return;


        DeleteBox(thisVec);
        GridManager.instance.CheckLayerGrid();
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Tile);

        if (GameManager.instance.UiController.UiGamePlay.ListItemPicked.L_Stored.Count > 0)
        {
            for (int i = 0; i < GameManager.instance.UiController.UiGamePlay.ListItemPicked.L_Stored.Count; i++)
            {
                if (_typeBrick == GameManager.instance.UiController.UiGamePlay.ListItemPicked.L_Stored[i]._typeBrick)
                {
                    CheckBrickTileReturned();
                    GameManager.instance.UiController.UiGamePlay.ListItemPicked.SortAndMoveElement(this);
                    GameManager.instance.UiController.UiGamePlay.ListItemPicked.AddDic(_typeBrick, this);
                    isSorted = true;
                    break;
                }
            }
        }
        else
        {
            CheckBrickTileReturned();
            GameManager.instance.UiController.UiGamePlay.ListItemPicked.AddDic(_typeBrick, this);
            MoveToTarget(GameManager.instance.UiController.UiGamePlay.ListItemPicked.L_Items[0].transform);
            isSorted = true;
        }

        if (!isSorted && GameManager.instance.UiController.UiGamePlay.ListItemPicked.L_Stored.Count > 0)
        {
            CheckBrickTileReturned();
            GameManager.instance.UiController.UiGamePlay.ListItemPicked.AddDic(_typeBrick, this);
            int index = GameManager.instance.UiController.UiGamePlay.ListItemPicked.L_Stored.Count - 1;
            MoveToTarget(GameManager.instance.UiController.UiGamePlay.ListItemPicked.L_Items[index].transform);
        }
    }


    public void MoveToTarget(Transform target, System.Action complete = null)
    {
        T_Move = transform.DOMove(new Vector3(target.position.x, target.position.y, transform.position.z), 0.3f).SetEase(Ease.Linear);
        //GameManager.instance.CurrentLevel.L_BrickInLevel.Remove(this);

        T_Move.OnComplete(() =>
        {
            GameManager.instance.UiController.UiGamePlay.ListItem.CheckCanUseItem();
            complete?.Invoke();
            GameManager.instance.UiController.UiGamePlay.ListItemPicked.SortElement();
            IsAddCompleted = true;
            GameManager.instance.UiController.UiGamePlay.ListItemPicked.CheckDic(this);
            GridManager.instance.ListCubeInLevel.Remove(this);
        });

        IsMoved = true;
        transform.SetParent(null);
    }

    private void CheckBrickTileReturned()
    {
        ListItem _listItem = GameManager.instance.UiController.UiGamePlay.ListItem;

        if (_listItem.L_ContainsItemTileReturn.Contains(this))
        {
            _listItem.L_ContainsItemTileReturn.Remove(this);
        }
    }

    public void DeleteBox(Vector2 pos)
    {
        Vector3 grisPos = GridManager.instance.grid.GetNearestPointOnGrid(pos);
        DeleteTile(grisPos);
    }

    public void DeleteTile(Vector2 vec)
    {
        float depth = int.MinValue;
        for (int i = 0; i < GridManager.instance.listGrid.Count; i++)
        {
            if (GridManager.instance.listGrid[i].x == vec.x && GridManager.instance.listGrid[i].y == vec.y)
            {
                if (GridManager.instance.listGrid[i].z > depth)
                {
                    depth = GridManager.instance.listGrid[i].z;
                }
            }
        }
        CheckDeleteGrid(new Vector3(vec.x, vec.y, depth));
    }

    #region CheckDeleteGrid
    public void CheckDeleteGrid(Vector3 vec)
    {
        if (depth == vec.z)
        {
            if (CheckGridOverlap())
            {
                if (vec.x == thisVec.x && vec.y == thisVec.y)
                {
                    KillThisCube();
                    return;
                }

                if (vec.x == thisVec.x && vec.y == thisVec.y - 1)
                {

                    KillThisCube();
                    return;
                }

                if (vec.x == thisVec.x - 1 && vec.y == thisVec.y)
                {
                    KillThisCube();
                    return;
                }

                if (vec.x == thisVec.x - 1 && vec.y == thisVec.y - 1)
                {
                    KillThisCube();
                    return;
                }
            }
        }
    }

    private void StartCheckGrid()
    {
        StartCoroutine(CheckGrid());
    }

    private IEnumerator CheckGrid()
    {
        yield return null;
        CheckGridOverlap();
    }
    #endregion
    public bool CheckGridOverlap()
    {
        for (int i = 0; i < GridManager.instance.listGrid.Count; i++)
        {
            if (GridManager.instance.listGrid[i].z > depth)
            {
                if (GridManager.instance.listGrid[i].x == thisVec.x && GridManager.instance.listGrid[i].y == thisVec.y)
                {
                    TurnBlack();
                    return false;
                }

                if (GridManager.instance.listGrid[i].x == thisVec.x - 1 && GridManager.instance.listGrid[i].y == thisVec.y)
                {
                    TurnBlack();
                    return false;
                }

                if (GridManager.instance.listGrid[i].x == thisVec.x && GridManager.instance.listGrid[i].y == thisVec.y - 1)
                {
                    TurnBlack();
                    return false;
                }

                if (GridManager.instance.listGrid[i].x == thisVec.x - 1 && GridManager.instance.listGrid[i].y == thisVec.y - 1)
                {
                    TurnBlack();
                    return false;
                }
            }
        }

        TurnWhite();
        return true;
    }

    public void UndoThisItem(Vector2 vec, int _depth)
    {
        GridManager.instance.listGrid.Add(new Vector3(vec.x, vec.y, _depth));
        GridManager.instance.listGridPlace.Add(new Vector3(vec.x, vec.y, _depth));
        GridManager.instance.ListCubeInLevel.Add(this);
        GridManager.instance.listGrid.Add(new Vector3(vec.x - 1, vec.y, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x, vec.y - 1, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x - 1, vec.y - 1, _depth));
    }

    public void KillThisCube()
    {
        GridManager.instance.listGrid.Remove(new Vector3(thisVec.x, thisVec.y, depth));
        GridManager.instance.listGridPlace.Remove(new Vector3(thisVec.x, thisVec.y, depth));
        GridManager.instance.listGrid.Remove(new Vector3(thisVec.x, thisVec.y - 1, depth));
        GridManager.instance.listGrid.Remove(new Vector3(thisVec.x - 1, thisVec.y, depth));
        GridManager.instance.listGrid.Remove(new Vector3(thisVec.x - 1, thisVec.y - 1, depth));
    }

    public void KillTween()
    {
        T_Move?.Kill();
        /*        tweern?.Kill();
                Rotation?.Kill();*/
    }

    public void TurnBlack()
    {
        _SpriteRendererTile.color = ColorGray;
        _SpriteRenderer.color = ColorGray;
    }

    public void TurnWhite()
    {
        _SpriteRenderer.color = ColorWhite;
        _SpriteRendererTile.color = ColorWhite;
    }

    private void OnDisable()
    {
        KillTween();
    }

}
