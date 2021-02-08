using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarObject
{
    public Image icon { get; set; }
    public GameObject owner { get; set; }
}

public class Radar : MonoBehaviour
{
    public Transform playerPos;
    public float mapScale = 2.0f;

    public static List<RadarObject> radObjects = new List<RadarObject>();

    public static void RegisterRadarObject(GameObject o, Image i)
    {
        Image image = Instantiate(i);
        radObjects.Add(new RadarObject(){owner = o, icon = image});
    }

    public static void RemoveRadarObject(GameObject o)
    {
        List<RadarObject> newList = new List<RadarObject>();
        for (int i = 0; i < radObjects.Count; i++)
        {
            if (radObjects[i].owner == o)
            {
                Destroy(radObjects[i].icon);
                continue;
            }
            else
                newList.Add(radObjects[i]);
        }
        radObjects.RemoveRange(0,radObjects.Count);
        radObjects.AddRange(newList);
    }

    private void Update()
    {
        if (playerPos == null) return;
        foreach (RadarObject ro in radObjects)
        {
            Vector3 radPos = ro.owner.transform.position - playerPos.position;
            float distToObject = Vector3.Distance(playerPos.position, ro.owner.transform.position) * mapScale;
            float deltay = Mathf.Atan2(radPos.x, radPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
            radPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);
            ro.icon.transform.SetParent(transform);
            RectTransform rt = GetComponent<RectTransform>();
            ro.icon.transform.position = new Vector3(radPos.x + rt.pivot.x, radPos.z + rt.pivot.y,0) + transform.position;
        }
    }
}
