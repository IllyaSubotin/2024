using UnityEngine;

public class CameraControler : MonoBehaviour
{
    private Vector3 startTouchPosition;
    private Vector3 currentTouchPosition;
    private Vector3 mouseDelta;
    private Touch touch;
    private float moveDirection;
    private float speed = 1f;

    public GameObject mainCamera;
    public CubeSpawn cubeSpawn;

    void Update()
    {
        if (Input.touchCount > 0) // Move camera 
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                currentTouchPosition = touch.position; 
                mouseDelta = currentTouchPosition - startTouchPosition;
                startTouchPosition = currentTouchPosition;
                moveDirection = -mouseDelta.x * speed * Time.deltaTime;

                if (mainCamera.transform.position.x + moveDirection > -4 && mainCamera.transform.position.x + moveDirection < 4) // Ñhecking exit from the field
                {                    
                    mainCamera.transform.Translate(moveDirection, 0, 0);
                    cubeSpawn.startCubePosition.x += moveDirection;
                }
            }
           

        }

        
    }
}
