using UnityEngine;
using System.Collections;

public class EnableSelectObjectOnAwake : MonoBehaviour 
{
    

    public GameObject obj;

    void Awake ()
    {
        obj.SetActive(true);
    }
}
