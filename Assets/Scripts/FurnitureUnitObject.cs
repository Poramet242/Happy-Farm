using CannabisFarm.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureUnitObject : MonoBehaviour
{
    public static FurnitureUnitObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("All furnitureDetails")]
    [SerializeField] public List<FurnitureDetail> all_furnitureDetails;
    [Header("Funniture")]
    [SerializeField] public List<Decoration> _allDecorationsDataList;
}
