﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXParticleController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        int rotation = Random.Range(0, 360);
        float scale = Random.Range(0.2f, 1f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        transform.localScale = new Vector3(scale, scale, scale);
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
