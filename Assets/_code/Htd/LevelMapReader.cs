using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelMapReader : MonoBehaviour
{
    #region Properties

    [SerializeField]
    Texture2D levelTexture;
    [SerializeField]
    Vector2 levelSize;
    [SerializeField]
    Transform objectParent;
    [SerializeField]
    SpriteRenderer objectPrefab;
    [SerializeField]
    Color objectRepresentativeColor;
  //[SerializeField]
    Sprite objectSprite;
    [SerializeField]
    float objectScale;
    [SerializeField]
    string objectTag;

    #endregion

    #region OldVersion

    List<Rect> UsedSpace = new List<Rect>();

    /*public*/ int x;
    /*public*/ int y;
    /*public*/ bool clear;

    public void PlaceObject()
    {
        if (clear)
        {
            clear = false;
            x = y = 0;
            UsedSpace.Clear();
        }

        Color[] checkBox;

        for (; y < levelTexture.height - (int)(objectSprite.texture.height * objectScale); y++)
        {
            for (; x < levelTexture.width - (int)(objectSprite.texture.width * objectScale); x++)
            {
                if (IsInUsedSpace(ref x,y, (int)(objectSprite.texture.width * objectScale), (int)(objectSprite.texture.height * objectScale)))
                {
                    continue;
                }

                checkBox = levelTexture.GetPixels(x, y, (int)(objectSprite.texture.width * objectScale), (int)(objectSprite.texture.height * objectScale));

                if (IsCheckBoxPassed(checkBox,objectRepresentativeColor))
                {
                    InstantiateAnObject(x,y);
                    UsedSpace.Add(new Rect(x, y, (int)(objectSprite.texture.width * objectScale), (int)(objectSprite.texture.height * objectScale)));
                    x += (int)(objectSprite.texture.width * objectScale);
                    
                    return;                
                }
            }

            x = 0;
            if (y % 20 == 0)
            {
                y++;
                return;
            }
        }
    }

    bool IsCheckBoxPassed(Color[] sample, Color color)
    {
        bool result = true;

        for (int i = 0; i < sample.Length; i++)
        {
            result &= (sample[i] == color);
            if (!result)
                return result;
        }

        return result;
    }

    void InstantiateAnObject(int x, int y)
    {
        GameObject go = (GameObject)Instantiate(objectPrefab.gameObject, transform);
        go.GetComponent<SpriteRenderer>().sprite = objectSprite;
        go.tag = objectTag;
        float xPos = (((x + objectSprite.texture.width / 2f * objectScale) / levelTexture.width)) * levelSize.x * 2 - levelSize.x;
        float yPos = (((y + objectSprite.texture.height / 2f * objectScale) / levelTexture.height)) * levelSize.y * 2 - levelSize.y;
        go.transform.localPosition = new Vector3(xPos,yPos,0);
    }

    bool IsInUsedSpace(ref int x, int y,int width, int height)
    {
        for (int i = 0; i < UsedSpace.Count; i++)
        {
            if (UsedSpace[i].Contains(new Vector2(x, y)))
            {
                x = (int)UsedSpace[i].x + (int)UsedSpace[i].width;
                return true;
            }
            if (UsedSpace[i].Contains(new Vector2(x + width, y)))
            {
                //x = (int)usedSpace[i].x + (int)usedSpace[i].width;
                return true;
            }
            if (UsedSpace[i].Contains(new Vector2(x, y+height)))
            {
                //x = (int)usedSpace[i].x + (int)usedSpace[i].width;
                return true;
            }
            if (UsedSpace[i].Contains(new Vector2(x+width, y+height)))
            {
                //x = (int)usedSpace[i].x + (int)usedSpace[i].width;
                return true;
            }
        }

        return false;
    }

    #endregion

    [SerializeField]
    LevelMapObject[] objects = new LevelMapObject[0];
    List<ObjectInfo> plantedObjects;
    public Vector2Int emptyPixlePos;
    public bool reset;

    public void Test() // need a lot of attention
    {
        if (reset)
        {
            plantedObjects = new List<ObjectInfo>();
            emptyPixlePos = Vector2Int.zero;
            reset = false;
        }

        Vector2Int pointPos = Vector2Int.zero;
        LevelMapObject nextObject = objects[UnityEngine.Random.Range(0, objects.Length)];

        if (FindAnEmptyPixle(ref pointPos))
        {
            List<ObjectInfo> currentClusterlist = new List<ObjectInfo>();

            do
            {
                PlaceAnObject(pointPos, nextObject); 
                currentClusterlist.Add(plantedObjects[plantedObjects.Count - 1]);
                //emptyPixlePos.x = Mathf.Max(emptyPixlePos.x, currentClusterlist[currentClusterlist.Count - 1].pos.x);
                nextObject = objects[UnityEngine.Random.Range(0, objects.Length)];

                while (currentClusterlist.Count > 0 && !FindNextObjectPos(currentClusterlist[currentClusterlist.Count - 1], nextObject, ref pointPos))
                {
                    currentClusterlist.RemoveAt(currentClusterlist.Count - 1);
                }

            } while (currentClusterlist.Count > 0);
        }

    }

    public bool FindAnEmptyPixle(ref Vector2Int pos)
    {
        Color pixleColor;
        print(emptyPixlePos);
        for (; emptyPixlePos.y < levelTexture.height; emptyPixlePos.y++)
        {
            for (; emptyPixlePos.x < levelTexture.width; emptyPixlePos.x++)
            {
                /*if (IsInOtherObjectRange(emptyPixleX, emptyPixleY))
                    continue;*/

                pixleColor = levelTexture.GetPixel(emptyPixlePos.x, emptyPixlePos.y);

                if (pixleColor == objectRepresentativeColor && !IsInOtherObjectRange(emptyPixlePos))
                {
                    emptyPixlePos.x++;
                    pos.x = emptyPixlePos.x;
                    pos.y = emptyPixlePos.y;
                    return true;
                }
                
            }

            emptyPixlePos.x = 0;
        }

        return false;
    }

    void PlaceAnObject(Vector2Int pos, LevelMapObject lMObject )
    {
        GameObject go = (GameObject)Instantiate(objectPrefab.gameObject, objectParent);
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = lMObject.objectSprite;
        sr.sortingOrder = -pos.y;
        go.tag = objectTag;
        float xPos = ((pos.x / (float)levelTexture.width)) * levelSize.x * 2 - levelSize.x;
        float yPos = ((pos.y / (float)levelTexture.height)) * levelSize.y * 2 - levelSize.y;
        go.transform.localPosition = new Vector3(xPos, yPos, 0);
        plantedObjects.Add(new ObjectInfo(pos, lMObject.range));
    }

    bool FindNextObjectPos(ObjectInfo oI,LevelMapObject nextObject, ref Vector2Int result)
    {
        List<Vector2Int> pointList = new List<Vector2Int>();
        int range = (int) (Mathf.Max(oI.range, nextObject.range) * objectScale);

        for (int i = 0; i < 360; i++)
        {
            Vector2Int temp = new Vector2Int(Mathf.RoundToInt(oI.pos.x + (range *1.1f) * Mathf.Cos(Mathf.Deg2Rad * i)),
                Mathf.RoundToInt(oI.pos.y + (range *1.1f) * Mathf.Sin(Mathf.Deg2Rad * i)));
            
            if (new Rect(0,0,levelTexture.width,levelTexture.height).Contains(temp) &&/* temp.x >= 0 && temp.y >= 0 &&*/ !pointList.Contains(temp) && levelTexture.GetPixel(temp.x, temp.y) == objectRepresentativeColor && !IsInOtherObjectRange(temp))
            {
                pointList.Add(temp);
            }
        }
       
        int pointIndex = UnityEngine.Random.Range(0, pointList.Count);
        if (pointList.Count > 0)
        {
            result = pointList[pointIndex];
            return true;
        }

        return false;
    }

    bool IsInOtherObjectRange(Vector2Int pos)
    {
        foreach (ObjectInfo item in plantedObjects)
        {
            if (Vector2.Distance(new Vector2(pos.x,pos.y), new Vector2(item.pos.x,item.pos.y)) < item.range * objectScale)
            {
                return true;
            }
        }
        return false;
    }
}

[Serializable]
class LevelMapObject
{
    public Sprite objectSprite;
    public float range;
}

class ObjectInfo
{
    public Vector2Int pos;
    public float range;
    public ObjectInfo(int _x, int _y, float _range)
    {
        pos = new Vector2Int(_x, _y);
        range = _range;
    }
    public ObjectInfo(Vector2Int _pos, float _range)
    {
        pos = _pos;
        range = _range;
    }
}
