using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveContoler : MonoBehaviour
{
    MoveType moveType;
    bool isMove;
    List<GameObject> currentBlock;
    private float playerTimer;//玩家移动计时器
    private float playerAmonTime;//移动时间间隔,或者叫移动速度
    private float autoAmonTime;//自动落体速度
    private float autoTimer;//自动移动计时
    private bool isTimer;//是否在计时

    // Use this for initialization
    void Start()
    {
        isMove = false;
        playerAmonTime = 0.2f;
        autoAmonTime = 1f;
        playerTimer = 0;
        autoTimer = 0;
        GameManager.Instacne.gameState = GameState.Gameing;
        currentBlock = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            BlockManager.Instance.SpawnBlockCombo();
        PlayerMove();
        AutoMove();
    }

    private void PlayerMove()
    {
        if (playerTimer > 0)
        {
            playerTimer -= Time.deltaTime;
            return;
        }
        if (BlockManager.Instance.GetCurrentCombo() == null)
            return;
        currentBlock = BlockManager.Instance.GetCurrentCombo();
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerTimer = playerAmonTime;
            if (GameManager.Instacne.IsValidPos(BlockManager.Instance.RotateBlockLocalPos(currentBlock[0].transform, currentBlock)))
                BlockManager.Instance.RotateBlock(currentBlock[0].transform, currentBlock);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            playerTimer = playerAmonTime;
            isMove = true;
            moveType = MoveType.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            playerTimer = playerAmonTime;
            isMove = true;
            moveType = MoveType.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            playerTimer = playerAmonTime;
            isMove = true;
            moveType = MoveType.right;
        }
        else if (Input.GetKey(KeyCode.S))
        {

            playerTimer = playerAmonTime;
            isMove = true;
            moveType = MoveType.down;
        }
        else if (Input.GetKey(KeyCode.A))
        {

            playerTimer = playerAmonTime;
            isMove = true;
            moveType = MoveType.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {

            playerTimer = playerAmonTime;
            isMove = true;
            moveType = MoveType.right;
        }
        if (isMove)
        {
            if (GameManager.Instacne.IsValidPos(currentBlock, moveType))
                BlockManager.Instance.BlockMove(moveType, currentBlock);
            isMove = false;
        }
    }
    private void AutoMove()
    {
        if (currentBlock == null)
            return;
        autoTimer += Time.deltaTime;
        if (autoTimer > autoAmonTime)
        {
            autoTimer = 0;
            if (!GameManager.Instacne.IsValidPos(currentBlock, MoveType.down))
            {
                Debug.Log("fangkuai stop!");

                GameManager.OnBlockStopEvent(currentBlock);

            }
            else
            {
                BlockManager.Instance.BlockMove(MoveType.down, currentBlock);
            }
        }
    }
}
