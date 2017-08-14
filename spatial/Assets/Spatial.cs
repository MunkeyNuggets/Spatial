using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle
{
    public Vector2 m_pos;
    public float m_radius;
}

public class Grid
{
    public List<Circle> circles = new List<Circle>();
}

public class Spatial : MonoBehaviour {
    Texture2D tex;
    List<Circle> circles = new List<Circle>();
    int count = 100;
    int gridDevision;
    int numGridSquares;

	void Start () {
        numGridSquares = 16;
        gridDevision = 1024 / numGridSquares;
        Grid[,] gridArray = new Grid[numGridSquares, numGridSquares];
        Random.InitState(0);
        tex = GetComponent<SpriteRenderer>().sprite.texture;
        for (int x = 0; x < numGridSquares; x++)
        {
            for (int y = 0; y < numGridSquares; y++)
            {
                gridArray[x, y] = new Grid();
            }
        }
        for(int i=0;i < count;++i)
        {
            Circle c = new Circle();
            c.m_pos = new Vector2(Random.Range(0, 1023), Random.Range(0, 1023));
            c.m_radius = Random.Range(1, 49);
            int startX = (int)(c.m_pos.x - c.m_radius) / gridDevision;
            int startY = (int)(c.m_pos.y - c.m_radius) / gridDevision;
            int endX = (int)(c.m_pos.x + c.m_radius) / gridDevision;
            int endY = (int)(c.m_pos.y + c.m_radius) / gridDevision;
            {
                if (startX < 0)
                    startX = 0;
                if (startY < 0)
                    startY = 0;

                if (endX > numGridSquares - 1)
                    endX = numGridSquares - 1;
                if (endY > numGridSquares - 1)
                    endY = numGridSquares - 1;
            }
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    gridArray[x, y].circles.Add(c);
                }
            }
            circles.Add(c);
        }
        Color32[] colours = new Color32[1024*1024];
        float t = Time.realtimeSinceStartup;
        //loops through the x and y
        for (int y = 0; y < 1024; ++y)
        {
            for (int x = 0; x < 1024; ++x)
            {
                float value = 0;
                int x_ = (int)x / 64;
                int y_ = (int)y / 64;
                for (int i = 0; i < gridArray[x_,y_].circles.Count; ++i)
                {
                    float d = (new Vector2((float)x, (float)y) - gridArray[x_, y_].circles[i].m_pos).magnitude;
                    if (d < gridArray[x_, y_].circles[i].m_radius)
                    {
                        value += (1.0f - d / gridArray[x_, y_].circles[i].m_radius);
                       value =  Mathf.Clamp(value, 0.0f, 1.0f);
                    }

                }
                colours[x + y * 1024].r = (byte)(value * 255);
                colours[x + y * 1024].g = (byte)(value * 255);
                colours[x + y * 1024].b = (byte)(value * 255);
                colours[x + y * 1024].a = 255;
            }
        }
        Debug.Log("Time taken = " + (Time.realtimeSinceStartup - t));
        tex.SetPixels32(colours);
        tex.Apply();
    }

    void Update () {
    }
}
