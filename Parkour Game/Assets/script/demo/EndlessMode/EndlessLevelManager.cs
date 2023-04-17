using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using System;

public class EndlessLevelManager : MonoBehaviour
{
    public bool gameOver;
    public GameObject gameOverScreen;

    public int tileAmount;

    public Transform player;
    public float playerZ, elapsedZ = 0;
    private float startZ = 0, recordZ;

    public int wallTilesSpawned;
    public GameObject[] wallTilePrefabs;
    public List<GameObject> wallTiles;

    //Limit walking on ground
    public float groundTime, groundTimeMultiplier = 1f;
    [HideInInspector]
    public bool groundTimeActive;
    public TextMeshProUGUI groundTimeText, scoreText, scoreGameOverText;
    public Animator groundTimeAnimator;

    //Chasing border
    public Transform chasingBorder;
    public float startBorderSpeed, maxBorderSpeed;
    float currBorderSpeed;

    private void Start()
    {
        //Setup first tiles
        for (int i = 0; i < tileAmount; i++)
        {
            ChooseNextTile();
        }

        player = GameObject.Find("PlayerObj").transform;
        startZ = player.position.z;
    }

    float timer;
    void Update()
    {
        playerZ = player.position.z;

        //calculate elapsedZ
        elapsedZ = playerZ - startZ;
        //Set record
        if (playerZ - 20 > recordZ) recordZ = playerZ - 20;

        //Set tile
        if (elapsedZ > 40)
        {
            startZ = playerZ; //Reset startZ
            ChooseNextTile();
            DeleteLastTile();
        }

        groundTimeAnimator.SetBool("active", groundTimeActive);

        //Set score
        scoreText.SetText("Score: " + Mathf.Round(recordZ));

        //Increase ground time lost over time
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            timer = 0;
            groundTimeMultiplier += 0.15f;
        }

        if (gameOver)
        {
            gameOverScreen.SetActive(true);
            player.GetComponent<playerMovement>().enabled = false;
            groundTimeText.enabled = false;
            scoreGameOverText.SetText("Score: " + Mathf.Round(recordZ));

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        MoveBorder();
    }

    int lastTileIndex;
    int sameTileCombo;
    private void ChooseNextTile()
    {
        //Make a 50% chance of keeping the last tile
        if (Random.Range(0, sameTileCombo + 2) == 0)
            sameTileCombo++;
        else
        {
            lastTileIndex = Random.Range(0, wallTilePrefabs.Length);
            sameTileCombo = 1;
        }

        SetNewTile();
    }
    private void SetNewTile()
    {
        wallTiles.Add(Instantiate(wallTilePrefabs[lastTileIndex], new Vector3(0, 0, wallTilesSpawned * 40), Quaternion.identity));
        wallTilesSpawned++;

        //increase border speed
        if (startBorderSpeed <= maxBorderSpeed) startBorderSpeed += 0.2f;
    }
    private void DeleteLastTile()
    {
        Destroy(wallTiles[0]);
        wallTiles.RemoveAt(0);
    }

    public void ShowGroundTime(float multiplier = 1)
    {
        if (gameOver) return;

        groundTimeActive = true;
        groundTimeText.enabled = true;

        groundTime -= Time.deltaTime * groundTimeMultiplier * multiplier;

        groundTimeText.SetText("" + Math.Round(groundTime, 1));

        //Game over
        if (groundTime <= 0)
            gameOver = true;
    }
    public void HideGroundTime()
    {
        groundTimeActive = false;
    }

    private void MoveBorder()
    {
        chasingBorder.Translate(Vector3.down * currBorderSpeed * Time.deltaTime);

        if (chasingBorder.position.z < playerZ - 30) currBorderSpeed = startBorderSpeed * 2;
        else currBorderSpeed = startBorderSpeed;

        //Punish player if behind border
        if (playerZ < chasingBorder.position.z) ShowGroundTime(3f);
    }
}
