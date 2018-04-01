using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Highscore {
    public string name;
    public int score;
    public string date;

    public Highscore(string _name, int _score, string _date) {
        name = _name;
        score = _score;
        date = _date;
    }
}

public class HighscoreManager : MonoBehaviour {

    public int numHighscores = 5;
    public GameObject highscorePanel;
    public Transform highscoreParent;
    public GameObject highscorePrefab;

    public bool clear = false;

    public List<Highscore> highscores;

    private void Start() {

        highscores = new List<Highscore>();

        LoadHighscores();
    }

    private void Update() {
        if (clear) {
            ClearHighscores();
            clear = false;
        }
    }

    public void DisplayHighscores() {
        for (int i = 0; i < highscores.Count; i++) {
            GameObject curHighscore = Instantiate(highscorePrefab, highscoreParent);
            curHighscore.GetComponent<Text>().text = highscores[i].name + ": " + highscores[i].score + " - " + highscores[i].date;
        }

        highscorePanel.SetActive(true);
    }

    public void HideHighscores() {
        highscorePanel.SetActive(false);

        for (int i = 0; i < highscoreParent.childCount; i++) {
            Destroy(highscoreParent.GetChild(0));
        }
    }

    public bool IsHighscore(int checkScore) {
        if (highscores.Count < numHighscores) {
            return true;
        }

        for (int i = 0; i < highscores.Count; i++) {
            if (checkScore > highscores[i].score) {
                return true;
            }
        }
        return false;
    }

    public void AddHighscore(string newName, int newScore, string newDate) {

        Debug.Log("Adding highscore: " + newName + ": " + newScore + " - " + newDate);
        if (highscores.Count == 0) {
            highscores.Add(new Highscore(newName, newScore, newDate));
        } else if (highscores.Count < numHighscores && highscores[highscores.Count - 1].score > newScore) {
            highscores.Add(new Highscore(newName, newScore, newDate));
        } else {
            for (int i = 0; i < highscores.Count; i++) {
                if (newScore > highscores[i].score) {
                    highscores.Insert(i, new Highscore(newName, newScore, newDate));
                    while (highscores.Count > numHighscores) {
                        highscores.RemoveAt(highscores.Count - 1);
                    }
                    break;
                }
            }
        }
    }

    void LoadHighscores() {
        for (int i = 0; i < numHighscores; i++) {
            string currentNameKey = "Name" + i;
            string currentScoreKey = "Score" + i;
            string currentDateKey = "Date" + i;
            if (PlayerPrefs.HasKey(currentNameKey) && PlayerPrefs.HasKey(currentScoreKey) && PlayerPrefs.HasKey(currentDateKey)) {
                highscores.Add(new Highscore(PlayerPrefs.GetString(currentNameKey), PlayerPrefs.GetInt(currentScoreKey), PlayerPrefs.GetString(currentDateKey)));
            }
        }
    }

    public void SaveHighscores() {
        for (int i = 0; i < numHighscores; i++) {

            string currentNameKey = "Name" + i;
            string currentScoreKey = "Score" + i;
            string currentDateKey = "Date" + i;

            if (i > highscores.Count - 1) {
                PlayerPrefs.DeleteKey(currentNameKey);
                PlayerPrefs.DeleteKey(currentScoreKey);
                PlayerPrefs.DeleteKey(currentDateKey);

            } else {
                string currentNameValue = highscores[i].name;
                int currentScoreValue = highscores[i].score;
                string currentDateValue = highscores[i].date;

                PlayerPrefs.SetString(currentNameKey, currentNameValue);
                PlayerPrefs.SetInt(currentScoreKey, currentScoreValue);
                PlayerPrefs.SetString(currentDateKey, currentDateValue);
            }
        }
    }

    void ClearHighscores() {
        for (int i = 0; i < numHighscores; i++) {

            string currentNameKey = "Name" + i;
            string currentScoreKey = "Score" + i;
            string currentDateKey = "Date" + i;
            
            PlayerPrefs.DeleteKey(currentNameKey);
            PlayerPrefs.DeleteKey(currentScoreKey);
            PlayerPrefs.DeleteKey(currentDateKey);
        }
        highscores.Clear();
    }
    
    private void OnApplicationQuit() {
        SaveHighscores();
    }

    private void OnApplicationPause(bool pause) {
        if (pause) {
            SaveHighscores();
        }
    }
}
