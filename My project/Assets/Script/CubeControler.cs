using System.Collections.Generic;
using UnityEngine;

public class CubeControler : MonoBehaviour
{
    public CubeSpawn controler;
    public Vector3 impulseDirection = new Vector3(0f, 0f, 1f);
    public bool isPlayerControlled = true;
    public Rigidbody rb;


    private float speed = 1f;
    private float minImpulse = 0.5f;
    private float impulseStrength = 15f;
    private float moveDirection;
    private Touch touch;
    private Vector3 startTouchPosition;
    private Vector3 currentTouchPosition;
    private Vector3 mouseDelta;

    void Update()
    {
        if(isPlayerControlled) // move players cube 
        {
            if (Input.touchCount > 0) 
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    startTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    currentTouchPosition = touch.position; 
                    mouseDelta = currentTouchPosition - startTouchPosition;
                    startTouchPosition = currentTouchPosition;
                    moveDirection = -mouseDelta.x * speed * Time.deltaTime;

                    if (transform.position.x + moveDirection > -4 && transform.position.x + moveDirection < 4)  // Ñhecking exit from the field
                    {
                        transform.Translate(moveDirection, 0, 0);
                    }

                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isPlayerControlled = false;
                    rb.AddForce(impulseDirection.normalized * impulseStrength, ForceMode.Impulse); // add impulse players cube
                }
            }        
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("2024")) // Collision check
        {
            
            if (gameObject.name == collision.gameObject.name) 
            {
                
                if(collision.impulse.magnitude > minImpulse)
                {
                    var (cubeNum, score) = CubeNumberCheck(); // Check cubes number through dictionary

                    controler.MargeCube(collision, cubeNum, score, gameObject, this); // Marge two cube in one 
                }               
                
            }
        }        
    }
        
    private (int, int) CubeNumberCheck()
    {
        Dictionary<string, (int, int)> cubeValues = new Dictionary<string, (int, int)>
        {
            { "Cube2(Clone)", (1, 1) },
            { "Cube4(Clone)", (2, 2) },
            { "Cube8(Clone)", (3, 4) },
            { "Cube16(Clone)", (4, 8) },
            { "Cube32(Clone)", (5, 16) },
            { "Cube64(Clone)", (6, 32) },
            { "Cube128(Clone)", (7, 64) },
            { "Cube256(Clone)", (8, 128) },
            { "Cube512(Clone)", (9, 256) },
            { "Cube1024(Clone)", (10, 512) }
        };

        if (cubeValues.TryGetValue(gameObject.name, out (int, int) values))
        {
            return values;
        }
        
        return (0, 0);
    }
}
