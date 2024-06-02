using TMPro;
using UnityEngine;

public class InformationEffect : MonoBehaviour
{
    [SerializeField] 
    private float speed;
    [SerializeField] 
    private float disappearingSpeed;
    [SerializeField] 
    private float lifeTime;
    [SerializeField] 
    private float colorDisapperanceSpeed;

    private float textTimer;
    private TextMeshPro myText;

    private void Start()
    {
        myText = GetComponent<TextMeshPro>();   
    }

    private void Update()
    {
        textTimer -= Time.deltaTime;

        if (textTimer < 0)
        {
            float alpha = myText.color.a - colorDisapperanceSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a <= 0)
            {
                Destroy(gameObject);
            }

            if (myText.color.a < 50)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x, transform.position.y + 1), disappearingSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
            }
        }
    }
}
