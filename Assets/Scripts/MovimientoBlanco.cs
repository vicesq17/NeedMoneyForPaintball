
using UnityEngine;

public class MovimientoBlanco : MonoBehaviour
{
    public float velocidad = 1.5f;
    public float alcance = 1.5f;

    private Vector3 origen;

    void Start()
    {
        origen = transform.position;
    }

    void Update()
    {
        float desfase = Mathf.PingPong(Time.time * velocidad, alcance * 2f) - alcance;
        transform.position = origen + new Vector3(desfase, 0f, 0f);
    }
}
