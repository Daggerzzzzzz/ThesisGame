using System.Linq;
using UnityEngine;

public class FleeBehavior : SteeringBehavior
{
    [SerializeField]
    private float dangerThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    private Vector2 targetPositionCached;
    private float[] interestsTemp;

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
            for (int i = 0; i < interest.Length; i++)
            {
                float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);
                if (result > 0)
                {
                    float valueToPutIn = result;
                    if (valueToPutIn > interest[i])
                    {
                        interest[i] = valueToPutIn;
                    }
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
