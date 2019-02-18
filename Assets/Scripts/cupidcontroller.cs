using UnityEngine;

public class cupidcontroller : MonoBehaviour
{

    [SerializeField]
    private GameObject m_arrowPrefab = null;

    [SerializeField]
    private Camera m_camera = null;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.ReadyToStart == true)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            var copy = Instantiate(m_arrowPrefab);
            copy.transform.forward = ray.direction;
            copy.transform.position = transform.position;
            copy.GetComponent<Rigidbody>().velocity = ray.direction * 50;
        }
    }
}
