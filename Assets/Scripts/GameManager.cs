using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        SFXManager.Instance.PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
