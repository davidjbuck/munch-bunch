using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    string myFilePath, fileName;
    public GameObject player;
    public GameObject playerH;
   // public GameObject inventoryL;
    //body for rotation which wasnt working
    //public GameObject pTurn;
    // Start is called before the first frame update
    void Start()
    {
        fileName = "save.txt";
        fileName = "saveInventory.txt";

        myFilePath = Application.dataPath + "/" + fileName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Load()
    {
        string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
        string[] playerLocationLoad = saveString.Split(',');
        SceneManager.LoadScene(int.Parse(playerLocationLoad[0]));

        player.transform.position = new Vector3(float.Parse(playerLocationLoad[1]), float.Parse(playerLocationLoad[2]), float.Parse(playerLocationLoad[3]));
        //pTurn.transform.eulerAngles = new Vector3(0,float.Parse(playerLocationLoad[3]),0);
        /*
        string[] inventoryLoad = saveString.Split(',');
        for(int j = 0; j < player.GetComponent<Inventory>().inventoryList.Count; j++)
        {
            player.GetComponent<Inventory>().inventoryList.Clear();
        }
        for(int i = 0; i < inventoryLoad.Length; i++)
        {
            player.GetComponent<Inventory>().inventoryList.Add(inventoryLoad[i]);
        }
        */
    }
    public void SaveGame()
    {
        float posX, posY, posZ, rotX, rotY, rotZ;/*
        GameObject playerC = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(playerC.transform.position.x + " ");
        */
        Debug.Log("Save Game");
        string saveString = (SceneManager.GetActiveScene().buildIndex + "," + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z + "," + playerH.GetComponent<ThirdPersonController>().health);
        //tried to get rotation, but it wasnt working consistently
        // + "," + pTurn.transform.eulerAngles.y
        File.WriteAllText(Application.dataPath + "/save.txt", saveString);

        /*
        string saveInventory = "";
        
         List<Item> invList = player.GetComponent<Inventory>().inventoryList;
        for(int i = 0; i < invList.Count; i++)
        {
            saveInventory = saveInventory + invList[i] + ",";
            //saveInventory = saveInventory + inventoryL.inventoryList[i].itemName + "|" + inventoryL.inventoryList[i].amount + ",";
        }
        
        File.WriteAllText(Application.dataPath + "/saveInventory.txt", saveInventory);
        */
    }
}
