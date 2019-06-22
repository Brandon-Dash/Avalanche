using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public PlayerController player;
    public GameObject box;
    public Text gameOverText;
    public Text scoreText;
    public float timeBetweenSpawns;
    public float spawnBoundary;
    public float timeSlowdownFactor;
    public float slowdownWaterRatePenalty;
    public float difficultyIncreaseRate;

    private float timer;
    private float spawnV;
    private float spawnH;
    private float lastSpawnH;
    private GameObject newBox;

    public float timeRate;
    public float difficulty;

    private void Start()
    {
        timer = 0.0f;
        spawnV = 15.0f;
        timeRate = 1.0f;
        difficulty = 1.0f;
        gameOverText.text = "";
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (player == null)
        {
            gameOverText.text = "Height: " + Mathf.RoundToInt(player.maxHeight).ToString() + "\nPress R to restart";
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }

        if (player != null)
        {
            scoreText.text = Mathf.RoundToInt(player.maxHeight).ToString();

            timer += Time.deltaTime * difficulty * timeRate;

            if (timer > timeBetweenSpawns)
            {

                spawnV += 1.0f;

                spawnH = Random.Range(-spawnBoundary, spawnBoundary);
                while (Mathf.Abs(spawnH - lastSpawnH) < 4.0f)
                {
                    spawnH = Random.Range(-spawnBoundary, spawnBoundary);
                }

                newBox = Instantiate(box, new Vector3(spawnH, spawnV), Quaternion.identity);

                newBox.transform.localScale *= Random.Range(2.0f, 4.0f);
                newBox.GetComponent<BoxMover>().fallSpeed = Random.Range(2.0f, 6.0f) * difficulty;
                newBox.GetComponent<ColorChanger>().changeColor(Random.ColorHSV());

                lastSpawnH = spawnH;
                timer = 0.0f;
            }

            difficulty += difficultyIncreaseRate * Time.deltaTime * timeRate;

            if (Input.GetButton("Fire1") && player != null && player.alive)
            {
                timeRate = timeSlowdownFactor;
            }
            else
            {
                timeRate = 1.0f;
            }
        }
    }
}
