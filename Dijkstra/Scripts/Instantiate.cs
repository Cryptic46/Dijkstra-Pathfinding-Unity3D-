using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    [SerializeField]
    private Transform zombie;

    private void Start()
    {
        Instantiate(zombie, transform.position, Quaternion.identity);
    }
}
