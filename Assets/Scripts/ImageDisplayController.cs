using System.Collections.Generic;
using UnityEngine;

public class ImageDisplayController : MonoBehaviour
{
    public static ImageDisplayController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Images Assistants")]
    [SerializeField] public List<Sprite> _assistantsRarityFrame = new List<Sprite>();
    [Header("Images Plants")]
    [SerializeField] public List<Sprite> _plantsRarityFrame = new List<Sprite>();
    [SerializeField] public List<Sprite> _plantsIcon = new List<Sprite>();
    [Header("Stage Plant")]
    [SerializeField] public Sprite _seed_Img;
    [SerializeField] public Sprite _babey_Img;
    [SerializeField] public Sprite _Rotted_Img;
    [Header("Honor")]
    [SerializeField] public List<Sprite> _honor_img = new List<Sprite>();

    private void Start()
    {
        foreach (Sprite item in Resources.LoadAll<Sprite>("Data/Cannabis/SpriteCannabis"))
        {
            _plantsIcon.Add(item);
        }
    }
}
