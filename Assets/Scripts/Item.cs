using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Coin")
            {
                CoinManager.Instance.AddCoin(10);
                Debug.Log("Player Coin :  " + CoinManager.Instance.Coin);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "HP")
            {
                GameManager.Instance.PlayerHP += 10;
                Debug.Log("Player HP : " + GameManager.Instance.PlayerHP);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "Speed")
            {
                GameManager.Instance.player.GetComponent<Character>().Speed += 10;
                Debug.Log("Player Speed : " + GameManager.Instance.player.GetComponent<Character>().Speed);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "Damage")
            {
                GameManager.Instance.player.GetComponent<Character>().AttackObj.GetComponent <Attack>().AttackDamage += 10;
                Debug.Log("Player Damage : " + GameManager.Instance.player.GetComponent<Character>().Damage);
                Destroy(gameObject);
            }
        }
    }
}
