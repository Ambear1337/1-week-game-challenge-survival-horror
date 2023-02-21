using UnityEngine;

public class ItemAssets : MonoBehaviour {

    public static ItemAssets Instance { get; private set; }

    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    [Header("Equip Items")]
    [SerializeField] private GameObject lockPick;

    public GameObject Lockpick { get { return lockPick; } }
}
