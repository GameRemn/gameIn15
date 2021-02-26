using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get => _instance;
        set
        {
            if (_instance)
            {
                Destroy(value.gameObject);
            }
            else
            {
                _instance = value;
            }
        }
    }

    private void Awake()
    {
        Instance = this as T;
    }
}
