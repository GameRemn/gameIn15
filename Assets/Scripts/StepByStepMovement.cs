using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class StepByStepMovement : MonoBehaviour
{
    public  float duration;
    private Transform _transform;
    public bool isMoving;
    public void StartMoving(Vector3 target)
    {
        isMoving = true;
        var stepSequence = DOTween.Sequence();
        stepSequence.Append(transform.DOMove(target, duration, false));
        stepSequence.OnComplete(OnComplete);
    }

    private void OnComplete()
    {
        isMoving = false;
    }
}
