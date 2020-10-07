using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Dungeon
{
    public Sub_Dungeon left, right;
    public Rect rect;
    public Rect roomRect;

    public int debugID;
    private static int debugCounter = 0;

    public Sub_Dungeon(Rect rectangle)
    {
        rect = rectangle;
        debugID = debugCounter;
        debugCounter++;
    }

    public bool IsLeaf()
    {
        return (left == null && right == null);
    }

    public void CreateRoom()
    {
        if(left != null)
        {
            left.CreateRoom();
        }

        if(right != null)
        {
            right.CreateRoom();
        }

        if(IsLeaf())
        {
            int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
            int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);

            int roomX = (int)Random.Range(1, rect.width - roomWidth);
            int roomY = (int)Random.Range(1, rect.height - roomHeight);

            roomRect = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
        }
    }

    public bool Split(int minRoomSize, int maxRoomSize)
    {
        if (!IsLeaf())
        {
            return false;
        }

        bool splitH;
        if (rect.width / rect.height >= 1.25)
        {
            splitH = false;
        }
        else if (rect.height / rect.width >= 1.25)
        {
            splitH = true;
        }
        else
        {
            splitH = Random.Range(0.0f, 1.0f) > 0.5;
        }

        if (Mathf.Min(rect.height, rect.width) / 2 < minRoomSize)
        {
            //Debug.Log("Sub-dungeon " + debugID + " will be a leaf");
            return false;
        }

        if (splitH)
        {
            int split = Random.Range(minRoomSize, (int)(rect.width - minRoomSize));

            left = new Sub_Dungeon(new Rect(rect.x, rect.y, rect.width, split));
            right = new Sub_Dungeon(
              new Rect(rect.x, rect.y + split, rect.width, rect.height - split));
        }
        else
        {
            int split = Random.Range(minRoomSize, (int)(rect.height - minRoomSize));

            left = new Sub_Dungeon(new Rect(rect.x, rect.y, split, rect.height));
            right = new Sub_Dungeon(
              new Rect(rect.x + split, rect.y, rect.width - split, rect.height));
        }

        return true;
    }


}
