using System.Collections.Generic;
using UnityEngine;

public class GuyGen : MonoBehaviour
{
    public List<GameObject> bods;
    private GameObject bod;
    private GameObject bodInstance;

    public List<GameObject> parts;

    public int partCount = 6;
    public float rayRadius = 5f;
    
    private void Start()
    {
        CreateGuy();
    }

    public void CreateGuy()
    {
        SelectBod();
        ChuckParts();
    }

    private void SelectBod()
    {
        if (bods.Count == 0) return;
        var lastBod = bod;
        while (bod == lastBod)
        {
            var bodIndex = Random.Range(0, bods.Count);
            bod = bods[bodIndex];
        }

        var bodsParent = transform.Find("bods");
        var numChildren = bodsParent.childCount;
        for(var i=numChildren-1; i >= 0; i--)
        {
            Destroy(bodsParent.GetChild(i).gameObject);
        }

        bodInstance = Instantiate(bod, bodsParent);
    }

    private void ChuckParts()
    {
        if (!bod) return;
        if (parts.Count == 0) return;

        var bodCollider = bodInstance.transform.GetChild(0).GetComponent<MeshCollider>();
        
        var partParent = transform.Find("parts");
        var numChildren = partParent.childCount;
        for (var i=numChildren-1; i >= 0; i--)
        {
            Destroy(partParent.GetChild(i).gameObject);
        }
        
        for (var i = 0; i < partCount; ++i)
        {
            var direction = Random.onUnitSphere * rayRadius;
            var ray = new Ray(direction, -direction);
            var ithit = bodCollider.Raycast(ray, out var hitInfo, rayRadius);
            Debug.Log(ithit);

            var partIndex = Random.Range(0, parts.Count);
            var part = Instantiate(parts[partIndex], hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            part.transform.parent = partParent;
        }
    }
}
