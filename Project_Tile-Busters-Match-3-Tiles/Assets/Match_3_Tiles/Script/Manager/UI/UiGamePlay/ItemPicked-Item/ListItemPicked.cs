using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sequence = DG.Tweening.Sequence;
using System.Collections;
using UnityEngine.Animations;

public class ListItemPicked : MonoBehaviour
{
    public List<Image> L_Items;

    /*[ShowInInspector]*/
    [ShowInInspector] public Dictionary<E_TypeBrick, List<BrickBase>> Dic_TypeCheck = new Dictionary<E_TypeBrick, List<BrickBase>>();
    public List<BrickBase> L_Stored;


    [Header("Slot")]
    public int MaxCapacity = 7;
    public Image AddSlot_img;

    public Image fill_tiles;
    public RectTransform underFill_Tiles;
    public Sprite SpriteFill_Tiles;

    // private Sequence sequence;

    //int TweenSequece = 0;

    Tweener T_Back;

    private void Start()
    {
        AddSlot_img.gameObject.SetActive(false);
    }

    public void InitListItem(Sprite Brick)
    {
        for (int i = 0; i < L_Items.Count; i++)
        {
            L_Items[i].sprite = Brick;
        }
    }

    public void AddDic(E_TypeBrick Type, BrickBase brick)
    {
        if (Dic_TypeCheck.ContainsKey(Type))
        {
            IncreaseDic(Type, brick);
        }
        else
        {
            List<BrickBase> L_Bricks = new List<BrickBase>();
            L_Bricks.Add(brick);
            Dic_TypeCheck.Add(Type, L_Bricks);
            L_Stored.Add(brick);
        }
    }

    void IncreaseDic(E_TypeBrick Type, BrickBase brick)
    {
        List<BrickBase> L_Bricks = Dic_TypeCheck[Type];

        L_Bricks.Add(brick);
        Dic_TypeCheck[Type] = L_Bricks;
        /*L_Stored.Add(brick);*/
    }

    public void CheckDic(BrickBase brick)
    {
        List<BrickBase> L_Bricks = Dic_TypeCheck[brick._typeBrick];


        if (L_Bricks.Count >= 3 && brick.IsAddCompleted)
        {
            if (!L_Bricks[2].IsAddCompleted) return;
            GameManager.instance.UiController.UiGamePlay.ListItem.isCanUseItem = false;
            List<BrickBase> L_3math = L_Bricks.GetRange(0, Mathf.Min(3, L_Bricks.Count));



            if (GridManager.instance.ListCubeInLevel.Count <= 1 && GameManager.instance.UiController.UiGamePlay.ListItem.L_ContainsItemTileReturn.Count <= 0)
            {
                Sequence sequenceComplete = DOTween.Sequence();

                sequenceComplete.Append(L_3math[1].transform.DOMove(L_Items[0].transform.position, 0.4f).SetEase(Ease.Linear));
                sequenceComplete.Join(L_3math[2].transform.DOMove(L_Items[1].transform.position, 0.4f).SetEase(Ease.Linear));

                L_3math[0].transform.DOMoveY(-3f, 0.4f).SetEase(Ease.Linear);
                L_3math[0].transform.DORotate(Vector3.forward * 360, 0.4f, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(() =>
                {
                    sequenceComplete.Play();
                });


                sequenceComplete.OnComplete(() =>
                {
                    if (L_3math[0].transform)
                    {
                        L_3math[0]._SpriteRenderer.sortingOrder = 2;
                        L_3math[0]._SpriteRendererTile.sortingOrder = 2;

                        L_3math[0].transform.DOMove(L_Items[0].transform.position, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            foreach (var item in L_3math)
                            {
                                if (item.transform)
                                {
                                    SimplePool.Spawn(PrefabStorage.Instance.FxMerge, item.transform.position, Quaternion.identity);
                                    Destroy(item.gameObject);
                                    L_Stored.Remove(item);
                                    Dic_TypeCheck[item._typeBrick].Remove(item);

                                    if (Dic_TypeCheck[brick._typeBrick].Count <= 0)
                                    {
                                        Dic_TypeCheck.Remove(brick._typeBrick);
                                    }
                                }
                            }

                            SoundManager.Instance.PlayFxSound(SoundManager.Instance.TileMatch);
                            GameManager.instance.UiController.UiGamePlay.ListItem.CheckCanUseItem();

                            PlayerDataManager.InCreaseLevel(PlayerDataManager.GetCurrentLevel());
                            GameManager.instance.UiController.ProcessWinLose(E_Result.Win);
                        });
                    }

                });
            }
            else
            {
                Sequence sequence = DOTween.Sequence();

                foreach (var item in L_3math)
                {
                    if (item.transform)
                    {
                        sequence.Join(item.transform.DOScale(2f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            SimplePool.Spawn(PrefabStorage.Instance.FxMerge, item.transform.position , Quaternion.identity);

                            Destroy(item.gameObject);
                            L_Stored.Remove(item);
                            Dic_TypeCheck[item._typeBrick].Remove(item);
                        }));
                    }
                }

                sequence.OnComplete(() =>
                {
                    SortElement();

                    if (Dic_TypeCheck[brick._typeBrick].Count <= 0)
                    {
                        Dic_TypeCheck.Remove(brick._typeBrick);
                    }

                    Debug.Log("complete");
                    GameManager.instance.UiController.UiGamePlay.ListItem.isCanUseItem = true;
                    SoundManager.Instance.PlayFxSound(SoundManager.Instance.TileMatch);
                    GameManager.instance.UiController.UiGamePlay.ListItem.CheckCanUseItem();
                });
            }
        }
        else
        {
            if (CheckIsFullItem() && brick.IsAddCompleted)
            {
                List<E_TypeBrick> L_TypeInGame = new();

                for (int i = 0; i < L_Stored.Count; i++)
                {
                    if (!L_TypeInGame.Contains(L_Stored[i]._typeBrick))
                    {
                        L_TypeInGame.Add(L_Stored[i]._typeBrick);
                    }
                }


                for (int i = 0; i < L_TypeInGame.Count; i++)
                {
                    int a = GetAmoutBrickInListItem(L_TypeInGame[i]);


                    if (a >= 3)
                    {
                        GameManager.instance.UiController.UiGamePlay.ListItem.CheckCanUseItem();
                        return;
                    }
                }
                GameManager.instance.UiController.ProcessWinLose(E_Result.Lose);
            }
        }
        StartCheck();
    }

    private void StartCheck()
    {
        StartCoroutine(IE_DelayCheckLayer());
    }

    IEnumerator IE_DelayCheckLayer()
    {
        yield return null;
        GridManager.instance.CheckLayerGrid();
    }

    public int GetPosMoveTo(E_TypeBrick e_TypeBrick)
    {
        if (L_Stored == null) return -1;

        if (L_Stored.Count == 8) return -1;


        for (int i = L_Stored.Count - 1; i >= 0; i--)
        {
            if (L_Stored[i]._typeBrick == e_TypeBrick)
            {
                return i + 1;
            }
        }
        return -1;
    }

    public void SortAndMoveElement(BrickBase brick)
    {
        int k = GetPosMoveTo(brick._typeBrick);
        if (k > L_Items.Count) return;
        brick.MoveToTarget(L_Items[k].transform);

        for (int i = k; i < L_Stored.Count; i++)
        {
            /*if (i < L_Stored.CountRemaining)*/
            L_Stored[i].transform.position = L_Items[i + 1].transform.position;
        }

        BrickBase[] Newlist = new BrickBase[L_Stored.Count + 1];

        int vitrichen = k;
        for (int i = 0; i < vitrichen; i++)
        {
            Newlist[i] = L_Stored[i];
        }
        Newlist[k] = brick;
        for (int j = vitrichen + 1; j < Newlist.Length; j++)
        {
            Newlist[j] = L_Stored[j - 1];
        }
        L_Stored = new List<BrickBase>(Newlist);


        /*L_Stored.Insert(k, brick);*/
    }

    public void SortElement()
    {
        if (L_Stored.Count > L_Items.Count) return;

        for (int i = 0; i < L_Stored.Count; i++)
        {
            L_Stored[i].transform.position = L_Items[i].transform.position;
        }
    }

    public void SortElement(List<BrickBase> List)
    {
        if (List.Count > L_Items.Count) return;

        for (int i = 0; i < List.Count; i++)
        {
            List[i].transform.position = L_Items[i].transform.position;
        }
    }

    public void ProcessItemMerge(BrickBase brick, E_TypeBrick Type)
    {
        bool isHaveBrickSameType = false;

        if (L_Stored.Count <= 0)
        {
            brick.MoveToTarget(L_Items[0].transform,()=>
            {
                brick._SpriteRenderer.sortingOrder = 1;
                brick._SpriteRendererTile.sortingOrder = 1;
            });
            AddDic(Type, brick);
            return;
        }

        for (int i = L_Stored.Count - 1; i >= 0; i--)
        {
            if (L_Stored[i]._typeBrick == Type)
            {
                AddDic(Type, brick);
                SortAndMoveElement(brick);
                isHaveBrickSameType = true;
                return;
            }
        }

        if (!isHaveBrickSameType)
        {
            AddDic(Type, brick);
            brick.MoveToTarget(L_Items[L_Stored.Count - 1].transform,()=>
            {
                brick._SpriteRenderer.sortingOrder = 1;
                brick._SpriteRendererTile.sortingOrder = 1;
            });
        }
    }



    public int GetAmoutBrickInListItem(E_TypeBrick Type)
    {
        int Count = 0;
        for (int i = 0; i < L_Stored.Count; i++)
        {
            if (Type == L_Stored[i]._typeBrick)
            {
                Count++;
            }
        }
        return Count;
    }

    public int GetAmoutRemainingItem()
    {
        return L_Items.Count - L_Stored.Count;
    }

    public bool CheckIsFullItem()
    {
        if (L_Stored.Count >= MaxCapacity)
        {
            return true;
        }
        return false;
    }

    public void ProcessElementTileReturn(BrickBase brick, int pos, Vector3 position)
    {
        brick.transform.DOMove(position, 0.4f).SetEase(Ease.Linear).OnStepComplete(() =>
        {
            if (L_Stored.Count >= 0)
            {
                L_Stored.Remove(L_Stored[pos]);
            }

            if (Dic_TypeCheck.ContainsKey(brick._typeBrick))
            {
                Dic_TypeCheck[brick._typeBrick].Remove(brick);

                if (Dic_TypeCheck[brick._typeBrick].Count == 0)
                {
                    Dic_TypeCheck.Remove(brick._typeBrick);
                }
            }

            brick.IsMoved = false;
            GameManager.instance.isInGame = true;
            GameManager.instance.UiController.UiGamePlay.ListItem.CheckCanUseItem();
        }).SetAutoKill();
    }

    public void Add1Slot()
    {
        L_Items.Add(AddSlot_img);
        MaxCapacity += 1;
        AddSlot_img.gameObject.SetActive(true);

        fill_tiles.sprite = SpriteFill_Tiles;
        transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,900);
        underFill_Tiles.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 875);
    }

    public void Move2BrickToStartPos()
    {
        /*        int Count = 0;

                for (int i = L_Stored.Count - 1; i >= 0; i--)
                {
                    Count++;

                    if (Dic_TypeCheck.ContainsKey(L_Stored[i]._typeBrick))
                    {
                        Dic_TypeCheck[L_Stored[i]._typeBrick].Remove(L_Stored[i]);


                        if (Dic_TypeCheck[L_Stored[i]._typeBrick].Count == 0)
                        {
                            Dic_TypeCheck.Remove(L_Stored[i]._typeBrick);
                        }
                    }

                    *//*L_Stored[i].transform.SetParent(L_Stored[i].ParentBrick.transform);*//*


                    Tweener Move = L_Stored[i].transform.DOLocalMove(L_Stored[i].startPos2, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        L_Stored[i].Rotation?.Kill();
                        L_Stored[i].IsMoved = false;
                       *//* L_Stored[i].transform.localRotation = L_Stored[i].RotationBase;*//*


                        L_Stored.Remove(L_Stored[i]);
                    });



                    if (Count == 2)
                    {
                        return;
                    }
                }*/
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
