using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public string Name;
    public AudioSource Speaker;
    public AudioClip Clip;
    public string Sentence;
    public UnityEvent Callback;
}