using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class disabelOthers : MonoBehaviour
{
    public MonoBehaviour[] scriptToIgnore;
    private PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            foreach (var script in scriptToIgnore)
            {
                script.enabled = false;
            }
        }
    }

    void Update()
    {
        
    }
}
