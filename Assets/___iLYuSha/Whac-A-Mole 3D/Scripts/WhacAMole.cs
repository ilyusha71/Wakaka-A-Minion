using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public partial class WhacAMole : MonoBehaviour
{
    [Header("倒計時")]
    public GameObject blockCountdown;
    public AudioClip[] clips;
    public Text textCountdown;



    public GameObject playButton;
    public GameObject restartButton;
    public GameObject ready;
    public GameObject final;
    public GameObject textVictory;
    public GameObject textGameover;
    public Text textHitMole;
    public Text textHitSponge;
    public Text textHitFaceless;
    public Text textHitDorara;
    public Text textHitMengZong;
    public Text textHitWangNiMa;

    public GameObject tipCross;
    public GameObject tipSilence;
    public Text textTipSilence;
    public GameObject tipBoom;
    public GameObject tipNiMa;
    public Text textNiMa;
    public GameObject[] message;
    public Text[] textMessage;
    public Transform[] textIn;
    public Transform[] textOut;

    public GameObject[] holeMinion;
    public GameObject[] holeMole;
    public GameObject[] holeSponge;
    public GameObject[] holeFaceless;
    public GameObject[] holeDorara;
    public GameObject[] holeMengZong;
    public GameObject[] holeWangNiMa;

    private bool openWhacAMole;
    private int rand;
    private float delay;
    [Header("Game Parameter")]
    public float gameTime;
    [Header("Game Statistics")]
    public Text textScore;
    public Text textCapture;
    public Image timerBar;
    private float score;
    private int capture;
    private float countdownTimer;
    private bool victory;    
    private int countHitSponge;
    private int countHitMole;
    private int countHitDorara;
    private int countHitFaceless;
    private int countHitWangNiMa;
    private int countHitMengZong;
    private float silence;
    private int numWangNiMa;

    void Awake()
    {
        InitializeCharacter();
        InitializeHoles();
        InitializeLevel();
    }

    void OnEnable()
    {
        playButton.SetActive(true);
        restartButton.SetActive(false);
        openWhacAMole = false;

        score = 0;
        capture = 0;
        countdownTimer = gameTime;
        textScore.text = "---";
        textCapture.text = "" + capture;
        timerBar.fillAmount = countdownTimer / gameTime;
    }

    public void Play()
    {
        StartCoroutine(Countdown());
    }

    public void Restart()
    {
        restartButton.SetActive(false);
        openWhacAMole = false;

        score = 0;
        capture = 0;
        countdownTimer = gameTime;
        textScore.text = "---";
        textCapture.text = "" + capture;
        timerBar.fillAmount = countdownTimer / gameTime;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        blockCountdown.SetActive(true);
        AudioSource audioCount = blockCountdown.GetComponent<AudioSource>();
        //audioCount.clip = clips[0];
        //textCountdown.text = "READY";
        //yield return new WaitForSeconds(0.73f);
        //audioCount.Play();
        //textCountdown.text = "3";
        //yield return new WaitForSeconds(1.0f);
        //audioCount.Play();
        //textCountdown.text = "2";
        //yield return new WaitForSeconds(1.0f);
        //audioCount.Play();
        textCountdown.text = "1";
        yield return new WaitForSeconds(1.0f);
        audioCount.clip = clips[1];
        audioCount.Play();
        textCountdown.text = "GO！";
        openWhacAMole = true;
        yield return new WaitForSeconds(0.73f);
        blockCountdown.SetActive(false);
    }

    void OnDisable()
    {
        tipCross.SetActive(false);
        tipSilence.SetActive(false);
        tipBoom.SetActive(false);
        tipNiMa.SetActive(false);
        openWhacAMole = false;
    }

    void Update()
    {
        this.transform.rotation *= Quaternion.Euler(0, Time.deltaTime*-1.37f, 0);
        if (openWhacAMole)
            OpenWhacAMole();
    }

    void OpenWhacAMole()
    {
        countdownTimer -= Time.deltaTime;
        timerBar.fillAmount = countdownTimer / gameTime;

        if (Time.time > delay)
        {
            if (capture > 1750)
            {
                // Level 6
                magnitudeLevel = 22;
                rand = Random.Range(0, 4);
                delay = Time.time + rand * 0.1f;
                rand = Random.Range(0, 9);
            }
            else if (capture > 1300)
            {
                // Level 5
                magnitudeLevel = 22;
                rand = Random.Range(0, 4);
                delay = Time.time + rand * 0.125f;
                rand = Random.Range(0, 9);
            }
            else if (capture > 900)
            {
                // Level 4
                magnitudeLevel = 19;
                rand = Random.Range(0, 4);
                delay = Time.time + rand * 0.137f;
                rand = Random.Range(0, 9);
            }
            else if (capture > 0)
                EnterLevel3();
            else if (capture > 0)
                EnterLevel2();
            else if (capture >= 0)
                EnterLevel1();
            Leaving(Random.Range(0, 9));
        }

        if (silence > Time.time)
        {
            textTipSilence.text = "沉默" + (int)(silence - Time.time + 1);
        }
        if (numWangNiMa > 0)
        {
            tipNiMa.SetActive(true);
            textNiMa.text = "-" + numWangNiMa * 100 + "/sec";
            score -= (100 * numWangNiMa * Time.deltaTime);
            textScore.text = "" + (int)score;
        }
        else
            tipNiMa.SetActive(false);

        /* Last */
        if (capture > 200)
        {
            // Victory
            victory = true;
        }
        if (countdownTimer < 0)
        {
            openWhacAMole = false;
            countdownTimer = 0;
            timerBar.fillAmount = countdownTimer / gameTime;
            restartButton.SetActive(true);
            tipCross.SetActive(false);
            tipSilence.SetActive(false);
            tipBoom.SetActive(false);
            tipNiMa.SetActive(false);

            // Final
            final.SetActive(true);
            if (victory)
            {
                textVictory.SetActive(true);
                textGameover.SetActive(false);
            }
            else
            {
                textVictory.SetActive(false);
                textGameover.SetActive(true);
            }
            textHitSponge.text = "" + countHitSponge;
            textHitMole.text = "" + countHitMole;
            textHitDorara.text = "" + countHitDorara;
            textHitFaceless.text = "" + countHitFaceless;
            textHitMengZong.text = "" + countHitMengZong;
            textHitWangNiMa.text = "" + countHitWangNiMa;
        }
    }

    #region Message
    IEnumerator TipsBack(GameObject tips)
    {
        if (tips == tipSilence)
        {
            yield return new WaitForSeconds(2.0f);
            tips.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(0.001f);
            tips.SetActive(false);
        }
    }
    void MsgBack(int order)
    {
        message[order].SetActive(false);
    }
    #endregion
}
