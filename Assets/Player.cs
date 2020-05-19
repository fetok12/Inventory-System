using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public InventoryObject inventory;

    private Transform boots;
    private Transform chest;
    private Transform helmet;
    private Transform offhand;
    private Transform sword;

    BoneCombiner boneCombiner;
    private void Start() {
     boneCombiner = new BoneCombiner(gameObject);
    }

    
    public void OnTriggerEnter(Collider other){
        var item = other.GetComponent<GroundItem>();
        if(item){
            inventory.AddItem(item.item, 1);
            boneCombiner.AddLimb(item.item.characterDisplay);
            Destroy(other.gameObject);
        }
    }
    private void Update(){
        //  Debug.Log(boneCombiner._RootBoneDictionary);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
        }
        
    }

    private void OnApplicationQuit(){
         inventory.Container.Items.Clear();
    }
}
