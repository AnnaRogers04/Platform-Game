using UnityEngine;
using UnityEngine.SceneManagement;
public class WinCollision : MonoBehaviour
{    
    public GameObject objectToDetect;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject == objectToDetect)
        {
            SceneManager.LoadScene(2);
            Debug.Log("Collision detected with the specified object: " + objectToDetect.name);

        }
    }
}