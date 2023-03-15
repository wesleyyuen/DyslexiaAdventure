using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _player;

    private void Awake()
    {
        _player = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(_player);
    }
}
