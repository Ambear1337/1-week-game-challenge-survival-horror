using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cog : ConnectedObject
{
    public Transform leverFixer;
    
    public override void Execute()
    {
        leverFixer.GetComponent<LeverFixer>().CheckIfAllCogsAreSettedUp();
    }
}
