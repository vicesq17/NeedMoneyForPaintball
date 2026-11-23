
using UnityEngine;

public class Apuntado : MonoBehaviour
{
    public float sensibilidad = 2f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Start()
    {
        SincronizarOrientacion();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SincronizarOrientacion()
    {
        Vector3 angulos = transform.eulerAngles;

        rotacionX = angulos.x;

        if (rotacionX > 180f)
            rotacionX -= 360f;

        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);
        rotacionY = angulos.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // F recupera el control del raton.
        if (Input.GetKeyDown(KeyCode.F))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float mouseX =
            Input.GetAxis("Mouse X") * sensibilidad;

        float mouseY =
            Input.GetAxis("Mouse Y") * sensibilidad;

        rotacionY += mouseX;
        rotacionX -= mouseY;

        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);

        transform.rotation = Quaternion.Euler(
            rotacionX,
            rotacionY,
            0f
        );
    }
}
