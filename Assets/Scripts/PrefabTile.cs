using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class PrefabTile : UnityEngine.Tilemaps.TileBase
{
    public Sprite Sprite; //The sprite of tile in the palette
    public GameObject Prefab; //The gameobject to spawn

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // Assign variables
        if (!Application.isPlaying) tileData.sprite = Sprite;
        else tileData.sprite = null;

        if (Prefab) tileData.gameObject = Prefab;
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        return true;
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
    {
        tileAnimationData.animatedSprites = new Sprite[] { null };
        tileAnimationData.animationSpeed = 0;
        tileAnimationData.animationStartTime = 0;
        return true;
    }

}