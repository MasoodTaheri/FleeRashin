using System.Collections;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    GroundTile[] groundTiles = new GroundTile[0];
    [SerializeField]
    Transform tilesParent;
    [SerializeField]
    Vector2 groundDimension;

    public void CreatGround()
    {
        GameObject go;
        SpriteRenderer sr;
        Vector3 position = Vector3.zero;
        Vector2 bound = Vector2.zero;
        float chance = 0;
        int tileIndex = 0;


        for (int x = 0; x < groundDimension.x; x++)
        {
            for (int y = 0; y < groundDimension.y; y++)
            {
                tileIndex = -1;
                go = new GameObject();
                go.name = "Tile ("+x.ToString()+","+y.ToString()+")";
                go.transform.SetParent(tilesParent, false);
                sr = go.AddComponent<SpriteRenderer>();

                chance = UnityEngine.Random.Range(0, 1f);
                do
                {
                    chance -= groundTiles[++tileIndex].chance;
                } while (chance > 0);
                sr.sprite = groundTiles[tileIndex].tileSprite;
                
                if (x == 0 && y == 0)
                    bound = new Vector2(sr.sprite.texture.width / sr.sprite.pixelsPerUnit, sr.sprite.texture.height / sr.sprite.pixelsPerUnit);

                sr.sortingOrder = -4100;
                go.transform.localPosition = new Vector3((x + 0.5f - groundDimension.x / 2) * bound.x, (y + 0.5f - groundDimension.y / 2) * bound.y, 0);
            }
        }
    }
}

[Serializable]
class GroundTile
{
    public Sprite tileSprite;
    [Range(0, 1f)]
    public float chance;
}
