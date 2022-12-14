using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using udoEventSystem;
using UnityEngine;

public class CollectableStacker : MonoBehaviour
{
    [Header("Stack")]
    public float distanceBetweenStackElements = 0.2f;
    public int stackCapacity = 20;

    public Transform stackParent;

    [Space(10)] 
    public float detachCollectibleInterval = 0.05f;
    private float _lastDetachedTime;
    
    [Space(10)] 
    public float takeMetalInterval = 0.05f;

    private float _lastMetalTakenTime;

    [SerializeField]private List<Collectable> collectableList = new List<Collectable>();

    private int _totalStackCount;

    private ThrashRecycleMachine _thrashRecycleMachine;

    [Header("Max Text")]
    [SerializeField] private float maxTextShowUpInterval = 3;

    [SerializeField] private GameObject maxText;

    private float _lastMaxTextShownTime = 0;

    private bool _tutorialCableSetUp;

    private void Start()
    {
        _tutorialCableSetUp = SaveManager.Instance.state.isTutorialShown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            var collectable = other.GetComponent<Collectable>();

            if (collectable.collectableState == Collectable.CollectableState.Active && !IsStackFull())
            {
                collectable.GetCollected(stackParent, 
                    GetNextStackElementLocation(), true);
                
                CollectCollectable(collectable);
            }
        }
        
        if (other.CompareTag("ThrashInTakeArea"))
        {
            _thrashRecycleMachine = other.GetComponentInParent<ThrashRecycleMachine>();
        }
        
        if (other.CompareTag("MetalOutputTrigger"))
        {
            _thrashRecycleMachine = other.GetComponentInParent<ThrashRecycleMachine>();
        }

        if (other.CompareTag("CableTransparent"))
        {
            DetachStackableCable(other.GetComponent<CablePiece>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ThrashInTakeArea"))
        {
            DetachThrash();
        }
        
        if (other.CompareTag("MetalOutputTrigger"))
        {
            TakeMetalFromMachine();
        }
    }

    private void TakeMetalFromMachine()
    {
        if (IsStackFull()) return;
        if (_thrashRecycleMachine.IsMetalStackEmpty()) return;
        if (_lastMetalTakenTime + takeMetalInterval > Time.time) return;

        _lastMetalTakenTime = Time.time;
            
        Collectable metalReceived = _thrashRecycleMachine
            .GiveMetalToPlayer(stackParent, GetNextStackElementLocation());
                
        CollectCollectable(metalReceived);
    }

    private void CollectCollectable(Collectable collectable)
    {
        collectableList.Add(collectable);
        
        _totalStackCount++;
    }

    private bool IsCollectibleTypeExists(CollectableType collectableType)
    {
        return collectableList.Exists(cl => cl.collectableType == collectableType);
    }

    private void DetachThrash()
    {
        if (!IsCollectibleTypeExists(CollectableType.ThrashChunk)) return;
        if (_lastDetachedTime + detachCollectibleInterval > Time.time) return;

        _lastDetachedTime = Time.time;

        _totalStackCount--;
            
        _thrashRecycleMachine.ReceiveThrash(
            DetachLastCollectableOfType(CollectableType.ThrashChunk));
    }

    private void DetachStackableCable(CablePiece cablePiece)
    {
        if (!IsCollectibleTypeExists(CollectableType.StackableCable)) return;
        if (cablePiece.IsCableThrowStarted()) return;

        if (!_tutorialCableSetUp)
        {
            _tutorialCableSetUp = true;
            
            EventManager.Get<SpawnThrashOnCity>().Execute(0);
            TutorialManager.Instance.ChangeToNextTutorialStep();
        }
        
        _totalStackCount--;

        cablePiece.SetCableThrowStartedTrue();
        
        cablePiece.CableThrowAction(DetachLastCollectableOfType(
            CollectableType.StackableCable));
    }

    private Collectable DetachLastCollectableOfType(CollectableType collectableType)
    {
        var lastOfThisType = 
            collectableList.FindLast(cl => cl.collectableType == collectableType);

        collectableList.Remove(lastOfThisType);

        ReOrderStackPositions();

        return lastOfThisType;
    }

    private void ReOrderStackPositions()
    {
        for (int i = 0; i < collectableList.Count; i++)
        {
            collectableList[i].ReOrderInStack(GetStackElementLocation(i));
        }
    }
    
    private Vector3 GetStackElementLocation(int listIndex)
    {
        return Vector3.up * distanceBetweenStackElements * listIndex;
    }

    private Vector3 GetNextStackElementLocation()
    {
        return Vector3.up * distanceBetweenStackElements * _totalStackCount;
    }

    private bool IsStackFull()
    {
        if (stackCapacity == _totalStackCount)
        {
            TryShowMaxTextWithAnimation();
            return true;
        }
        
        return false;
    }

    private void TryShowMaxTextWithAnimation()
    {
        if (_lastMaxTextShownTime + maxTextShowUpInterval > Time.time) return;
        
        _lastMaxTextShownTime = Time.time;
        
        maxText.SetActive(true);

        DOVirtual.DelayedCall(1.5f, () =>
        {
            maxText.SetActive(false);
        }, false);
    }
}
