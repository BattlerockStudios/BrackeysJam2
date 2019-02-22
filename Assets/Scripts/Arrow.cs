using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField]
    private AudioClip m_fireSound = null;

    [SerializeField]
    private AudioClip m_hitSound = null;

    [SerializeField]
    private AudioSource m_audioSource = null;

    public void Fire(Vector3 position, Vector3 direction, float speed)
    {
        transform.forward = direction;
        transform.position = position;
        GetComponent<Rigidbody>().velocity = direction * speed;
        m_audioSource.PlayOneShot(m_fireSound);
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_audioSource.PlayOneShot(m_hitSound);
        transform.SetParent(collision.transform, worldPositionStays: true);
        //transform.localScale = Vector3.one;

        var colliders = transform.GetComponentsInChildren<Collider>();
        foreach (var item in colliders)
        {
            item.enabled = false;
        }

        collision.gameObject.GetComponent<IArrowTarget>()?.OnHitByArrow(this);

        Destroy(transform.GetComponent<Rigidbody>());
        Destroy(this);
    }

}

public interface IArrowTarget
{
    void OnHitByArrow(Arrow arrow);
}