using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static bool hapticsActive;

    public static void Haptic(HapticTypes hapticType)
    {
        MMVibrationManager.Haptic(hapticType);
    }

    public void TransientHaptic(float intensity, float sharpness)
    {
        MMVibrationManager.TransientHaptic(intensity, sharpness);
    }

    public static void ToggleHaptics()
    {
        hapticsActive = !hapticsActive;
        
        MMVibrationManager.SetHapticsActive(hapticsActive);
    }
}
