using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowsLine : MonoBehaviour
{
    public static TutorialArrowsLine Instance { private set; get; }

    [SerializeField] Transform _player;
    [SerializeField] List<Transform> _targetPoses = new List<Transform>();
    [SerializeField] List<Transform> _secondTargetPoses = new List<Transform>();
    LineRenderer _lineRenderer;
    Transform _currentTarget;
    int _targetCounter;
    int _secondTargetCounter;
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("double instance this singleton", gameObject);
            Destroy(gameObject);
        }
        
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        if (!SaveManager.Instance.state.isTutorialShown)
        {
            SetTarget();
            SetTargetPoint();
        }
    }

    private void Update()
    {
        SetTargetPoint();
    }
    
    public void StopLine()
    {
        _lineRenderer.enabled = false;
    }
    public void StartLine()
    {
        _lineRenderer.enabled = true;
    }
    
    public void SetTarget()
    {
        _currentTarget = _targetPoses[_targetCounter];
    }
    public void ChangeTarget()
    {
        _targetCounter++;
        
        if (TutorialManager.Instance.tutorialStepCount < _targetCounter) return;
        
        if (TutorialManager.Instance.tutorialStepCount == _targetCounter)
        {
            TutorialManager.Instance.FirstTutorialFinished();
            StopLine();
            return;
        }
        
        SetTarget();
        StartLine();
    }

    public void SetTargetFromSecondTargets()
    {
        _currentTarget = _secondTargetPoses[_secondTargetCounter];
    }

    public void ChangeTargetFromSecondTargets()
    {
        if (_secondTargetPoses.Count <= _secondTargetCounter) return;

        if (_secondTargetPoses.Count - 1 == _secondTargetCounter)
        {
            TutorialManager.Instance.EnableSecondTutorialUpgradeLocation();
        }

        SetTargetFromSecondTargets();
        StartLine();
        
        _secondTargetCounter++;
    }
    
    void SetTargetPoint()
    {
        if (_currentTarget == null) return;
        _lineRenderer.SetPosition(0, _player.position + (Vector3.up * 1));
        _lineRenderer.SetPosition(1, _currentTarget.position + (Vector3.up * 1));
    }

    public void SetTargetToBridgeCollider(Transform cityBridgeCollider)
    {
        _currentTarget = cityBridgeCollider;
        
        SetTargetPoint();
        
        StartLine();

        StartCoroutine(StopLineAfterDestinationReached());
    }

    private IEnumerator StopLineAfterDestinationReached()
    {
        WaitUntil waitUntil = new WaitUntil(
            () => Vector3.Distance(_player.position, _currentTarget.position) < 7.5f);
        
        yield return waitUntil;
        
        StopLine();
    }
}