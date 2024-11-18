using UnityEngine;

[CreateAssetMenu(fileName = "Section", menuName = "Scriptable Objects/Section")]
public class Section : ScriptableObject
{
    public int bpm, duration;
}