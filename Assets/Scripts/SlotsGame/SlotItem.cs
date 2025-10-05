using UnityEngine;

public class SlotItem : MonoBehaviour
{
    public float startingY;

    public void SetStartingY()
    {
        startingY = transform.localPosition.y;
    }

    public void GoToStartingY(float offset)
    {
        transform.localPosition = new Vector3 (transform.localPosition.x, startingY + offset, transform.localPosition.z);
    }
}
