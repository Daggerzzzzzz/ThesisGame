using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    [SerializeField]
    private Collider2D attackUp;
    [SerializeField]
    private Collider2D attackDown;
    [SerializeField]
    private Collider2D attackLeft;
    [SerializeField]
    private Collider2D attackRight;

    public void AttackRight()
    {
        attackRight.enabled = true;
    }

    public void AttackLeft()
    {
        attackLeft.enabled = true;
    }

    public void AttackUp()
    {
        attackUp.enabled = true;
    }

    public void AttackDown()
    {
        attackDown.enabled = true;
    }

    public void StopAttack()
    {
        attackRight.enabled = false;
        attackLeft.enabled = false;
        attackUp.enabled = false;
        attackDown.enabled = false;
    }
}
