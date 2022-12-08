using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pillar : ITarget
{

    // no difference from parent Target Class this class is only used to give object type Pillar

    public override void Die()
    {
        isDestroyed = true;
        GetComponent<MeshRenderer>().enabled = false;

    }
}
