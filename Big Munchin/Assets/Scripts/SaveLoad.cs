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
        LoadInventory();
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
    public void NewGame()
    {
        string saveString = "";
        File.WriteAllText(Application.dataPath + "/save.txt", saveString);
        ClearInventorySaveFile();
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
            for (int i = 0; i < player.GetComponent<Inventory>().inventoryList.Count; i++)
            {
                Item currentItem = player.GetComponent<Inventory>().inventoryList[i];

                // Serialize arrays into strings
                string buffTypesString = string.Join(",", currentItem.itemBuffTypes);
                string durationString = string.Join(",", currentItem.effectDuration);
                string valueString = string.Join(",", currentItem.effectValue);

                // Construct the line to be saved
                string invString =
                    currentItem.itemName + "|" +
                    currentItem.flavorText + "|" +
                    currentItem.amount + "|" +
                    buffTypesString + "|" +
                    durationString + "|" +
                    valueString + "\n";


                /* NGcontroller needs to add a few values to meals: (up to i think 4 buffs, each must have same size array)
                    temBuffTypes;//when adding a new item, fill which buff types it has
                        0 = crit chance up, 
                        1 = max stamina up,
                        2 = stamina regen up, 
                        3 = stamina cost decrease, 
                        4 = player speed up, 
                        5 = instant healing, 
                        6 = healing per second
                    effectDuration
                    effectValue
                */

                Debug.Log(invString);

                // Write the line to the file
                sw.WriteLine(invString);
            }
        }
    }
    /*
    public void SaveInventory()
    {
        using (StreamWriter sw = new StreamWriter(FILE_PATH))
        {
            sw.Write("");

                for (int i = 0; i < player.GetComponent<Inventory>().inventoryList.Count; i++) {
                string invString = 
                    player.GetComponent<Inventory>().inventoryList[i].itemName + "|" + 
                    player.GetComponent<Inventory>().inventoryList[i].flavorText + "|" + 
                    player.GetComponent<Inventory>().inventoryList[i].amount + "|" +
                    player.GetComponent<Inventory>().inventoryList[i].itemBuffTypes[] + "|" +

                    +"\n";
                Debug.Log(invString);
                    sw.WriteLine(invString); 
                }
           

        }
    }
    */
                /*
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
             */
                public void LoadInventory()
    {
        loading = false;

        if (InventorySaveExists())
        {
            using (StreamReader sr = new StreamReader(FILE_PATH))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    string[] parts = line.Split('|');

                    if (parts.Length < 6)
                    {
                        Debug.LogError(parts.Length + "1- Invalid line format: " + line);
                        for (int t = 0; t < parts.Length; t++) {
                            Debug.Log(parts[t]);
                        }
                        continue; // Skip this line and proceed with the next one
                    }

                    Item loadedItem = new Item();
                    loadedItem.itemName = parts[0];
                    loadedItem.flavorText = parts[1];
                    if (!int.TryParse(parts[2], out loadedItem.amount))
                    {
                        Debug.LogError("2- Invalid amount format: " + parts[2]);
                        continue; // Skip this line and proceed with the next one
                    }

                    // Deserialize arrays
                    string[] buffTypeStrings = parts[3].Split('|');
                    int[] buffTypes = new int[buffTypeStrings.Length];
                    for (int j = 0; j < buffTypeStrings.Length; j++)
                    {
                        if (!int.TryParse(buffTypeStrings[j], out buffTypes[j]))
                        {
                            Debug.LogError("3- Invalid buff type format: " + buffTypeStrings[j]);
                            continue; // Skip this value and proceed with the next one
                        }
                    }

                    float[] durations = Array.ConvertAll(parts[4].Split(','), float.Parse);
                    float[] values = Array.ConvertAll(parts[5].Split(','), float.Parse);

                    // Set arrays to the loaded item
                    loadedItem.itemBuffTypes = buffTypes;
                    loadedItem.effectDuration = durations;
                    loadedItem.effectValue = values;
                    player.GetComponent<Inventory>().addItem(loadedItem);
                }
            }
        }
    }
    /*
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
                    string[] lMeals = line.Split('!');
                    for (int i = 0; i < lMeals.Length; i++)
                    {
                        string[] parts = line.Split('|');

                        if (parts.Length != 3)
                        {
                            Debug.LogError("Invalid line format: " + line);
                            continue; // Skip this line and proceed with the next one
                        }

                        Item completeMeal = new Item();
                        completeMeal.itemName = parts[0];
                        completeMeal.flavorText = parts[1];
                        completeMeal.amount = Int32.Parse(parts[2]);
                        Debug.Log(parts[0] + " " + parts[1] + " " + parts[2]);
                        if (!int.TryParse(parts[2], out completeMeal.amount))
                        {
                            Debug.LogError("Invalid amount format: " + parts[2]);
                            continue; // Skip this line and proceed with the next one
                        }

                        player.GetComponent<Inventory>().LoadItem(completeMeal, completeMeal.amount);
                    } 

                }
            }
        }
    }
    */

    /*
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

                    Item completeMeal = new Item();
                    completeMeal.itemName = line.Split("|")[0];
                    Debug.Log(line.Split("|")[0]);
                    completeMeal.flavorText = line.Split("|")[1];
                    Debug.Log(line.Split("|")[1]);

                    completeMeal.amount = Int32.Parse(line.Split("|")[2]);
                    Debug.Log(line.Split("|")[2]);

                    playerForInventory.GetComponent<Inventory>().addItem(completeMeal);
                    /*
                    int key = int.Parse(line.Split(SPLIT_CHAR)[0]);
                    Item item = allItemCodes[key];
                    int count = int.Parse(line.Split(SPLIT_CHAR)[1]);
                    // for (int i = 0; i < count; i++) {
                    player.GetComponent<Inventory>().LoadItem(item, count);
                    //int invListLength = player.GetComponent<Inventory>().inventoryList.Count;
                    //player.GetComponent<Inventory>().inventoryList[invListLength].amount = count;
                    // }
                    */
    /*
                }
            }
        }
    }

    */
}
