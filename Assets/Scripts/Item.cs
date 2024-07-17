using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Coin")
            {
                GameManager.Instance.PlayerStat.Coin += 10;
                Debug.Log("Player Coin :  " + GameManager.Instance.PlayerStat.Coin);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "HP")
            {
                GameManager.Instance.PlayerStat.HP += 10;
                Debug.Log("Player Coin : " + GameManager.Instance.PlayerStat.HP);
                Destroy(gameObject);
            }
        }
    }
}
