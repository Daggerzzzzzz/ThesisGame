using UnityEngine;

public class AvoidBehavior : SteeringBehavior
{
    [SerializeField]
    private float avoidanceDistance = 5f;

    [SerializeField]
    private bool showGizmo = true;

    // Gizmo parameters
    private Vector2 avoidanceDirection;
    private float[] avoidanceInterestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        // Get the direction away from the player
        Vector2 directionAwayFromPlayer = (Vector2)transform.position - (Vector2)aiData.currentTarget.position;

        // Check if the player is within the avoidance distance
        if (directionAwayFromPlayer.magnitude <= avoidanceDistance)
        {
            // Normalize the direction to have a magnitude of 1
            avoidanceDirection = directionAwayFromPlayer.normalized;

            // Modify the interest array based on the avoidance direction
            for (int i = 0; i < interest.Length; i++)
            {
                float result = Vector2.Dot(avoidanceDirection, Directions.eightDirections[i]);

                // Accept only directions less than 90 degrees away from the avoidance direction
                if (result > 0)
                {
                    float valueToPutIn = result;
                    if (valueToPutIn > interest[i])
                    {
                        interest[i] = valueToPutIn;
                    }
                }
            }
            avoidanceInterestsTemp = interest;
        }
        else
        {
            // If the player is not within the avoidance distance, reset interest array
            for (int i = 0; i < interest.Length; i++)
            {
                interest[i] = 0f;
            }
            avoidanceInterestsTemp = null;
        }

        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        if (avoidanceInterestsTemp != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < avoidanceInterestsTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * avoidanceInterestsTemp[i] * 2);
            }
        }
    }
}

