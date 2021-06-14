using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;


public class ItemDatabase : MonoBehaviour {
    private List<Item> database = new List<Item>();
    private JsonData itemData;

    void Awake(){

        #if UNITY_STANDALONE
                itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json", System.Text.Encoding.Default));
                ConstructItemDatabase();
        #elif UNITY_ANDROID
                TextAsset file = Resources.Load("Items") as TextAsset;
                string jsonString = file.ToString ();
                itemData = JsonMapper.ToObject(jsonString);
                ConstructItemDatabase();
        #endif
    }

    void Start() {
        
    }

    public Item FetchItemByID(int id) {
        for (int i = 0; i<database.Count; i++)
            if (database[i].ID == id)
                return database[i];
        return null;
    }

    void ConstructItemDatabase() {
        for (int i = 0; i<itemData.Count; i++) {
            database.Add(new Item((int)itemData[i]["id"],itemData[i]["title"].ToString(),
            itemData[i]["slug"].ToString(),(int)itemData[i]["spaces"], itemData[i]["info"].ToString()));
        }
    }

}

public class Item {
    public int ID { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public int Spaces { get; set; }
    public string Info { get; set; }
    public Sprite Sprite { get; set; }

    public Item(int id, string title, string slug,int spaces, string info) {
        this.ID = id;
        this.Title = title;
        this.Slug = slug;
        this.Spaces = spaces;
        this.Info = info;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }

    public Item() {
        this.ID = -1;
    }
}