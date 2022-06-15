using UnityEngine;

public class Chunk : MonoBehaviour
{
    public string typeOfChunk;
    public Transform Start;
    public Transform End;

    private void Awake()
    {
        this.gameObject.name = this.gameObject.name.Replace("(Clone)", "");
        this.gameObject.tag = "Chunk";
    }
}