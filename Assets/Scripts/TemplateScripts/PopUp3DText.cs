using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class PopUp3DText : MonoBehaviour
{
    public bool updateRotationEveryFrame;
    
    [Header("PunchScale")] 
    public float power = 5;
    public float punchScaleDuration = 0.5f;
    public int vibrato = 0;
    public float elasticity = 0.5f;
    public bool punchScale;
    
    // [Header("ScaleUp")]
    // public bool scaleUp;
    // public float scaleUpToMultiplier = 2;
    // public float scaleUpDuration = 0.5f;
    // public Ease scaleUpEase = Ease.InElastic;
    
    [Header("RisingUp")]
    public float height = 300;
    public float riseDuration = 0.8f;
    public float riseDelay = 0.2f;
    public Ease raiseEase = Ease.OutCubic;

    [Header("Fade")]
    public float fadeDuration = 0.5f;
    public float fadeDelay = 0.15f;
    public Ease fadeEase = Ease.InCubic;

    [Space(10)]
    public bool inCanvas;

    public Pools.Types poolType;

    private TMP_Text _thisText;

    private Camera _mainCam;

    private Color _initialColor;
    private Vector3 _initialAnchoredPosition;
    private Vector3 _initialScale;

    private bool risingAndFading;

    private Tween riseTween;
    private Tween fadeTween;
    private Tween spriteFadeTween;

    private SpriteRenderer _spriteRenderer;
    private Color _spriteInitialColor;

    private void Awake()
    {
        _thisText = GetComponent<TMP_Text>();
        
        _initialColor = _thisText.color;
        _initialAnchoredPosition = _thisText.rectTransform.anchoredPosition;
        _initialScale = _thisText.rectTransform.localScale;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteInitialColor = _spriteRenderer.color;
    }
    
    private void OnEnable()
    {
        if (true)//SceneManager.GetActiveScene().buildIndex > 1)
        {
            try
            {
                if (_mainCam == null)
                {
                    _mainCam = Camera.main;
                }

                if (!risingAndFading)
                {
                    RiseAndFade();
                }
            }
            catch(Exception e)
            {
                Debug.Log("exception caught : " + e);
            }
            
        }
    }

    private void Update()
    {
        if (updateRotationEveryFrame)
        {
            CameraAlignment();
        }
    }

    private void RiseAndFade()
    {
        risingAndFading = true;
        //CameraAlignment();

        CameraAlignment();

        if (punchScale)
        {
            _thisText.transform.DOPunchScale(Vector3.one * power, punchScaleDuration, vibrato, elasticity);
        }
        
        DOVirtual.DelayedCall(fadeDelay,() =>
        {
            fadeTween = _thisText.DOFade(0, fadeDuration)
                .SetEase(fadeEase).OnComplete(BackToPool);
            
            if (_spriteRenderer != null)
            {
                spriteFadeTween = _spriteRenderer.DOFade(0, fadeDuration).SetEase(fadeEase);
            }
        }, false);

        DOVirtual.DelayedCall(riseDelay,() =>
        {
            riseTween = _thisText.rectTransform.DOAnchorPosY(_thisText.rectTransform.anchoredPosition.y + height, riseDuration);
        }, false);
        //thisText.transform.DOLocalMoveY(thisText.transform.localPosition.y + height, riseDuration).SetEase(raiseEase);
    }

    private void CameraAlignment()
    {
        //transform.parent = mainCam.transform;
        //transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (inCanvas)
        {
            transform.parent.rotation = _mainCam.transform.rotation;
        }
        else 
        {
            transform.rotation = _mainCam.transform.rotation;
        }
    }

    private void BackToPool()
    {
        ResetForPooling();
        gameObject.SetActive(false);

        if (inCanvas)
        {
            PoolManager.Instance.Despawn(poolType, transform.parent.gameObject);
        }
        else
        {
            PoolManager.Instance.Despawn(poolType, gameObject);
        }
    }

    private void ResetForPooling()
    {
        risingAndFading = false;
        
        fadeTween?.Kill();
        _thisText.color = _initialColor;
        
        if (_spriteRenderer != null)
        {
            spriteFadeTween?.Kill();
            _spriteRenderer.color = _spriteInitialColor;
        }
        
        riseTween?.Kill();
        _thisText.rectTransform.anchoredPosition = _initialAnchoredPosition;
        //Debug.Log("anchoredPos " + thisText.rectTransform.anchoredPosition);
        //Debug.Log("initialAnchoredPosition " + initialAnchoredPosition);

        _thisText.rectTransform.localScale = _initialScale;
    }
}