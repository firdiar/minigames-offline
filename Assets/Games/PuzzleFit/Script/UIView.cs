using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BoardFit
{
    public class UIView : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI highscore;
        [SerializeField]
        TextMeshProUGUI nowScore;
        [SerializeField]
        Button restartButton;
        [SerializeField]
        GameObject gameOver;

        private void Start()
        {
            highscore.text = PlayerPrefs.GetInt("BoardFit-Highscore", 0).ToString();
            nowScore.text = "0";
            restartButton.onClick.AddListener(Restart);
            gameOver.SetActive(false);
            Debug.Log("Highscore: " + PlayerPrefs.GetInt("BoardFit-Highscore", 0));
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ShowGameOver()
        {
            var nowScoreValue = int.Parse(nowScore.text);
            if (nowScoreValue > PlayerPrefs.GetInt("BoardFit-Highscore", 0))
            {
                PlayerPrefs.SetInt("BoardFit-Highscore", nowScoreValue);
            }

            //do some ui animation
            gameOver.SetActive(true);
        }

        public void SetScore(int score)
        {
            nowScore.text = score.ToString();
        }
    }
}