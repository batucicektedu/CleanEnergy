using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public enum Type
    {
        None = 0,
        Main = 5,
        Game = 10,
        Upgrade = 15,
        Revive = 16,
        WheelOfFortune = 20,
        ChestRoom = 25,
        LevelCompleted = 30,
        FailedLevel = 40,
        Continue = 45,
        SingleChest=50,
        Tutorial = 60,
        Rate = 70,
        LockUI = 160,
        Empty = 170,
        Shop = 190,
        ItemUnlockedPanel = 210,
        Settings = 220,
        LevelSelection = 250,
        Loading = 260,
        TapToStart = 265,
        YardPanel = 270,
    }

    public Type panelType;
    public bool isAlwaysPopup;
    [HideInInspector]
    public bool isCurrentlyPopup;

    public bool isCurrentlyOpen;
    public GameObject filterImage;
}