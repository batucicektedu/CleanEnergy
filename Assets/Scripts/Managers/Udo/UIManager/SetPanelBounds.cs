using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SetPanelBounds : MonoBehaviour
{

    //#if UNITY_IOS
    //    [DllImport("__Internal")]
    //    private extern static void GetSafeAreaForPanels(out float x, out float y, out float w, out float h);
    //#endif

    public static Rect GetSafeArea()
    {
        float x, y, w, h;
        //#if UNITY_IOS && !UNITY_EDITOR
        //        GetSafeAreaForPanels(out x, out y, out w, out h);
        //#else
        x = Screen.safeArea.x;
        y = Screen.safeArea.y;
        w = Screen.safeArea.width;
        h = Screen.safeArea.height;

        //x = 0;
        //y = 0;
        //w = Screen.width;
        //h = Screen.height;

#if UNITY_EDITOR
        ////LANDSCAPE
        //x = 132;
        //y = 63;
        //w = 2172; // 2436 - 132 - 132
        //h = 1062; // 1125 - 63

        //PORTRAIT
        if (Screen.width == 1125 && Screen.height == 2436)
        {
            x = 0;
            y = 102;
            w = 1125; // 1125
            h = 2202; // 2436 - 102 - 132
        }
#endif

        //#endif
        return new Rect(x, y, w, h);
    }

    //public RectTransform canvas;
    RectTransform panel;
    //Rect lastSafeArea = new Rect(0, 0, 0, 0);

    // Use this for initialization
    //void OnEnable()
    //void Awake()
    void Start()
    {
        panel = GetComponent<RectTransform>();
        Rect safeArea = GetSafeArea(); // or Screen.safeArea if you use a version of Unity that supports it

        //if (safeArea != lastSafeArea)
        ApplySafeArea(safeArea);
    }

    void ApplySafeArea(Rect area)
    {
        var anchorMin = area.position;
        var anchorMax = area.position + area.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

        //lastSafeArea = area;
    }

    public static float GetSafeAreaX()
    {
        return GetSafeArea().x;
    }
}
