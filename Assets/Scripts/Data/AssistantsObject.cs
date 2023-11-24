using System.Collections.Generic;
using UnityEngine;

public class AssistantsObject : MonoBehaviour
{
    public static AssistantsObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("My Assistants All List")]
    [SerializeField] public List<AssisstantDetail> _assistantsAllList;
}
