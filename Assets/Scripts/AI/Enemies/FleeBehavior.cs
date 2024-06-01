using System.Linq;
using UnityEngine;

public class FleeBehavior : SteeringBehavior
{
    [SerializeField]
    private float dangerThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    [SerializeField]
    private float centerAttractionStrength = 0.3f;

    private Vector2 targetPositionCached;
    private float[] interestsTemp;
    
    [SerializeField]
    private Vector2 roomCenterPosition;
    [SerializeField]
    private RoomCenter roomCenter;

    private void Start()
    {
        roomCenter = GetComponentInParent<RoomCenter>();
        roomCenterPosition = roomCenter.roomCenterPos;
        if (roomCenter != null)
        {
            roomCenterPosition = roomCenter.roomCenterPos;
        }
    }

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        if (aiData.targets is not { Count: > 0 })
        {
            aiData.currentTarget = null;
            return (danger, interest);
        }

        aiData.currentTarget = aiData.targets.OrderBy
            (target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();

        if (aiData.currentTarget != null && aiData.targets.Contains(aiData.currentTarget))
            targetPositionCached = aiData.currentTarget.position;

        if (aiData.currentTarget != null)
        {
            Vector2 directionToTarget = (Vector2)transform.position - targetPositionCached;
            Vector2 directionToCenter = roomCenterPosition - (Vector2)transform.position;

            for (int i = 0; i < interest.Length; i++)
            {
                float fleeDot = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);
                float centerDot = Vector2.Dot(directionToCenter.normalized, Directions.eightDirections[i]);

                if (fleeDot > 0)
                {
                    interest[i] = Mathf.Max(interest[i], fleeDot);
                }

                if (centerDot > 0)
                {
                    float adjustedCenterDot = centerDot * centerAttractionStrength;
                    interest[i] = Mathf.Max(interest[i], adjustedCenterDot);
                }
            }
        }

        for (int i = 0; i < danger.Length; i++)
        {
            if (danger[i] > dangerThreshold)
            {
                interest[i] = 0;
            }
        }

        interestsTemp = interest;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < interestsTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPositionCached, 0.1f);
        }
    }
}
