using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFadeScreen : MonoBehaviour
{
   private Animator anim;
   
   private static readonly int Out = Animator.StringToHash("fadeOut");
   private static readonly int In = Animator.StringToHash("fadeIn");

   private void Awake()
   {
      anim = GetComponent<Animator>();
   }

   public void FadeOut()
   {
      anim.SetTrigger(Out);
   }
   
   public void FadeIn()
   {
      anim.SetTrigger(In);
   }
}
