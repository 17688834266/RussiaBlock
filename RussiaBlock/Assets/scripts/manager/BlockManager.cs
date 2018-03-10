using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance;
    private List<GameObject> currentCombo;//当前组合
    private GameObject block;
    private float angle;
    private Transform blockParent;
    private int moveStep;
    // Use this for initialization
    private float[] angles;//随机旋转角度
    public void Init()
    {
        angle=90;
        angles=new  float[]{0,90,180,270};
        moveStep = GameConfig.blocklength;
        blockParent = GameObject.FindGameObjectWithTag("spawnRoot").transform;
        block = Resources.Load<GameObject>("block");
        QPoolSingleton.Instance.AddSpawnPool(PoolName.blockPoolName, GameConfig.widthBlockNum * GameConfig.lengthBlockNum, block);
        SpawnBlockCombo();
    }

    //生成方块组合，//默认第一个为组合的中心点
    public List<GameObject> SpawnBlockCombo()
    {
        blockType ty = (blockType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(blockType)).Length);
        switch (ty)
        {
            case blockType.Z:
                currentCombo = SpawnZ();
                break;
            case blockType.I:
                currentCombo = SpawnI();
                break;
            case blockType.T:
                currentCombo = SpawnT();
                break;
            case blockType.L:
                currentCombo = SpawnL();
                break;
            case blockType.O:
                currentCombo = SpawnO();
                break;
        }
        // float angle=UnityEngine.Random.Range(0,angles.Length);
        // RotateBlock(currentCombo[0].transform,currentCombo,angle);
        int isOverTurn=UnityEngine.Random.Range(0,100);
        if(isOverTurn>50)
         OverTure(currentCombo[0].transform,currentCombo);
        Color[] colors=GameConfig.Colors[UnityEngine.Random.Range(0,GameConfig.Colors.Length)];
        foreach(GameObject obj in currentCombo)
        {
            obj.GetComponent<Image>().color=colors[0];
            obj.transform.GetChild(0).GetComponent<Image>().color=colors[1];
        }
        return currentCombo;
    }
    private List<GameObject> SpawnZ()
    {
        List<GameObject> lis = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = QPoolSingleton.Instance.SpawnGo(PoolName.blockPoolName);
            obj.transform.SetParent(blockParent);
            obj.transform.localScale = Vector3.one;
            lis.Add(obj);
        }
        lis[0].transform.localPosition = GameConfig.spawnPos;
        lis[1].transform.localPosition = GetPosByMove(MoveType.right, lis[0].transform.localPosition);
        lis[2].transform.localPosition = GetPosByMove(MoveType.up, lis[0].transform.localPosition);
        lis[3].transform.localPosition = GetPosByMove(MoveType.left, lis[2].transform.localPosition);
        return lis;
    }
    private List<GameObject> SpawnT()
    {
        List<GameObject> lis = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = QPoolSingleton.Instance.SpawnGo(PoolName.blockPoolName);
            obj.transform.SetParent(blockParent);
            obj.transform.localScale = Vector3.one;
            lis.Add(obj);
        }
        lis[0].transform.localPosition = GameConfig.spawnPos;
        lis[1].transform.localPosition = GetPosByMove(MoveType.down, lis[0].transform.localPosition);
        lis[2].transform.localPosition = GetPosByMove(MoveType.right, lis[0].transform.localPosition);
        lis[3].transform.localPosition = GetPosByMove(MoveType.left, lis[0].transform.localPosition);
        return lis;
    }
    private List<GameObject> SpawnI()
    {
        List<GameObject> lis = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = QPoolSingleton.Instance.SpawnGo(PoolName.blockPoolName);
            obj.transform.SetParent(blockParent);
            obj.transform.localScale = Vector3.one;
            lis.Add(obj);
        }
        lis[0].transform.localPosition = GameConfig.spawnPos;
        lis[1].transform.localPosition = GetPosByMove(MoveType.up, lis[0].transform.localPosition);
        lis[2].transform.localPosition = GetPosByMove(MoveType.down, lis[0].transform.localPosition);
        lis[3].transform.localPosition = GetPosByMove(MoveType.up, lis[1].transform.localPosition);
        return lis;
    }
    private List<GameObject> SpawnL()
    {
        List<GameObject> lis = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = QPoolSingleton.Instance.SpawnGo(PoolName.blockPoolName);
            obj.transform.SetParent(blockParent);
            obj.transform.localScale = Vector3.one;
            lis.Add(obj);
        }
        lis[0].transform.localPosition = GameConfig.spawnPos;
        lis[1].transform.localPosition = GetPosByMove(MoveType.up, lis[0].transform.localPosition);
        lis[2].transform.localPosition = GetPosByMove(MoveType.down, lis[0].transform.localPosition);
        lis[3].transform.localPosition = GetPosByMove(MoveType.left, lis[2].transform.localPosition);
        return lis;
    }
    private List<GameObject> SpawnO()
    {
        List<GameObject> lis = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = QPoolSingleton.Instance.SpawnGo(PoolName.blockPoolName);
            obj.transform.SetParent(blockParent);
            obj.transform.localScale = Vector3.one;
            lis.Add(obj);
        }
        lis[0].transform.localPosition = GameConfig.spawnPos;
        lis[1].transform.localPosition = GetPosByMove(MoveType.down, lis[0].transform.localPosition);
        lis[2].transform.localPosition = GetPosByMove(MoveType.right, lis[0].transform.localPosition);
        lis[3].transform.localPosition = GetPosByMove(MoveType.down, lis[2].transform.localPosition);
        return lis;
    }
    public List<GameObject> GetCurrentCombo()
    {
        return currentCombo;
    }

    public Vector2 GetPosByMove(MoveType tp, Vector2 startPos)
    {
        switch (tp)
        {
            case MoveType.up:
                return startPos + new Vector2(0, moveStep);
            case MoveType.down:
                return startPos + new Vector2(0, -moveStep);
            case MoveType.left:
                return startPos + new Vector2(-moveStep, 0);
            case MoveType.right:
                return startPos + new Vector2(moveStep, 0);
        }
        return startPos;
    }

    public void BlockMove(MoveType ty, List<GameObject> lis)
    {
        Vector2 pos = GetPosByMove(ty, Vector2.zero);

        foreach (GameObject obj in lis)
        {
            obj.transform.localPosition += new Vector3(pos.x, pos.y, 0);
        }
    }
    //toel
    private enum blockType
    {
        Z,
        I,
        T,
        L,
        O
    }
    public void RotateBlock(Transform root, List<GameObject> lis,float _angle=90)
    {
        foreach (GameObject obj in lis)
            obj.transform.RotateAround(root.position, Vector3.forward, _angle);
    }
    public List<Vector3> RotateBlockLocalPos(Transform root, List<GameObject> lis)
    {
        List<Vector3> pos = new List<Vector3>();
        Vector3 rootPos =root.localPosition;
        foreach (GameObject obj in lis)
        {
            Vector3 posStart = obj.transform.localPosition;
            float x = (posStart.x - rootPos.x) * Mathf.Cos(angle) - (posStart.y - rootPos.y) * Mathf.Sin(angle) + rootPos.x;
            float y = (posStart.x - rootPos.x) * Mathf.Sin(angle) - (posStart.y - rootPos.y) * Mathf.Cos(angle) + rootPos.y;
            pos.Add(new Vector3(x, y, 0));
        }
        return pos;
    }
    //翻转
    public void OverTure(Transform root,List<GameObject> lis)
    {
             Vector3 rootPos=root.localPosition;
             foreach(GameObject obj in lis)
             {
                 Vector3 pos=obj.transform.localPosition;
                 obj.transform.localPosition=new Vector3 (2*rootPos.x-pos.x,pos.y,0);
             }
    }
}
public enum MoveType
{
    up,
    down,
    left,
    right
}