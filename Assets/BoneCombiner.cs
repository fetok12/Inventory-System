using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCombiner
{
    // public readonly Dictionary<int, Transform> _RootBoneDictionary = new Dictionary<int, Transform>();
    // public readonly Transform[] _boneTransforms = new Transform[67];

    // public readonly Transform _transform;
    public  Dictionary<int, Transform> _RootBoneDictionary = new Dictionary<int, Transform>();
    public  Transform[] _boneTransforms = new Transform[67];

    public  Transform _transform;

    public BoneCombiner(GameObject rootObj)
    {
        _transform = rootObj.transform;
        TraverseHierarchy(_transform);
    }

    public Transform AddLimb(GameObject bonedObj)
    {
        var limb = ProcessBonedObject(bonedObj.GetComponentInChildren<SkinnedMeshRenderer>());
        limb.SetParent(_transform);
        return limb;
    }

      private Transform ProcessBonedObject(SkinnedMeshRenderer renderer)
    {
        var bonedObject = new GameObject().transform;

        var meshRenderer = bonedObject.gameObject.AddComponent<SkinnedMeshRenderer>();

        var bones =renderer.bones;

        for (var i = 0; i < bones.Length; i++)
        {      
            // Debug.Log(bones[i].transform.position);
            // Debug.Log(_RootBoneDictionary[bones[i].name.GetHashCode()].transform.position);
            _boneTransforms[i] = _RootBoneDictionary[bones[i].name.GetHashCode()];
            // Debug.Log(_boneTransforms[i].transform.position);
        }

        meshRenderer.bones = _boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;

        return bonedObject;

    }


    private void TraverseHierarchy(Transform transform)
    {
        foreach (Transform child in transform)
        {   
            // Debug.Log(child);
            _RootBoneDictionary.Add(child.name.GetHashCode(), child);
            TraverseHierarchy(child);
        }
    }
    
}