using System.Collections;
using UnityEngine;

public class DestroyAt : MonoBehaviour
{
    public float coolDown;

    private void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(coolDown);
        Destroy(gameObject);
    }
}
