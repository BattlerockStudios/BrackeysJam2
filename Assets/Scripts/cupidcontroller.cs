using System;
using UnityEngine;

public class cupidcontroller : MonoBehaviour
{

    [SerializeField]
    private Arrow m_arrowPrefab = null;

    [SerializeField]
    private Camera m_camera = null;

    private DateTime? m_timeOfLastFire = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.ReadyToStart == true)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            Instantiate(m_arrowPrefab).Fire(transform.position, ray.direction, 50f);

            m_timeOfLastFire = DateTime.UtcNow;
        }

        const float FLASH_DURATION_SECONDS = .15f;
        var secondsSinceLastShot = m_timeOfLastFire.HasValue ? (float)(DateTime.UtcNow - m_timeOfLastFire.Value).TotalSeconds : FLASH_DURATION_SECONDS;
        var progress = Mathf.Clamp01(secondsSinceLastShot / FLASH_DURATION_SECONDS);
        GameManager.Instance.SetBlindfoldOpacity(Mathf.Lerp(.25f, 0f, progress));
    }
}
