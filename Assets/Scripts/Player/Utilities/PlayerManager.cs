using UnityEngine;

public class PlayerManager : SingletonMonoBehavior<PlayerManager>
{
   public Player player;
   public int soulCurrency;

   public bool HaveEnoughCurrency(int price)
   {
      if (price > soulCurrency)
      {
         Debug.Log("not enough money");
         return false;
      }

      soulCurrency -= price;

      return true;
   }
   
   public int CurrentSouls()
   {
      return soulCurrency;
   }
}
