using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

    private void Start() {
            GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
    }

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
    }
}