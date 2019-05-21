using UnityEngine;

[CreateAssetMenu(fileName = "NewAudio", menuName = "System/UIScreen", order = 2)]
public class UIScreen : ScriptableObject
{
    public string nameCanvas;
    public Canvas source;
    public float referencePPU;
}
