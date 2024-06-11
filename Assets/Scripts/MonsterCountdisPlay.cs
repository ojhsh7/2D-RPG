using UnityEngine;
using UnityEngine.UI;

public class MonsterCountDisplay : MonoBehaviour
{
    public Text monsterCountText;

    void Update()
    {
        // 몬스터의 개수를 가져와 UI를 업데이트합니다.
        int monsterCount = MonsterManager.Instance.GetMonsterCount();
        monsterCountText.text = $": {monsterCount}";
    }
}
