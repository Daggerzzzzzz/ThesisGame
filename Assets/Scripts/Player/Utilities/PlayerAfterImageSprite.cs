using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{

    private float activeTime = 0.15f;
    private float timeActivated;
    private float alpha;
    private float alphaSet = 0.8f;

    private Transform player;
    private SpriteRenderer SR;
    private SpriteRenderer playerSR;
    private Color color;
    private void OnEnable() 
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player Sprite").transform;
        playerSR = player.GetComponent<SpriteRenderer>();
        new Vector3(7f, 7f, 1f);

        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.localScale = new Vector3(7f, 7f, 1f);
        timeActivated = Time.time;
    }
    private void Update() {
        // Calculate the alpha value based on the time since activation
        float timeSinceActivated = Time.time - timeActivated;
        alpha = Mathf.Lerp(alphaSet, 0f, timeSinceActivated / activeTime);

        // Set the color with the calculated alpha
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if(Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
