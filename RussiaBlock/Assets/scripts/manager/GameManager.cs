using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class GameManager
{
    #region 单例
    private GameManager()
    {

    }
    public static Action<List<GameObject>> OnBlockStopEvent;//当方块落到底的时候
    private static GameManager _instace;
    public GameState gameState;
    public static GameManager Instacne
    {
        get
        {
            if (_instace == null)
                _instace = new GameManager();
            return _instace;
        }
    }
    #endregion;

    private Dictionary<Vector2, bool> blockRoot;//记录该位置是否存在方块
    private Dictionary<Vector2, GameObject> blockWithPos;//记录方块与位置的关系
    public void Init()
    {
        blockRoot = new Dictionary<Vector2, bool>();
        blockWithPos = new Dictionary<Vector2, GameObject>();
        OnBlockStopEvent += OnBlockStop;
    }

    //通过位置获取坐标
    public Vector2 GetCoordByPos(Vector2 pos)
    {
        Vector2 coord = new Vector2();
        coord.x = Mathf.RoundToInt((pos.x - GameConfig.startPos.x) / GameConfig.blocklength);
        coord.y = Mathf.RoundToInt((pos.y - GameConfig.startPos.y) / GameConfig.blocklength);
        return coord;
    }

    //通过坐标查看该位置是否有方块存在
    public bool IsExistInPos(Vector2 coord)
    {
        if (blockRoot.ContainsKey(coord) && blockRoot[coord])
        {
            Debug.Log("pos valid:" + coord);
            return true;
        }

        return false;
    }

    //通过坐标查看该位置是在范围内
    public bool IsInRange(Vector2 coord)
    {
        int a = Mathf.RoundToInt(coord.x);
        int yy = Mathf.RoundToInt(coord.y);
        if (a <= GameConfig.widthBlockNum && a >= 0 && yy >= 0)
        {
            return true;
        }
        Debug.Log("coord out range:" + coord);
        return false;
    }

    //通过坐标查看该位置是否是有效位置
    public bool IsValidPos(Vector2 coord)
    {
        // Debug.Log(coord+"!"+ coord.x + "             " + GameConfig.widthBlockNum);
        if (IsInRange(coord) && !IsExistInPos(coord))
        {
            return true;
        }
        // Debug.Log(coord + ":false");
        return false;
    }

    public bool IsValidPos(List<GameObject> lis, MoveType ty)
    {
        foreach (GameObject obj in lis)
        {
            Vector2 coord = GetCoordByPos(BlockManager.Instance.GetPosByMove(ty, obj.transform.localPosition));
            if (!IsValidPos(coord))
            {
                Debug.Log("valid:" + coord);
                return false;
            }
        }
        return true;
    }

    public bool IsValidPos(List<Vector3> lis)
    {
        foreach (Vector3 v3 in lis)
        {
            Vector2 coord = GetCoordByPos(v3);
            // Debug.Log(coord + "    " + v3);
            if (!IsValidPos(coord))
            {
                Debug.Log("旋转不了了");
                return false;
            }
        }
        return true;
    }

    //通过坐标，将该位置设置为已存在
    private void SetPosExist(Vector2 coord)
    {
        if (blockRoot.ContainsKey(coord))
            blockRoot[coord] = true;
        else
            blockRoot.Add(coord, true);
    }
    private void SetBlockExist(Vector2 coord, GameObject block)
    {
        if (blockWithPos.ContainsKey(coord))
            blockWithPos[coord] = block;
        else
            blockWithPos.Add(coord, block);
    }
    //设置方块占据某个位置
    public void SetPosExist(List<GameObject> lis)
    {
        foreach (GameObject obj in lis)
        {
            Vector2 cood = GetCoordByPos(obj.transform.localPosition);
            SetPosExist(cood);
            SetBlockExist(cood, obj);
        }
    }
    //消除一行
    public void Clear(int line)
    {
        Debug.Log("clear line:" + line);
        for (int i = 0; i <= GameConfig.widthBlockNum; i++)
        {
            Vector2 v2 = new Vector2(i, line);
            if (blockRoot.ContainsKey(v2))
                blockRoot.Remove(v2);
            if (blockWithPos.ContainsKey(v2))
            {
                QPoolSingleton.Instance.DisSpawnGo(PoolName.blockPoolName, blockWithPos[v2]);
                blockWithPos.Remove(v2);
            }
        }
    }
    //判断某一行是否已满
    public bool CheckIsFullInLien(int line)
    {
        for (int row = 0; row <= GameConfig.widthBlockNum; row++)
        {
            Vector2 pos = new Vector2(row, line);
            if (!blockWithPos.ContainsKey(pos) || blockWithPos[pos] == null)
            {
                return false;
            }
        }
        Clear(line);
        return true;
    }
    public bool CheckIsFullInLien(List<GameObject> lis)
    {
        bool isFull = false;
        List<int> fullLine = new List<int>();
        foreach (GameObject obj in lis)
        {
            int line = Mathf.RoundToInt(GetCoordByPos(obj.transform.localPosition).y);
            if (!fullLine.Contains(line))
            {
                if (CheckIsFullInLien(line))
                {
                    fullLine.Add(line);
                    isFull = true;
                }
            }
        }
        if(isFull)
        UpdatePosOnFull(fullLine);
        return isFull;
    }
    //当方块满的时候，跟新方块位置
    private void UpdatePosOnFull(List<int> lines)
    {
        lines.Sort();
        //跟新方块位置;
        foreach (var pair in blockWithPos)
        {
            if (pair.Key.y > lines[0])
            {
                int index = 1;
                for (int i = 1; i < lines.Count; i++)
                {
                    if (pair.Key.y < lines[i])
                    {
                        index = i;
                        break;
                    }
                    index=i+1;
                }
                Vector3 pos=  pair.Value.transform.localPosition;
                pair.Value.transform.localPosition=pos-index*Vector3.up*GameConfig.blocklength;
            }
        }
        InitDate(blockWithPos.Values.ToList());
    }
    //跟新数据
    private void InitDate(List<GameObject> lis)
    {
      blockRoot=new Dictionary<Vector2, bool>();
      blockWithPos=new Dictionary<Vector2, GameObject>();
      foreach(GameObject block in lis)
      {
          Vector2 coord=GetCoordByPos(block.transform.localPosition);
          blockRoot.Add(coord,true);
          blockWithPos.Add(coord,block);
      }
    }
    private void OnBlockStop(List<GameObject> lis)
    {
        SetPosExist(lis);
        if (CheckIsFullInLien(lis))
        {
            BlockManager.Instance.SpawnBlockCombo();
        }
        else if (IsGameOver(lis))
        {
            Debug.Log("Game Over");
            return;
        }
        else
        {

            BlockManager.Instance.SpawnBlockCombo();
        }
        // foreach (var pair in blockRoot)
        // {
        //     Debug.Log(pair.Key + "    " + pair.Value);
        // }
        // Debug.Log("-----------------------------");
    }
    //判断游戏是否结束
    private bool IsGameOver(List<GameObject> lis)
    {
        foreach (GameObject obj in lis)
        {
            Vector2 coord = GetCoordByPos(obj.transform.localPosition);

            if (Mathf.RoundToInt(coord.y) >= GameConfig.lengthBlockNum)
            {
                Debug.Log("game over:" + coord + "   " + GameConfig.lengthBlockNum);
                return true;
            }
        }
        return false;
    }
}
public enum GameState
{
    UnStarte,
    Gameing,
    Pause,

}

