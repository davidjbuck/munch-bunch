using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    [SerializeField] private Inventory inventoryToSave = null;
    string myFilePath, fileName;
    public GameObject player; 
    public GameObject playerH;
    //public GameObject inventoryList;
    public static bool loading;
    const char SPLIT_CHAR = '_';
    
    private static Dictionary<int, Item> allItemCodes = new Dictionary<int, Item>();
    private static int HashItem(Item item) => Animator.StringToHash(item.itemName);
    private static string FILE_PATH = "NULL!";

    // public GameObject inventoryL;
    //body for rotation which wasnt working
    //public GameObject pTurn; 
    // Start is called before the first frame update
    private void Awake()
    {
        FILE_PATH = Application.dataPath + "/saveInventory.txt";

        CreateItemDictionary();
    }
    void Start()
    {
        fileName = "save.txt";
        fileName = "saveInventory.txt";

        myFilePath = Application.dataPath + "/" + fileName;

    }

    // Update is called once per frame
    void Update()
    {
        if (loading)
        {
            // Load();
            Debug.Log("LOAD AGAIN");
            setPosition();
            LoadInventory();

            loading = false;


        }
    }
    private bool InventorySaveExists()
    {
        if (!File.Exists(FILE_PATH))
        {
            Debug.LogWarning("The file you're trying to access doesn't exist. (Try saving an inventory first).");
            return false;
        }
        return true;
    }

    private void CreateItemDictionary()
    {
        Item[] allItems = Resources.FindObjectsOfTypeAll<Item>();

        foreach (Item i in allItems)
        {
            int key = HashItem(i);
            Debug.Log(key);
            Debug.Log("LOCATION:" + i.transform.position);
            if (!allItemCodes.ContainsKey(key))
                allItemCodes.Add(key, i);
        }
    }


    //Delete all items in the inventory. Will be irreversable. Could just create a new file (ie. Change the name of the old save file and create a new one)
    public void ClearInventorySaveFile()
    {
   
        File.WriteAllText(Application.dataPath + "/saveInventory.txt", "");//Was previously using String.Empty for the "" empty string but this does not require system namespace
    }


    public void setPosition()
    {

        string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
        string[] playerLocationLoad = saveString.Split(',');
        player.transform.position = new Vector3(float.Parse(playerLocationLoad[1]), float.Parse(playerLocationLoad[2]), float.Parse(playerLocationLoad[3]));
        Debug.Log("SET PLAYER POSITION AGAIN");
    }
    public void Load()
    {
        loading = true;

        string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
        string[] playerLocationLoad = saveString.Split(',');
        SceneManager.LoadScene(int.Parse(playerLocationLoad[0]));
        setPosition();
        player.transform.position = new Vector3(float.Parse(playerLocationLoad[1]), float.Parse(playerLocationLoad[2]), float.Parse(playerLocationLoad[3]));
        Debug.Log("SET PLAYER POSITION");
        loading = false;
    }
    public void SaveGame()
    {
        float posX, posY, posZ, rotX, rotY, rotZ;/*
        GameObject playerC = GameObject.FindGameObjectWithTag("Player"); 
        Debug.Log(playerC.transform.position.x + " ");
        */
        Debug.Log("Save Game");
        string saveString = (SceneManager.GetActiveScene().buildIndex + "," + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z + "," + playerH.GetComponent<ThirdPersonController>().health + "\n");
        //tried to get rotation, but it wasnt working consistently
        // + "," + pTurn.transform.eulerAngles.y

        File.WriteAllText(Application.dataPath + "/save.txt", saveString);
        SaveInventory();

    }

    //INVENTORY SAVE:
    //FINDS ALL ITEMS IN GAME
    //SETS A SPECIFIC KEY TO EACH ONE
    //SAVES KEY INT
    //SAVES COUNT INT
    //LOADS KEY AND COUNT
    //USES KEY TO ADD ITEM TO INVENTORY
    //USES COUNT TO SET AMOUNT
    //works better for ingredients, rather than specific foods with custom stats
    public void SaveInventory()
    {
        using (StreamWriter sw = new StreamWriter(FILE_PATH))
        {
            sw.Write("");
                Dictionary<Item, int> inventory = new Dictionary<Item, int>();
                for (int i = 0; i < player.GetComponent<Inventory>().inventoryList.Count; i++)
                {
                    inventory.Add(player.GetComponent<Inventory>().inventoryList[i], player.GetComponent<Inventory>().inventoryList[i].amount);
                }
                foreach (KeyValuePair<Item, int> kvp in inventory)
                { 
                    Item item = kvp.Key;
                    int count = kvp.Value;

                    string itemID = HashItem(item).ToString();

                    sw.WriteLine(itemID + SPLIT_CHAR + count);
                }
                
            } 
    }
    public void LoadInventory()
    {
        loading = false;

        Dictionary<Item, int> inventory = new Dictionary<Item, int>();

        if (InventorySaveExists())
        {


            string line = "";

            using (StreamReader sr = new StreamReader(FILE_PATH))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    int key = int.Parse(line.Split(SPLIT_CHAR)[0]);
                    Item item = allItemCodes[key];
                    int count = int.Parse(line.Split(SPLIT_CHAR)[1]);
                    // for (int i = 0; i < count; i++) {
                    player.GetComponent<Inventory>().LoadItem(item, count);
                    //int invListLength = player.GetComponent<Inventory>().inventoryList.Count;
                    //player.GetComponent<Inventory>().inventoryList[invListLength].amount = count;
                    // }
                }
            }
        }
    }
}
