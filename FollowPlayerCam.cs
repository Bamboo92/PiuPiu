using UnityEngine;
using System;

public class FollowPlayerCam : MonoBehaviour
{
    private Transform player1, player2;
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float smoothFactor = 0.01f;
    [SerializeField]
    private bool LookAtPlayer = true;
    [SerializeField]
    private bool forPlayer1, forPlayer2;

    [SerializeField]
    private float leftLimit;
    [SerializeField]
    private float rightLimit;
    [SerializeField]
    private float bottomLimit;
    [SerializeField]
    private float topLimit;
    
    void Start()
    {
        player1 = GameObject.Find("Player1").GetComponent<Transform>();
        player2 = GameObject.Find("Player2").GetComponent<Transform>();
    }
    
    void LateUpdate()
    {
        if (forPlayer1){
            Vector3 newPosition = player1.position + offset;

            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

            if (LookAtPlayer){
                transform.LookAt(player1);
            }
            
        } else if (forPlayer2){
            Vector3 newPosition = player2.position + offset;

            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

            if (LookAtPlayer){
                transform.LookAt(player2);
            }
        }

        transform.position = new Vector3
        (
            Math.Clamp(transform.position.x, leftLimit, rightLimit),
            transform.position.y,
            Math.Clamp(transform.position.z, bottomLimit, topLimit)
        );
    }

    private void OnDrawGizmos()
    {
        //draw a box around our camera boundary
        Gizmos.color = Color.green;

        //top boundary line
        Gizmos.DrawLine(new Vector3(leftLimit, 0, topLimit), new Vector3(rightLimit, 0, topLimit));
        //right boundary line
        Gizmos.DrawLine(new Vector3(rightLimit, 0, topLimit), new Vector3(rightLimit, 0, bottomLimit));
        //bottom boundary line
        Gizmos.DrawLine(new Vector3(rightLimit, 0, bottomLimit), new Vector3(leftLimit, 0, bottomLimit));
        //left boundary line
        Gizmos.DrawLine(new Vector3(leftLimit, 0, bottomLimit), new Vector3(leftLimit, 0, topLimit));
    }

    void OnEnable()
    {
        Game_Manager.instance.onPlayerSpawn.AddListener(FindPlayers);
    }

    void OnDisable()
    {
        Game_Manager.instance.onPlayerSpawn.RemoveListener(FindPlayers);
    }

    void FindPlayers()
    {
        GameObject player1GO = GameObject.Find("Player1");
        if (player1GO != null)
        {
            player1 = player1GO.GetComponent<Transform>();
        }

        GameObject player2GO = GameObject.Find("Player2");
        if (player2GO != null)
        {
            player2 = player2GO.GetComponent<Transform>();
        }
    } 
}
