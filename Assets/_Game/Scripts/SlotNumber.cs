using UnityEngine;

public class SlotNumber : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private Transform bottomLimit;
    private SlotNumber nextNumber;
    private bool isLooping;
    private void LateUpdate()
    {
        if(!isLooping)
        {
            return;
        }
        if (transform.position.y < bottomLimit.position.y)
        {
            transform.position = nextNumber.transform.position;
            transform.Translate(0, size, 0);
        }
    }
    public void SetNext(SlotNumber next)
    {
        nextNumber=next;
    }

    public void Move(float verticalMovement)
    {
        transform.Translate(0, verticalMovement, 0);
    }

    public void StopLooping()
    {
        isLooping = false;
    }
    public void StartLooping()
    {
        isLooping=true;
    }
}
