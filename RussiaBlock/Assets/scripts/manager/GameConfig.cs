using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{

    private static int _width = 500;       //游戏屏幕宽度
    private static int _length = 1000;   //游戏屏幕长度
    private static int _blocklength = 50;//小方块边长
    private static int _widthBlockNum = _width / _blocklength;
    private static int _lengthBlockNum = _length / _blocklength;
    private static Vector2 _startPos = new Vector2(-250, -500);//小方块起始位置
    private static Vector2 _spawnPos = new Vector2(0, 500);//方块生成位置 

    private static Color[][] _colors = new Color[][]{
        new Color[]{new Color (19/255f,101/255f,98/255f),new Color(34/255f,195/255f,195/255f)},
        new Color[]{new Color (129/255f,29/255f,65/255f),new Color(240/255f,42/255f,104/255f)},
        new Color[]{new Color (26/255f,74/255f,114/255f),new Color(52/255f,130/255f,236/255f)},
        new Color[]{new Color (55/255f,144/255f,26/255f),new Color(74/255f,236/255f,52/255f)},
        new Color[]{new Color (111/255f,105/255f,22/255f),new Color(204/255f,234/255f,24/255f)}
    };
    public static Color[][] Colors
    {
        get{ return _colors;}
    }
    public static int width
    {
        get { return _width; }
    }
    public static int length
    {
        get { return _length; }
    }
    public static int blocklength
    {
        get { return _blocklength; }
    }
    public static int widthBlockNum
    {
        get { return _widthBlockNum; }
    }
    public static int lengthBlockNum
    {
        get { return _lengthBlockNum; }
    }
    public static Vector2 startPos
    {
        get { return _startPos; }
    }
    public static Vector2 spawnPos
    {
        get { return _spawnPos; }
    }

}
