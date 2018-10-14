using UnityEngine;
using DG.Tweening;

public enum CharacterIndex
{
    Minion = 0, // +100
    SpongeBob = 1, // -300
    Mole =2, // Cross Bomb
    Dorara = 3, // increase Time
    Faceless = 4, // Silence
    WangNiMa = 5, // decrese score
    MengZong = 6, // hit all
    Cartman = 7, // HP=3 , hit surround
    Pandaman = 8, // infinity
    HuaJi = 9, // all home
}

public partial class WhacAMole : MonoBehaviour
{
    [Header("Character Parameter")]
    public GameObject[] characterPrefabs;
    private int countCharacter;
    private Transform[] characterContainers;
    private HitEventHandler<HItEventArgs>[] hitEventHandler;
    [Header("Character FX")]
    public GameObject fxHit;
    public GameObject fxExplosion;
    private ObjPoolData poolFXHit;
    private ObjPoolData poolFXExplosion;


    void InitializeCharacter()
    {
        countCharacter = characterPrefabs.Length; // 啟用角色
        characterContainers = new Transform[countCharacter];
        poolFXHit = GetComponent<ObjectPoolManager>().CreatObjPool(fxHit,30);
        poolFXExplosion = GetComponent<ObjectPoolManager>().CreatObjPool(fxExplosion, 30);
        hitEventHandler = new HitEventHandler<HItEventArgs>[countCharacter];
        SetHitEventHandler();
    }

    void InstantiateCharacter(int indexHole)
    {
        //if (indexHole != 4) return;
        for (int j = 0; j < countCharacter; j++)
        {
            if (characterContainers[j] == null)
            {
                characterContainers[j] = new GameObject().transform;
                characterContainers[j].name = characterPrefabs[j].name.Replace("Prefab", "Group");
                characterContainers[j].SetParent(this.transform);
            }
            holes[indexHole].characters[j] = Instantiate(characterPrefabs[j], holes[indexHole].posIn.position, Quaternion.identity).transform;
            holes[indexHole].characters[j].transform.localScale = characterPrefabs[j].transform.localScale;
            holes[indexHole].characters[j].transform.localRotation = this.transform.localRotation;
            holes[indexHole].characters[j].SetParent(characterContainers[j]);

            // 在Prefab的Collider組件中新增Character腳本
            Character character = holes[indexHole].characters[j].GetComponentInChildren<Collider>().gameObject.AddComponent<Character>();
            character.Initialize(hitEventHandler[j], new HItEventArgs(indexHole, (CharacterIndex)j));
        }
    }

    void SetHitEventHandler()
    {
        for (int i = 0; i < countCharacter; i++)
        {
            hitEventHandler[i] = HitDefault;
        }
        hitEventHandler[0] = HitMinion;
        hitEventHandler[1] = HitSponge;
        hitEventHandler[2] = HitMole;
        hitEventHandler[3] = HitDorara;
        hitEventHandler[4] = HitFaceless;
        hitEventHandler[5] = HitWangNiMa;
        hitEventHandler[6] = HitMengZong;
    }

    void HitDefault(object sender, HItEventArgs e)
    {

    }
    void HitMinion(object sender, HItEventArgs e)
    {
        if (Time.time > silence)
        {
            int index = e.index;
            capture++;
            textCapture.text = "" + capture;

            ExplosionFX fx =  poolFXHit.Reuse(holes[index].nowLeave.position, Quaternion.identity).GetComponent<ExplosionFX>();
            StartCoroutine(fx.Audio(Random.Range(2, 9)));
            Home(index);

            message[index].SetActive(true);
            message[index].transform.DOKill(true);
            message[index].transform.position = textIn[index].position;
            message[index].transform.DOMove(textOut[index].position, 0.37f).OnComplete(() => MsgBack(index));
            textMessage[index].text = "+100";
            score += 100;
            textScore.text = "" + (int)score;
        }
    }
    void HitSponge(object sender, HItEventArgs e)
    {
        if (Time.time > silence)
        {
            int index = e.index;
            countHitSponge++;

            ExplosionFX fx = poolFXHit.Reuse(holes[index].nowLeave.position, Quaternion.identity).GetComponent<ExplosionFX>();
            StartCoroutine(fx.Audio(9));
            Home(index);

            message[index].SetActive(true);
            message[index].transform.DOKill(true);
            message[index].transform.position = textIn[index].position;
            message[index].transform.DOMove(textOut[index].position, 0.37f).OnComplete(() => MsgBack(index));
            textMessage[index].text = "-300";
            score -= 300;
            textScore.text = "" + (int)score;
        }
    }
    void HitDorara(object sender, HItEventArgs e)
    {
        if (Time.time > silence)
        {
            int index = e.index;
            countHitDorara++;

            ExplosionFX fx = poolFXHit.Reuse(holes[index].nowLeave.position, Quaternion.identity).GetComponent<ExplosionFX>();
            StartCoroutine(fx.Audio(10));
            Home(index);

            message[index].SetActive(true);
            message[index].transform.DOKill(true);
            message[index].transform.position = textIn[index].position;
            message[index].transform.DOMove(textOut[index].position, 0.37f).OnComplete(() => MsgBack(index));
            textMessage[index].text = "+3sec";
            countdownTimer += 3;
        }       
    }
    void HitMole(object sender, HItEventArgs e)
    {
        if (Time.time > silence)
        {
            int index = e.index;
            countHitMole++;
            ExplosionFX fx = poolFXExplosion.Reuse(holes[index].nowLeave.position, Quaternion.identity).GetComponent<ExplosionFX>();
            StartCoroutine(fx.Audio(1));
            Home(index);

            tipCross.SetActive(true);
            tipCross.GetComponent<RectTransform>().localScale = Vector3.zero;
            tipCross.transform.DOScale(new Vector3(1, 1, 1), 1.0f).SetEase(Ease.OutExpo).OnComplete(() => StartCoroutine(TipsBack(tipCross)));

            int tempScore = 50;

            int indexN = index - 3;
            int indexS = index + 3;
            int indexW = index - 1;
            int indexE = index + 1;

            if (indexN > 0)
            {
                if (holes[indexN].nowLeave != null)
                {
                    holes[indexN].nowLeave.GetComponentInChildren<Character>().Boom();
                    tempScore += 50;
                }
            }
            if (indexS < countCharacter)
            {
                if (holes[indexS].nowLeave != null)
                {
                    holes[indexS].nowLeave.GetComponentInChildren<Character>().Boom();
                    tempScore += 50;
                }
            }
            if (indexW > 0)
            {
                if (holes[indexW].nowLeave != null)
                {
                    holes[indexW].nowLeave.GetComponentInChildren<Character>().Boom();
                    tempScore += 50;
                }
            }
            if (indexE < countCharacter)
            {
                if (holes[indexE].nowLeave != null)
                {
                    holes[indexE].nowLeave.GetComponentInChildren<Character>().Boom();
                    tempScore += 50;
                }
            }

            message[index].SetActive(true);
            message[index].transform.DOKill(true);
            message[index].transform.position = textIn[index].position;
            message[index].transform.DOMove(textOut[index].position, 0.37f).OnComplete(() => MsgBack(index));
            textMessage[index].text = "+" + tempScore;
            score += tempScore;
            textScore.text = "" + (int)score;
        }
    }
    void HitFaceless(object sender, HItEventArgs e)
    {
        if (Time.time > silence)
        {
            int index = e.index;
            countHitFaceless++;
            Home(index);

            tipSilence.SetActive(true);
            tipSilence.GetComponent<RectTransform>().localScale = Vector3.zero;
            tipSilence.transform.DOScale(new Vector3(1, 1, 1), 1.0f).SetEase(Ease.OutExpo).OnComplete(() => StartCoroutine(TipsBack(tipSilence)));

            message[index].SetActive(true);
            message[index].transform.DOKill(true);
            message[index].transform.position = textIn[index].position;
            message[index].transform.DOMove(textOut[index].position, 0.37f).OnComplete(() => MsgBack(index));
            textMessage[index].text = "X";
            silence = Time.time + 3;
        }
    }
    void HitWangNiMa(object sender, HItEventArgs e)
    {
        if (Time.time > silence)
        {
            int index = e.index;
            countHitWangNiMa++;

            ExplosionFX fx = poolFXHit.Reuse(holes[index].nowLeave.position, Quaternion.identity).GetComponent<ExplosionFX>();
            StartCoroutine(fx.Audio(0));
            Home(index);

            message[index].SetActive(true);
            message[index].transform.DOKill(true);
            message[index].transform.position = textIn[index].position;
            message[index].transform.DOMove(textOut[index].position, 0.37f).OnComplete(() => MsgBack(index));
            textMessage[index].text = "Nice";
        }
    }
    void HitMengZong(object sender, HItEventArgs e)
    {
        if (Time.time > silence)
        {
            int index = e.index;
            countHitMengZong++;

            ExplosionFX fx = poolFXExplosion.Reuse(holes[index].nowLeave.position, Quaternion.identity).GetComponent<ExplosionFX>();
            StartCoroutine(fx.Audio(11));
            Home(index);

            tipBoom.SetActive(true);
            tipBoom.GetComponent<RectTransform>().localScale = Vector3.zero;
            tipBoom.transform.DOScale(new Vector3(1, 1, 1), 0.73f).SetEase(Ease.OutElastic).OnComplete(() => StartCoroutine(TipsBack(tipBoom)));

            int tempScore = 200;

            for (int i = 0; i < 9; i++)
            {
                if (holes[i].nowLeave != null)
                {
                    holes[i].nowLeave.GetComponentInChildren<Character>().Boom();
                    tempScore += 100;
                }
            }

            message[index].SetActive(true);
            message[index].transform.DOKill(true);
            message[index].transform.position = textIn[index].position;
            message[index].transform.DOMove(textOut[index].position, 0.37f).OnComplete(() => MsgBack(index));
            textMessage[index].text = "+" + tempScore;
            score += tempScore;
            textScore.text = "" + (int)score;
        }
    }
    void HitHuaJi(object sender, HItEventArgs e)
    {
        if (Time.time > silence)
        {
            int index = e.index;
            //countHitMengZong++;

            ExplosionFX fx = poolFXHit.Reuse(holes[index].nowLeave.position, Quaternion.identity).GetComponent<ExplosionFX>();
            StartCoroutine(fx.Audio(0));
            Home(index);

            message[index].SetActive(true);
            message[index].transform.DOKill(true);
            message[index].transform.position = textIn[index].position;
            message[index].transform.DOMove(textOut[index].position, 0.37f).OnComplete(() => MsgBack(index));
        }
    }

}
