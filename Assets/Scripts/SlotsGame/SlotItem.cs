using UnityEngine;

public class SlotItem : MonoBehaviour
{
    public float startingY;

    private void Awake()
    {
        startingY = transform.localPosition.y;
    }
}
