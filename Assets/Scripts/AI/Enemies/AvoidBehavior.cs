using UnityEngine;

public class AvoidBehavior : SteeringBehavior
{
    [SerializeField]
    private float avoidanceDistance = 5f;

    [SerializeField]
    private bool showGizmo = true;
    
    private Vector2 avoidanceDirection;
    private float[] avoidanceInterestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        Vector2 directionAwayFromPlayer = (Vector2)transform.position - (Vector2)aiData.currentTarget.position;
        
        if (directionAwayFromPlayer.magnitude <= avoidanceDistance)
        {
            avoidanceDirection = directionAwayFromPlayer.normalized;
            
            for (int i = 0; i < interest.Length; i++)
            {
                float result = Vector2.Dot(avoidanceDirection, Directions.eightDirections[i]);
                
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

