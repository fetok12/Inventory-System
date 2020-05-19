using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")] 
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;

//     private void onEnable(){
// #if UNITY_EDITOR
//         database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
// #else   
//     database = Resources.Load<ItemDatabaseObject>("Database");
// #endif
//     }

    public void AddItem(ItemObject _item, int _amount){

        if(_item.buffs.Length > 0){
            Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
            return;
        }


        bool hasItem = false;
        for(int i = 0; i< Container.Items.Count; i++){
            if(Container.Items[i].item.Id == _item.Id){
                Container.Items[i].AddAmount(_amount);
                hasItem = true; 
                break;
            }
        }
        if(!hasItem){
            // BURDAKI ID ITEME AIT, INVENTORYSLOT RECORDUNA DEGIL;
            //JS De Aslinda olmasi gereken {id:1,item:{id:2, itemName}, amount}
            // C# Ta {{id:2, itemName}, amount} YAZILIS:{2,itemName, amount}
            Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
        }
    }

[ContextMenu("Save")]
    public void Save(){ 
        // string saveData = JsonUtility.ToJson(this, true);
        // BinaryFormatter bf = new BinaryFormatter();
        // FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        // bf.Serialize(file, saveData);
        // file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();


    }
[ContextMenu("Load")]
    public void Load(){
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))){
            // BinaryFormatter bf = new BinaryFormatter();
            // FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            // JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            // file.Close();
             IFormatter formatter = new BinaryFormatter();
             Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
             Container = (Inventory)formatter.Deserialize(stream);
             stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear(){
        Container = new Inventory();
    }


    public void OnAfterDeserialize(){
    //  for (int i =0; i< Container.Items.Count; i++){
    //      Container.Items[i].item = database.GetItem[Container.Items[i].ID];
    //  }
    }
    public void OnBeforeSerialize(){ 

    }   

}

[System.Serializable]
public class Inventory{
    public List<InventorySlot> Items = new List<InventorySlot>();
}


[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObject item;
    public int amount;
    public ItemBuff[] buffs;
    public InventorySlot(int _id, ItemObject _item, int _amount){
        ID = _id;
        item = _item;
        amount = _amount;
        buffs = new ItemBuff[_item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max)
            {
                attribute = item.buffs[i].attribute
            };
        }
    }

    public void AddAmount(int value){
        amount += value;
    }
}