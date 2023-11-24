using UnityEngine;

public class UnitLoader : MonoBehaviour
{
    public static UnitLoader Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
