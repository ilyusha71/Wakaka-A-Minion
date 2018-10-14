using DG.Tweening;
using System.Collections;
using UnityEngine;

public struct Hole
{
    public int index; // 洞口編號
    public Transform posIn;
    public Transform posOut;
    public Transform[] characters;
    public Transform nowLeave;
    public float durationTime;
}

public partial class WhacAMole : MonoBehaviour
{
    [Header("Hole Parameter")]
    public int countHole;
    public Transform holeIn;
    public Transform holeOut;
    private Hole[] holes;

    void InitializeHoles()
    {
        holes = new Hole[countHole];

        for (int i = 0; i < countHole; i++)
        {
            holes[i].index = i;
            holes[i].posIn = holeIn.GetChild(i);
            holes[i].posOut = holeOut.GetChild(i);
            holes[i].characters = new Transform[countCharacter];
            holes[i].durationTime = 1.37f;
            InstantiateCharacter(i);
        }
    }

    void Leaving(int index)
    {
        if (holes[index].nowLeave == null)
        {
            int summon = (int)(Time.time * 10) % magnitudeLevel;
            holes[index].nowLeave = holes[index].characters[levelBox[summon]];
            holes[index].nowLeave.
                DOMove(holes[index].posOut.position, 0.37f).SetEase(Ease.OutBack).OnComplete(() => StartCoroutine(LeavingCallback(index)));

            switch (levelBox[summon])
            {
                case (int)CharacterIndex.WangNiMa: numWangNiMa++; break;
            }
        }
    }

    IEnumerator LeavingCallback(int index)
    {
        // 確認DOTWEEN動作完成
        // 若被擊中則會中斷
        yield return new WaitForSeconds(holes[index].durationTime);
        holes[index].nowLeave.
            DOMove(holes[index].posIn.position, 0.37f).SetEase(Ease.InBack).OnComplete(() => Home(index));
    }

    void Home(int index)
    {
        if (holes[index].nowLeave != null)
        {
            if (holes[index].nowLeave.name.Contains("WangNiMa"))
                numWangNiMa--;

            holes[index].nowLeave.DOKill();
            holes[index].nowLeave.position = holes[index].posIn.position;
            holes[index].nowLeave = null;
        }
    }
}
