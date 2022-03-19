using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapScroll : MonoBehaviour
{
    public RectTransform rightPos, leftPos;
    public RectTransform[] scrollElements;
    private Vector3 center;
    private float rightFactor, leftFactor;
    void Start()
    {
        center = GetComponent<RectTransform>().position;
        rightFactor = Vector3.Distance(rightPos.position, center);
        leftFactor = Vector3.Distance(leftPos.position, center);
    }

    void Update()
    {
        foreach (var element in scrollElements)
        {
            Vector3 elementPositionRelativeToCenter = element.InverseTransformPoint(center);
            if(elementPositionRelativeToCenter.x < 0) // on the right of the center
            {
                float DistanceToRight = Vector3.Distance(element.position, rightPos.position);
                float targetScale = (DistanceToRight / rightFactor);
                element.localScale = Vector3.one * targetScale;
            }
            else// on the left of the center or exactly at the center
            {
                float DistanceToLeft = Vector3.Distance(element.position, leftPos.position);
                float targetScale = (DistanceToLeft / leftFactor);
                element.localScale = Vector3.one * targetScale;
            }
        }
    }
}
