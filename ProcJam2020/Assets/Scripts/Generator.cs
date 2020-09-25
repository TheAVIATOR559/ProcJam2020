using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] GameObject cube;

    public int mapWidth, mapHeight;
    private int[,] perlinMap;
    public int seed;
    public float perlinDivider;

    private void Start()
    {
        perlinMap = new int[mapWidth, mapHeight];

        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                perlinMap[i, j] = (int)(Mathf.PerlinNoise((i + seed) / perlinDivider, (j + seed) / perlinDivider) * 10f);
                //Debug.Log(perlinMap[i, j]);
            }
        }

        //randomly set parts of the map to empty spaces
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if(Random.Range(0, 101) < 40)
                {
                    perlinMap[i, j] = -1;
                }
            }
        }

        //set the height of each section to the average height around it
        //for (int i = 0; i < mapWidth; i++)
        //{
        //    for (int j = 0; j < mapHeight; j++)
        //    {

        //    }
        //}

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if(perlinMap[i,j] != -1)
                {
                    Instantiate(cube, new Vector3(i, perlinMap[i, j], j), Quaternion.identity);
                }
            }
        }
    }
}
