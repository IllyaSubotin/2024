using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CubeSpawn : MonoBehaviour
{    
    private CubeControler PlayersCubeControllerComponent;
    private Touch touch;
    private bool isCubeSpawning = true;
    private float spawnInterval = 0.5f;
    private int i = 0;
    private int cubeCount = 0;


    public GameObject PlayersCube;
    public Vector3 startCubePosition = new Vector3(0f, 0.5f, -10f);
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI cubeCountText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public CameraControler cameraControler;
    public GameObject[] cubePrefab;
    public int scoreCount = 0;


    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(SpawnObjectWithDelay(0.1f)); // Create first cube without interval
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                if (isCubeSpawning)
                {
                    isCubeSpawning = false;

                    if (cubeCount > 50) // When cube count = 50, GameOver 
                    {
                        gameOverText.gameObject.SetActive(true);
                        restartButton.gameObject.SetActive(true);

                        finalScoreText.text = ("Yuor score " + scoreCount);
                        finalScoreText.gameObject.SetActive(true);

                        cubeCountText.gameObject.SetActive(false);
                        scoreText.gameObject.SetActive(false);

                        Time.timeScale = 0f;

                        restartButton.onClick.AddListener(() =>    // on button click get this scene name and reload 
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        });
                    }

                    StartCoroutine(SpawnObjectWithDelay(spawnInterval)); // Create next cube after TouchPhase.Ended with some interval 

                }
            }
        }
    }

    IEnumerator SpawnObjectWithDelay(float spawnInterval) // Create cube4 with 25% chance or cube2 with other 75% with some interval
    {
        yield return new WaitForSeconds(spawnInterval);
               
        if (4 == Random.Range(1, 5))
        {
            PlayersCube = Instantiate(cubePrefab[1], startCubePosition, Quaternion.identity);
            PlayersCubeControllerComponent = PlayersCube.GetComponent<CubeControler>();

            PlayersCubeControllerComponent.controler = this;
        }
        else
        {
            PlayersCube =  Instantiate(cubePrefab[0], startCubePosition, Quaternion.identity);
            PlayersCubeControllerComponent = PlayersCube.GetComponent<CubeControler>();

            PlayersCubeControllerComponent.controler = this;
        }

        var swap = PlayersCube.transform.position; 
        swap.x = cameraControler.mainCamera.transform.position.x;
        PlayersCube.transform.position = swap;

        cubeCount++;
        var temp = 50 - cubeCount;
        cubeCountText.text = ("Ñubes left: " + temp);

        isCubeSpawning = true;
    }

    public void MargeCube(Collision collision, int cubeNum, int scoreForCube, GameObject cube, CubeControler CubeControler) // Marge two cube in one
    {
        i++;

        if (i % 2 == 0) 
        {
            Vector3 mergePosition = (CubeControler.transform.position + collision.transform.position) / 2;

            GameObject mergeCube = Instantiate(cubePrefab[cubeNum], mergePosition, Quaternion.identity);
            CubeControler mergeCubeControler = mergeCube.GetComponent<CubeControler>();

            mergeCubeControler.rb.AddForce((CubeControler.rb.velocity + collision.rigidbody.velocity)/2, ForceMode.Impulse); // Save impulse in merge cube

            scoreCount += scoreForCube;
            scoreText.text = ("Score: " + scoreCount);

            mergeCubeControler.controler = this;
            mergeCubeControler.isPlayerControlled = false;

            cubeCount--;
            var temp = 50 - cubeCount;
            cubeCountText.text = ("Ñubes left: " + temp);

            Destroy(cube);
            Destroy(collision.gameObject);
        }      
    }
}

  
