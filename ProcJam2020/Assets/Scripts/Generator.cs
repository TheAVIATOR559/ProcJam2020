using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] GameObject tile;

    public int mapWidth, mapHeight;
    public int minRoomSize = 6, maxRoomSize = 16; //TODO SHIFT THIS FROM ROOM SIZE TO NUMBER OF SPLITS
    public int desiredRoomCount = 10;

    public bool drawDebugLines;

    Sub_Dungeon root;

    [SerializeField] private int seed;

    private void Awake()
    {
        GenerateDungeon();
    }

    private void ClearDungeon()
    {
        foreach(Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        root = null;
    }

    public void GenerateDungeon()
    {
        ClearDungeon();

        /*desiredRoomCount -> number of splits
         * 
         * desiredRoomCount MUST be even
         * round down to nearest even number
         * 
         * number of splits = desiredRoomCount - 1
         * 
         */

        root = new Sub_Dungeon(new Rect(0, 0, mapWidth, mapHeight));

        CreateBSP(root);
        root.CreateRoom();
        DrawRooms(root);
    }

    private void CreateBSP(Sub_Dungeon sd)
    {
        if(sd.IsLeaf())
        {
            if(sd.rect.width > maxRoomSize || sd.rect.height > maxRoomSize)
            {
                if(sd.Split(minRoomSize, maxRoomSize))
                {
                    CreateBSP(sd.left);
                    CreateBSP(sd.right);
                }
            }
        }
    }

    public void DrawRooms(Sub_Dungeon sd)
    {
        if(sd == null)
        {
            return;
        }

        if(sd.IsLeaf())
        {
            float roomHeight = 0;

            for (int i = (int)sd.roomRect.x; i < sd.roomRect.xMax; i++)
            {
                for (int j = (int)sd.roomRect.y; j < sd.roomRect.yMax; j++)
                {
                    roomHeight += Mathf.PerlinNoise((float)(i + seed) / 10f, (float)(j + seed) / 10f) * 10f;
                    //Debug.Log(Mathf.PerlinNoise((float)(i + seed) / 10f, (float)(j + seed) / 10f) * 10f);
                }
            }
            roomHeight = (int)(roomHeight / (sd.roomRect.height * sd.roomRect.width)) - 5;
            //Debug.Log("room height :: " + roomHeight);

            for (int i = (int)sd.roomRect.x; i < sd.roomRect.xMax; i++)
            {
                for(int j = (int)sd.roomRect.y; j < sd.roomRect.yMax; j++)
                {
                    GameObject newTile = Instantiate(tile, new Vector3(i, roomHeight, j), Quaternion.identity);
                    newTile.transform.SetParent(transform);
                }
            }
        }
        else
        {
            DrawRooms(sd.left);
            DrawRooms(sd.right);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(drawDebugLines)
        {
            DrawDebugBSP(root);
        }
    }

    private void DrawDebugBSP(Sub_Dungeon sd)
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(new Vector3(sd.rect.x, 0, sd.rect.y), new Vector3(sd.rect.xMax, 0, sd.rect.y));
        Gizmos.DrawLine(new Vector3(sd.rect.xMax, 0, sd.rect.y), new Vector3(sd.rect.xMax, 0, sd.rect.yMax));
        Gizmos.DrawLine(new Vector3(sd.rect.x, 0, sd.rect.yMax), new Vector3(sd.rect.xMax, 0, sd.rect.yMax));
        Gizmos.DrawLine(new Vector3(sd.rect.x, 0, sd.rect.y), new Vector3(sd.rect.x, 0, sd.rect.yMax));

        if(sd.left != null)
        {
            DrawDebugBSP(sd.left);
        }

        if(sd.right != null)
        {
            DrawDebugBSP(sd.right);
        }

        DrawDebugRoom(sd);
    }

    private void DrawDebugRoom(Sub_Dungeon sd)
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector3(sd.roomRect.x, 0, sd.roomRect.y), new Vector3(sd.roomRect.xMax, 0, sd.roomRect.y));
        Gizmos.DrawLine(new Vector3(sd.roomRect.xMax, 0, sd.roomRect.y), new Vector3(sd.roomRect.xMax, 0, sd.roomRect.yMax));
        Gizmos.DrawLine(new Vector3(sd.roomRect.x, 0, sd.roomRect.yMax), new Vector3(sd.roomRect.xMax, 0, sd.roomRect.yMax));
        Gizmos.DrawLine(new Vector3(sd.roomRect.x, 0, sd.roomRect.y), new Vector3(sd.roomRect.x, 0, sd.roomRect.yMax));
    }
}
