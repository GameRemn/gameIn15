using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    public int speed;
    public Vector3 target;
    NumbersManager numbersManager;
    AudioSource audioSource;
    void Start()
    {
        numbersManager = transform.parent.GetComponent<NumbersManager>();
        audioSource = GetComponent<AudioSource>();
        target = transform.position;
    }
    void Update()
    {
    }
    private void OnMouseDown()
    {
        numbersManager.NewTarget(transform);
    }
}
