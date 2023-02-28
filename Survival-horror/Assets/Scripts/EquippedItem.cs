using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquippedItem : MonoBehaviour
{
    public abstract void Use();

    public abstract void Drop();

    public abstract void DestroyItem();
}
