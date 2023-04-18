using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trenchBLPuzzle : MonoBehaviour
{
    public GameObject theGate;
    GateScr gScr;

    public void blPuzzleActivate()
    {
        gScr = theGate.GetComponent<GateScr>();
        gScr.gateClosed = true;
    }
}
