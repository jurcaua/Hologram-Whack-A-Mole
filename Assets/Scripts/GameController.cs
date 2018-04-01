using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour {

    public bool debugMode = true;
    public Transform[] debugMoles;
    public Transform debugBulletsParent;

    public Camera ARCamera;
    public GameObject projectile;
    public VuforiaTracker tracker;
    public Transform bulletsParent;
    public TextDebug textDebug;

    public Transform[] moles;
    public float startSpeed = 7.5f;
    public float maxSpeed = 30f;
    public float startDelay = 0.5f;
    public float hideSpeed = 30f;

    private float betweenDelay;
    private float moleSpeed;

    private int currentMole;
    public bool hide = false;
    private bool readyToRestart = false;
    private float timeCanRestart = 0;

    public TextMeshProUGUI scoreText;
    private int score = 0;
    public HighscoreManager highscoreManager;
    public GameObject enterNameHighscorePanel;

    // Use this for initialization
    void Start () {
        if (Application.isMobilePlatform) {
            debugMode = false;
            foreach (Transform t in debugMoles) {
                t.gameObject.SetActive(false);
            }
        }

        betweenDelay = startDelay;
        moleSpeed = startSpeed;

        enterNameHighscorePanel.SetActive(false);
        AddScore(0);

        RandomPopOut();
    }

    public void GameOver() {
        if (highscoreManager.IsHighscore(score)) {
            enterNameHighscorePanel.SetActive(true);
        } else {
            highscoreManager.DisplayHighscores();
            readyToRestart = true;
            timeCanRestart = Time.time + 2f;
        }
    }

    public void SetHighscore(string name) {
        enterNameHighscorePanel.SetActive(false);

        string date = System.DateTime.Now.ToShortDateString();
        highscoreManager.AddHighscore(name, score, date);
        highscoreManager.DisplayHighscores();
        readyToRestart = true;
        timeCanRestart = Time.time + 2f;
    }

    void RandomPopOut() {
        StartCoroutine(PopOut(Random.Range(0, 3)));
    }
	
    public void AddScore(int pointValue) {
        score += pointValue;
        scoreText.text = "Score: " + score;
    }

    void IncreaseDifficulty() {
        moleSpeed = Mathf.Clamp(moleSpeed + 2.5f, startSpeed, maxSpeed);
        betweenDelay = Mathf.Clamp(betweenDelay - 0.1f, 0f, startDelay);
    }

    // Update is called once per frame
    void Update () {
        if (tracker.TrackingFound() || debugMode) {
            bool fire = false;
            Ray shootRay = new Ray();
            if (Application.isMobilePlatform) {
                if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
                    shootRay = new Ray(ARCamera.transform.position, ARCamera.transform.forward);
                    fire = true;
                }
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    shootRay = new Ray(ARCamera.ScreenToWorldPoint(Input.mousePosition), ARCamera.transform.forward);
                    fire = true;
                }
            }
            if (fire) {
                ShootBullet();
            }
        }

        for (int i = 0; i < 3; i++) {
            if (i != currentMole) {
                moles[i].gameObject.SetActive(false);
                if (debugMode) {
                    debugMoles[i].gameObject.SetActive(false);
                }
            }
        }

        if (Application.isEditor) {
            if (Input.GetKey(KeyCode.Space)) {
                AddScore(1);
            }
        }

        if (readyToRestart && Time.time > timeCanRestart) {
            if (Input.anyKey || Input.touchCount > 0) {
                highscoreManager.SaveHighscores();
                SceneManager.LoadScene("title");
            }
        }
    }

    void ShootBullet() {
        Vector3 newScale = new Vector3(0.01f, 0.01f, 0.01f);

        GameObject newBullet = Instantiate(projectile, ARCamera.transform.position, ARCamera.transform.rotation);
        //newBullet.transform.position = ARCamera.transform.position;
        newBullet.transform.localScale = newScale;
        //newBullet.GetComponent<Bullet>().transform.localRotation.SetLookRotation(ARCamera.transform.forward);

        newBullet.transform.parent = bulletsParent;

        textDebug.shooting = true;
        if (debugMode) {
            GameObject debugBullet = Instantiate(projectile, ARCamera.transform.position, ARCamera.transform.rotation);
            debugBullet.GetComponent<Bullet>().SetIsDebug(true);
            //debugBullet.transform.position = ARCamera.transform.position;
            debugBullet.transform.localScale = newScale;
            //debugBullet.GetComponent<Bullet>().transform.localRotation.SetLookRotation(ARCamera.transform.forward);

            debugBullet.transform.parent = debugBulletsParent;
        }
    }

    IEnumerator FlashMole(int moleIndex) {
        Material material = moles[moleIndex].Find("Cone").GetComponent<Renderer>().material;
        Material debugMaterial = debugMoles[moleIndex].Find("Cone").GetComponent<Renderer>().material;
        Color startColor = material.color;
        while (true) {
            if (tracker.TrackingFound() || debugMode) {
                float emission = Mathf.PingPong(Time.time, 1f);
                Color baseColor = startColor; //Replace this with whatever you want for your base color at emission level '1'

                Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

                material.SetColor("_EmissionColor", finalColor);
                if (debugMode) {
                    debugMaterial.SetColor("_EmissionColor", finalColor);
                }
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    IEnumerator PopOut(int moleIndex) {
        currentMole = moleIndex;
        moles[moleIndex].gameObject.SetActive(true);
        if (debugMode) {
            debugMoles[moleIndex].gameObject.SetActive(true);
        }
        Coroutine flashing = StartCoroutine(FlashMole(moleIndex));
        float startY = moles[moleIndex].localPosition.y;
        while (moles[moleIndex].localPosition.y < 0f) {
            if (tracker.TrackingFound() || debugMode) {
                moles[moleIndex].localPosition += new Vector3(0f, moleSpeed * Time.fixedDeltaTime, 0f);
                if (debugMode) {
                    debugMoles[moleIndex].localPosition += new Vector3(0f, moleSpeed * Time.fixedDeltaTime, 0f);
                }
                if (hide) {
                    break;
                }
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        while (moles[moleIndex].localPosition.y > startY) {
            if (tracker.TrackingFound() || debugMode) {
                if (hide) {
                    moles[moleIndex].localPosition -= new Vector3(0f, hideSpeed * Time.fixedDeltaTime, 0f);
                } else {
                    moles[moleIndex].localPosition -= new Vector3(0f, moleSpeed * Time.fixedDeltaTime, 0f);
                }
                if (debugMode) {
                    debugMoles[moleIndex].localPosition -= new Vector3(0f, moleSpeed * Time.fixedDeltaTime, 0f);
                }
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        hide = false;
        StopCoroutine(flashing);
        moles[moleIndex].gameObject.SetActive(false);
        if (debugMode) {
            debugMoles[moleIndex].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(betweenDelay);

        IncreaseDifficulty();
        RandomPopOut();
    }
}
