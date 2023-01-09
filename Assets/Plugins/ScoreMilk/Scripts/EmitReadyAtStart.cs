using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScoreMilk;

public class EmitReadyAtStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScoreMilk.Connection.EmitReady();
    }
}
