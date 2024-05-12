using UnityEngine;

public class FleeBehavior : SteeringBehavior
{
    [SerializeField]
    private float fleeDistance = 5f;

    [SerializeField]
    private bool showGizmo = true;

    // Cache parameters for gizmos
    private Vector2 fleePositionCached;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        // Flee behavior: move away from the current target
        if (aiData.currentTarget == null)
            return (danger, interest);

        // Calculate direction to flee
        Vector2 directionToFlee = transform.position - aiData.currentTarget.position;
        directionToFlee.Normalize();

        // Update interest directions based on fleeing direction
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToFlee.normalized, Directions.eightDirections[i]);

            // Accept only directions less than 90 degrees to the fleeing direction
            if (result > 0)
            {
                float valueToPutIn = result * fleeDistance; // Scale by distance to flee
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }

        // Cache the flee position
        fleePositionCached = (Vector2)transform.position + directionToFlee * fleeDistance;

        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo)
            return;

        // Draw the flee position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(fleePositionCached, 0.2f);
    }
}