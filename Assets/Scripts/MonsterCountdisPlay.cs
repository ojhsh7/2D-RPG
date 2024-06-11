using UnityEngine;
using UnityEngine.UI;

public class MonsterCountDisplay : MonoBehaviour
{
    public Text monsterCountText;

    void Update()
    {
        // ������ ������ ������ UI�� ������Ʈ�մϴ�.
        int monsterCount = MonsterManager.Instance.GetMonsterCount();
        monsterCountText.text = $": {monsterCount}";
    }
}
