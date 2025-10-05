using UnityEngine;

public class SlotItem : MonoBehaviour
{
    // Movement
    public float startingY;

    // Value
    public SpriteRenderer valueRenderer;
    public int value;

    public void SetStartingY()
    {
        startingY = transform.localPosition.y;
    }

    public void GoToStartingY(float offset)
    {
        transform.localPosition = new Vector3 (transform.localPosition.x, startingY + offset, transform.localPosition.z);
    }

    public void SetValue(int _value)
    {
        value = _value;
        valueRenderer.sprite = SlotsGame.Instance.itemValueSprites[value];
    }
}
