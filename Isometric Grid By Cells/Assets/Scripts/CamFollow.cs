using UnityEngine;

public class CamFollow : MonoBehaviour
{
    GameObject playerPref;
    public float timeOffset;
    public Vector3 posOffset;

    private Vector3 velocity;

    TestPlayer playerScript;

    private void Start()
    {
        playerScript = FindObjectOfType<TestPlayer>();
        playerPref = FindObjectOfType<TestPlayer>().gameObject;
    }
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (playerPref != null && playerScript != null)
        {
            if (playerScript.alive)
            {
                transform.position = Vector3.SmoothDamp(transform.position, playerPref.transform.position + posOffset, ref velocity, timeOffset);
            }
        }
        else
        {
            try
            {
                playerScript = FindObjectOfType<TestPlayer>();
                playerPref = FindObjectOfType<TestPlayer>().gameObject;
            }
            catch (System.Exception) { }
        }
    }
}
