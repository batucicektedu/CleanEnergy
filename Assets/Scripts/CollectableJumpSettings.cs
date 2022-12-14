using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CollectableJumpSettings")]
public class CollectableJumpSettings : ScriptableObject
{
    [Header("Collection Animation")]
    public float collectionJumpPower = 5;
    public float collectionJumpDuration = 0.5f;
    public AnimationCurve collectionJumpEase;
    
    [Header("To Machine Animation")]
    public float toMachineJumpPower = 5;
    public float toMachineJumpDuration = 0.5f;
    public AnimationCurve toMachineJumpEase;
    
    [Header("To Metal Stack Animation")]
    public float toMetalStackJumpPower = 5;
    public float toMetalStackJumpDuration = 0.5f;
    public AnimationCurve toMetalStackJumpEase;
    
    [Header("To Cable Piece Animation")]
    public float toCablePieceJumpPower = 1;
    public float toCablePieceJumpDuration = 0.5f;
    public AnimationCurve toCablePieceJumpEase;
    
    [Header("To Money Pile Animation")]
    public float toMoneyPileJumpPower = 1;
    public float toMoneyPileJumpDuration = 0.5f;
    public AnimationCurve toMoneyPileJumpEase;
    public float toMoneyPunchScaleAmount = 3;
    public float toMoneyPilePunchScaleDuration = 0.33f;
    public float toMoneyPileRotationAmount = 360;
    
    [Header("Re-Order Stack Animation")]
    public float reOrderStackSpeed = 10f;
}