using GDLib.Comms;
using UnityEngine;

public class EmotionLibrary : MonoBehaviour, IService
{
    [SerializeField] Sprite bumped;
    [SerializeField] Sprite dizzy;
    [SerializeField] Sprite preparing;
    [SerializeField] Sprite sleeping;
    public Sprite Bumped => bumped;
    public Sprite Dizzy => dizzy;
    public Sprite Preparing => preparing;
    public Sprite Sleeping => sleeping;

    private void Awake()
        => ServiceLocator.AddService("emotionLibrary", this);
}
