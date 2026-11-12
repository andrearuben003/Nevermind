
using UnityEngine;

public class TimerConnection : MonoBehaviour
{
    [SerializeField]
    private Transform buttonPoint;

    [SerializeField]
    private Transform gatePoint;

    [SerializeField]
    private LineRenderer connectionLine;

    private void Awake()
    {
        connectionLine.positionCount = 2;
        connectionLine.useWorldSpace = true;
        connectionLine.enabled = false;
    }

    public void SetRemaining(float fraction)
    {
        //clamps from 0 to 1
        fraction = Mathf.Clamp01(fraction);

        connectionLine.enabled = fraction > 0f;

        if (!connectionLine.enabled)
            return;

        Vector3 start = buttonPoint.position;
        Vector3 end = gatePoint.position;

        connectionLine.SetPosition(0, start);

        //modifies the end of the rendered line using the percentage form the timer and the funcion Vector3.Lerp 
        connectionLine.SetPosition(
            1,
            Vector3.Lerp(start, end, fraction)
        );
    }
}
