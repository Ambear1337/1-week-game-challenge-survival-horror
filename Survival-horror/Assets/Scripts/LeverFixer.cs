using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverFixer : MonoBehaviour
{
    public Transform[] cogs;

    public bool allCogsSettedUp = false;

    public void CheckIfAllCogsAreSettedUp()
    {
        foreach (var t in cogs)
        {
            if (!t.gameObject.activeSelf)
            {
                allCogsSettedUp = false;
                return;
            }
            else
            {
                allCogsSettedUp = true;
            }
        }
        
        //Play sound of fixing mechanism
    }
}
