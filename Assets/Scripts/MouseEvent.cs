using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    public GameObject Potion_UI;
    public GameObject Power_UI;
    public GameObject Battle_UI;
    void Update()
    {
        MouseClick();
    }


    private void MouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Pos, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "PowerNPC")
                {
                    Power_UI.gameObject.SetActive(true);
                }
                else if (hit.collider.gameObject.name == "PotionNPC")
                {
                    Potion_UI.gameObject.SetActive(true);
                }
                else if (hit.collider.gameObject.name == "BattleNPC")
                {
                   Battle_UI.gameObject.SetActive(true);
                }
            }
        }
    }
}
