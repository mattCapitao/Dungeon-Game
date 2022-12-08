using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pillar : ITarget
{
    public override void Die()
    {
        isDestroyed = true;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
