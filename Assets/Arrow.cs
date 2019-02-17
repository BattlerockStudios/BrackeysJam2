using UnityEngine;

public class Arrow : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        transform.GetComponent<Rigidbody>().isKinematic = true;

        transform.SetParent(collision.transform, worldPositionStays: true);
        transform.localScale = Vector3.one;
        enabled = false;
    }

}
