using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDictionary
{

    public static Dictionary<string, GameItem> itemsDict = new()
    {
        { "cub_1", new GameItem("cub_1", "50_cube_color", "Collectable", "Art/Block/Block_Color", "Sprite/50_cube_color") },
        { "cub_2", new GameItem("cub_2", "50_cube_glow", "Collectable", "Art/Block/Block_Glow", "Sprite/50_cube_glow") },
        { "token", new GameItem("token_1", "token", "Token", "Art/Token/Token", "Sprite/token") },
        { "lucium_1", new GameItem("lucium_1", "lucium", "Collectable", "Art/Lucium/Lucium", "Sprite/lucium") },
        { "seed_1", new GameItem("seed_1", "fzj_seed", "Collectable", "Art/Seed/seed_FZJ", "Sprite/fzj_seed") },
        { "seed_2", new GameItem("seed_2", "qnh_seed", "Collectable", "Art/Seed/seed_QNH", "Sprite/qnh_seed") },
        { "seed_3", new GameItem("seed_3", "mg_seed", "Collectable", "Art/Seed/seed_MG", "Sprite/mg_seed") },
    };
}

public class GameItem
{
    public string itemId;
    public string itemName;
    public string tag;
    public GameObject prefab;
    public Sprite iconSprite;

    public GameItem(string itemId, string itemName, string tag, string prefabLocationPath, string spriteLocationPath)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.tag = tag;
        prefab = Resources.Load<GameObject>(prefabLocationPath);
        iconSprite = Resources.Load<Sprite>(spriteLocationPath);
    }



}

public enum RemoteToGameConnect
{
    cub_1=1,
    cub_2=2,
    token=3, 
    lucium_1=4, 
    seed_1=5, 
    seed_2=6, 
    seed_3=7
}
