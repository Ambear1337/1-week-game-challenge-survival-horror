using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (FindObjectOfType<ScenesManager>() == null) return;

        FindObjectOfType<ScenesManager>().LoadMainMenuScene();
    }
}
