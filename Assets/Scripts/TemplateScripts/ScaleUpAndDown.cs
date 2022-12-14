using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleUpAndDown : MonoBehaviour
{
    private void Start()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(GetComponent<RectTransform>().DOScale(transform.localScale * 1.2f, 0.25f))
            .Append(GetComponent<RectTransform>().DOScale(transform.localScale, 0.25f))
            .SetLoops(-1);
    }
}
