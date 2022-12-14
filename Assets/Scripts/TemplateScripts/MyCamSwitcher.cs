using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using JetBrains.Annotations;
using udoEventSystem;

public class MyCamSwitcher : MonoBehaviour
{
    public static MyCamSwitcher Instance { private set; get; }

    public CinemachineVirtualCamera mainVirtualCam;
    private CinemachineTransposer _mainCamTransposer;

    public float transitionDamping = 1.5f;

    public List<CamStates> cityCams = new List<CamStates>();
    public List<CinemachineVirtualCamera> cityVCams = new List<CinemachineVirtualCamera>();

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
        
        _mainCamTransposer = mainVirtualCam.GetCinemachineComponent<CinemachineTransposer>();
    }
    
    private bool isRunwayCam;
    private Animator _animator;

    public float movedBackEndCamDelay = 1;

    private void OnEnable()
    {
        EventManager.Get<LevelCompleted>().AddListener(SwitchToCameraThatIsSomeDistanceBehind);
    }
    
    private void OnDisable()
    {
        EventManager.Get<LevelCompleted>().RemoveListener(SwitchToCameraThatIsSomeDistanceBehind);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void SwitchToCam(CamStates newState)
    {
        if (_animator != null)
        {
            //switch (newState)
            //{
            //    case CamStates.DefaultCam:
            //        _animator.Play("DefaultCam");
            //        break;
            //    case CamStates.RunwayCam:
            //        _animator.Play("RunwayCam");
            //        break;
            //    case CamStates.EndGameCam:
            //        _animator.Play("EndGameCam");
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            //}

            _animator.Play(newState.ToString());
                
            //CinemachineShake.Instance.ChangeActiveCameraIndex(1);
        }
        else
        {
            Debug.LogWarning("Animator null -- But how ??!!");
        }
    }

    public void ChangeCamTargetToCityCamWithID(Transform newTarget, int cityID)
    {
        cityVCams[cityID].Follow = newTarget;
    }

    public void SwitchToCamThenTurnBackDisablingInput(int camIndex)
    {
        EventManager.Get<StopUserInput>().Execute();
                    
        SwitchToCam(cityCams[camIndex]);
        
        DOVirtual.DelayedCall(GameDataManager.Instance.CamSwitchDuration, () =>
        {
            SwitchToCam(CamStates.DefaultCam);
                        
            EventManager.Get<StartUserInput>().Execute();
        
        }, false);            
    }
    
    public void SwitchToCamThenTurnBackDisablingInput(CamStates camState)
    {
        EventManager.Get<StopUserInput>().Execute();
                    
        SwitchToCam(camState);
        
        DOVirtual.DelayedCall(GameDataManager.Instance.CamSwitchDuration, () =>
        {
            SwitchToCam(CamStates.DefaultCam);
                        
            EventManager.Get<StartUserInput>().Execute();
        
        }, false);            
    }

    public void SwitchMainCamTargetThenTurnBackDisablingInput(Transform newTarget)
    {
        EventManager.Get<StopUserInput>().Execute();

        mainVirtualCam.Follow = newTarget;
        
        DOVirtual.DelayedCall(3.5f, () =>
        {
            EventManager.Get<StartUserInput>().Execute();

            mainVirtualCam.Follow = Player.Instance.transform;

        }, false);     
    }
    
    public void SwitchMainCamTargetThenTurnBackDisablingInputWithDampening(Transform newTarget)
    {
        EventManager.Get<StopUserInput>().Execute();

        Transform mainCamTarget = mainVirtualCam.Follow;

        mainVirtualCam.Follow = newTarget;
        _mainCamTransposer.m_XDamping = transitionDamping;
        _mainCamTransposer.m_YDamping = transitionDamping;
        _mainCamTransposer.m_ZDamping = transitionDamping;
        
        DOVirtual.DelayedCall(GameDataManager.Instance.CamSwitchDuration, () =>
        {
            mainVirtualCam.Follow = mainCamTarget;

            DOVirtual.DelayedCall(1, () =>
            {
                EventManager.Get<StartUserInput>().Execute();
                
                _mainCamTransposer.m_XDamping = 0;
                _mainCamTransposer.m_YDamping = 0;
                _mainCamTransposer.m_ZDamping = 0;
            }, false);

        }, false);     
    }

    private void SwitchToCameraThatIsSomeDistanceBehind()
    {
        // DOVirtual.DelayedCall(movedBackEndCamDelay, () =>
        // {
        //     //SwitchToCam(CamStates.EndGameCamMovedBack);
        // });
    }
    
    public enum CamStates
    {
        DefaultCam,
        EndGameCam,
        EndGameCamMovedBack,
        EndGameMidCam,
        FailCam,
        DamViewCam,
        CityViewCam1,
        CityViewCam2,
        CityViewCam3,
        
    }

    // public void SwitchToRunwayCam()
    // {
    //     if (!isRunwayCam)
    //     {
    //         if (_animator != null)
    //         {
    //             _animator.Play("RunwayCam");
    //             
    //             //CinemachineShake.Instance.ChangeActiveCameraIndex(1);
    //         }
    //         else
    //         {
    //             Debug.LogWarning("Animator null -- But how ??!!");
    //         }
    //         
    //
    //         isRunwayCam = true;
    //     }
    // }
    //
    // public void SwitchBackToDefaultCam()
    // {
    //     if (isRunwayCam)
    //     {
    //         if (_animator != null)
    //         {
    //             _animator.Play("DefaultCam");
    //             
    //             //CinemachineShake.Instance.ChangeActiveCameraIndex(1);
    //         }
    //         else
    //         {
    //             Debug.LogWarning("Animator null -- But how ??!!");
    //         }
    //         
    //
    //         isRunwayCam = false;
    //     }
    // }
}
