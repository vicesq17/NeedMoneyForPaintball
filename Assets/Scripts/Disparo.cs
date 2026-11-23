
using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject prefabBola;
    public Transform puntoDisparo;
    public float velocidad = 20f;

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (prefabBola == null || puntoDisparo == null)
                return;

            GameObject bola = Instantiate(
                prefabBola,
                puntoDisparo.position,
                puntoDisparo.rotation
            );

            Rigidbody rb = bola.GetComponent<Rigidbody>();

            if (rb != null)
                rb.linearVelocity =
                    puntoDisparo.forward * velocidad;

            Destroy(bola, 2f);
        }
    }
}
