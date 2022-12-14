using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using udoEventSystem;
using UnityEngine.PlayerLoop;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);    
        }
    }

    public CinemachineVirtualCamera[] _cinemachineVirtualCameras;
    private float shakeTimer;

    public float intensity = 1;
    public float time = 0.2f;

    public float intensityIncreaseMultiplier = 0.05f;

    private int currentActiveCameraIndex;

    private bool canIncreaseIntensity;
    
    private void OnEnable()
    {
        EventManager.Get<StartGame>().AddListener(StartIncreasingIntensity);
        EventManager.Get<PauseGame>().AddListener(StopIncreasingIntensity);
        EventManager.Get<CameraShake>().AddListener(ShakeCamera);
    }

    private void OnDisable()
    {
        EventManager.Get<StartGame>().RemoveListener(StartIncreasingIntensity);
        EventManager.Get<PauseGame>().RemoveListener(StopIncreasingIntensity);
        EventManager.Get<CameraShake>().RemoveListener(ShakeCamera);
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            _cinemachineVirtualCameras[currentActiveCameraIndex].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                //Time over
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    _cinemachineVirtualCameras[currentActiveCameraIndex].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }

        if (canIncreaseIntensity)
        {
            intensity += Time.deltaTime * intensityIncreaseMultiplier;
        }
        
    }

    public void ChangeActiveCameraIndex(int newIndex)
    {
        currentActiveCameraIndex = newIndex;
    }

    public void StartIncreasingIntensity()
    {
        canIncreaseIntensity = true;
    }

    public void StopIncreasingIntensity()
    {
        canIncreaseIntensity = false;
    }
}
