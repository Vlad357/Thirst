using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour
{
    public Transform rider;

    private void Update()
    {
        if(rider != null)
        {
            transform.position = rider.transform.position;
            transform.rotation = rider.transform.rotation;
        }
    }
}
