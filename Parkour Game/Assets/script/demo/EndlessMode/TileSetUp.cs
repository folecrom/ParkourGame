using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetUp : MonoBehaviour
{
    //Reference
    public EndlessLevelManager elm;
    public Transform playerObj;

    //Prefabs
    public GameObject[] wallPrefabs;
    public GameObject[] platformPrefabs;
    public GameObject[] handmadePrefabs;
    public Material[] wallMaterials, wallEmissiveMaterials;
    public Material[] platformMaterials;

    //Amount of Objects to spawn
    public int minWallAmount, maxWallAmount;
    public int minPlatformAmount, maxPlatformAmount;
    public int minHandmadeAmount, maxHandmadeAmount;
    int wallAmount, platformAmount, handmadeAmount;

    //Where to spawn
    public LayerMask whatIsObject;
    public float avoidObjectsRadius, handmadeRadiusMultiplier;
    public float pullPlatformsDown, pushWallsUp;
    public float decreaseXExtents, decreaseYExtents;
    public float xExtents, yExtents, zExtents;

    void Start()
    {
        elm = GameObject.Find("EndlessLevelManager").GetComponent<EndlessLevelManager>();
        playerObj = GameObject.Find("PlayerObj").transform;

        wallAmount = Random.Range(minWallAmount, maxWallAmount + 1);
        platformAmount = Random.Range(minPlatformAmount, maxPlatformAmount + 1);
        handmadeAmount = Random.Range(minHandmadeAmount, maxHandmadeAmount + 1);

        //Reduce other stuff when handmade spawns
        if(handmadeAmount == 1)
        {
            wallAmount--;
            platformAmount--;
        }

        //Set x and y Extents to spawn
        xExtents = GetComponent<MeshCollider>().bounds.extents.x - decreaseXExtents;
        yExtents = GetComponent<MeshCollider>().bounds.extents.y - decreaseYExtents;
        zExtents = GetComponent<MeshCollider>().bounds.extents.z;

        //Start spawning
        SpawnWalls();
        SpawnPlatforms();
        SpawnHandmade();
    }

    private bool deactivated;
    private void Update()
    {
        //count down ground time in elm
        if (playerObj.position.y < -(yExtents + decreaseYExtents) + 1.55f)
        {
            elm.ShowGroundTime();
            deactivated = false;
        }
        else if (elm.groundTimeActive && !deactivated)
        {
            elm.HideGroundTime();
            deactivated = true;
        }
    }

    private void SpawnWalls()
    {
        for (int i = 0; i < wallAmount; i++)
        {
            //Search the best position
            Vector3 randomVector = randomExtents();
            int counter = 0;
            while (Physics.CheckSphere(randomVector, avoidObjectsRadius, whatIsObject) && counter < 77)
            {
                Debug.Log("TooClose");
                randomVector = randomExtents();

                counter++;

                if (counter >= 77) Debug.Log("Crash Warning Dave...!");
            }

            //generally pull platforms down (if not already on the ground)
            if (randomVector.y < (yExtents + decreaseYExtents) - pushWallsUp)
                randomVector.y = randomVector.y + pushWallsUp;

            GameObject currWall = Instantiate(wallPrefabs[Random.Range(0,wallPrefabs.Length)], randomVector, Quaternion.Euler(0, -90, 0), transform);
            //Assign material
            List<Material> temp = new List<Material>();
            temp.Add(wallMaterials[Random.Range(0, wallMaterials.Length)]);
            temp.Add(wallEmissiveMaterials[Random.Range(0, wallEmissiveMaterials.Length)]);

            currWall.GetComponent<MeshRenderer>().materials = temp.ToArray();

            //currWall.GetComponent<MeshRenderer>().material = wallMaterials[Random.Range(0, wallMaterials.Length)];
            //currWall.GetComponent<MeshRenderer>().materials[1] = wallEmissiveMaterials[Random.Range(0, wallEmissiveMaterials.Length)];
        }
    }

    private void SpawnPlatforms()
    {
        for (int i = 0; i < platformAmount; i++)
        {
            //Search the best position
            Vector3 randomVector = randomExtents();
            int counter = 0;
            while (Physics.CheckSphere(randomVector, avoidObjectsRadius, whatIsObject) && counter < 77)
            {
                Debug.Log("TooClose");
                randomVector = randomExtents();

                counter++;

                if (counter >= 77) Debug.Log("Crash Warning Dave...!");
            }

            //generally pull platforms down (if not already on the ground)
            if (randomVector.y > -(yExtents+decreaseYExtents) + pullPlatformsDown)
            randomVector.y = randomVector.y - pullPlatformsDown;

            GameObject currPlatform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], randomVector, Quaternion.Euler(-90, 0, 0), transform);
            //Assign material
            currPlatform.GetComponent<MeshRenderer>().material = platformMaterials[Random.Range(0, platformMaterials.Length)];
        }
    }

    private void SpawnHandmade()
    {
        for (int i = 0; i < handmadeAmount; i++)
        {
            ///int randomMaterial = Random.Range(0, platformMaterials.Length + 1);

            //Search the best position
            Vector3 randomVector = randomExtents();
            //Drag it more to the middle
            randomVector.y -= Mathf.Sign(randomVector.y) * 2;

            int counter = 0;
            while (Physics.CheckSphere(randomVector, avoidObjectsRadius * handmadeRadiusMultiplier, whatIsObject) && counter < 77)
            {
                Debug.Log("TooClose");
                randomVector = randomExtents();
                randomVector.y -= Mathf.Sign(randomVector.y) * 2;

                counter++;

                if (counter >= 77) Debug.Log("Crash Warning Dave...!");
            }

            GameObject currHandmade = Instantiate(handmadePrefabs[Random.Range(0, handmadePrefabs.Length)], randomVector, Quaternion.identity, transform);
            //Assign material
            //currPlatform.GetComponent<MeshRenderer>().material = platformMaterials[randomMaterial];
        }
    }

    private Vector3 randomExtents()
    {
        Debug.Log("Generated");

        //Calculate random Vector
        float x = Random.Range(-xExtents, xExtents) + transform.position.x;
        float y = Random.Range(-yExtents, yExtents) + transform.position.y;
        float z = Random.Range(-zExtents, zExtents) + transform.position.z;
        return new Vector3(x, y, z);
    }
}
