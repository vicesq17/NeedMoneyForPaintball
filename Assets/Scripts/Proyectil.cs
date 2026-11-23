
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public GameObject prefabImpacto;

    private bool impactoRegistrado = false;
    private GestorPartida gestor;

    void Start()
    {
        gestor = FindFirstObjectByType<GestorPartida>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (impactoRegistrado)
            return;

        Transform objetivo = BuscarObjetivo(other.transform);

        // Ignora triggers ajenos a los blancos.
        if (objetivo == null)
            return;

        ProcesarImpacto(
            other,
            objetivo,
            other.ClosestPoint(transform.position)
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (impactoRegistrado)
            return;

        Collider collider = collision.collider;
        Transform objetivo = BuscarObjetivo(collider.transform);

        Vector3 punto = transform.position;

        if (collision.contactCount > 0)
            punto = collision.GetContact(0).point;

        ProcesarImpacto(collider, objetivo, punto);
    }

    Transform BuscarObjetivo(Transform objeto)
    {
        // Revisa el objeto alcanzado y sus padres.
        while (objeto != null)
        {
            if (objeto.CompareTag("Practica") ||
                objeto.CompareTag("Blanco") ||
                objeto.CompareTag("Aliado"))
            {
                return objeto;
            }

            objeto = objeto.parent;
        }

        return null;
    }

    void ProcesarImpacto(
        Collider collider,
        Transform objetivo,
        Vector3 posicion)
    {
        if (impactoRegistrado)
            return;

        impactoRegistrado = true;

        if (objetivo != null)
        {
            Debug.Log(
                "Colision detectada: " + collider.name +
                " | Objetivo: " + objetivo.name
            );

            if (gestor != null)
                gestor.RegistrarImpacto(objetivo.gameObject);
            else
                Debug.LogWarning("No se encontro GestorPartida.");
        }
        else
        {
            Debug.Log(
                "Impacto contra superficie: " + collider.name
            );
        }

        if (prefabImpacto != null)
        {
            Instantiate(
                prefabImpacto,
                posicion,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}
