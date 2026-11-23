
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorPartida : MonoBehaviour
{
    public enum Fase
    {
        Instructor,
        Tutorial,
        Transicion,
        Ronda1,
        Ronda2,
        Ronda3,
        Final
    }

    [Header("Camara del jugador")]
    public Transform camaraJugador;

    [Header("Posiciones de camara")]
    public Transform vistaInstructor;
    public Transform puestoTutorial;
    public Transform puestoRonda1;
    public Transform puestoRonda2;
    public Transform puestoRonda3;

    [Header("Objetivos por etapa")]
    public GameObject blancoPractica;
    public GameObject[] blancosRonda1 = new GameObject[2];
    public GameObject[] aliadosRonda1 = new GameObject[1];
    public GameObject[] blancosRonda2 = new GameObject[2];
    public GameObject[] blancosRonda3 = new GameObject[3];
    public GameObject[] aliadosRonda3 = new GameObject[2];

    [Header("HUD")]
    public GameObject globoInstructor;
    public TMP_Text textoFase;
    public TMP_Text textoTiempo;
    public TMP_Text textoPuntos;
    public TMP_Text textoAviso;

    [Header("Animacion del instructor")]
    public Animator animadorInstructor;

    [Header("Finalizacion")]
    public GameObject panelResultado;
    public TMP_Text textoResultado;

    private Apuntado apuntado;
    private Disparo disparo;

    private Fase faseActual;
    private int puntos = 0;
    private bool partidaTerminada = false;

    private readonly HashSet<GameObject> blancosAlcanzados =
        new HashSet<GameObject>();

    void Start()
    {
        if (camaraJugador == null ||
            vistaInstructor == null ||
            puestoTutorial == null ||
            puestoRonda1 == null ||
            puestoRonda2 == null ||
            puestoRonda3 == null)
        {
            Debug.LogError(
                "GestorPartida: faltan referencias de camara o puestos.",
                this
            );
            enabled = false;
            return;
        }

        apuntado = camaraJugador.GetComponent<Apuntado>();
        disparo = camaraJugador.GetComponent<Disparo>();

        if (apuntado == null || disparo == null)
        {
            Debug.LogError(
                "Main Camera necesita Apuntado y Disparo.",
                this
            );
            enabled = false;
            return;
        }

        if (panelResultado != null)
            panelResultado.SetActive(false);

        if (textoAviso != null)
            textoAviso.text = "";

        ActualizarPuntos();
        MostrarTodos(false);

        StartCoroutine(SecuenciaPartida());
    }

    IEnumerator SecuenciaPartida()
    {
        // Introduccion: 4 segundos.
        faseActual = Fase.Instructor;
        ActualizarFase("TUTORIAL");

        ActivarControles(false);
        PosicionarCamara(vistaInstructor);

        if (animadorInstructor != null)
            animadorInstructor.SetFloat("velocidad", 1f);

        if (globoInstructor != null)
            globoInstructor.SetActive(true);

        Debug.Log("Instructor: 4 segundos");

        // Muestra los 15 segundos totales del tutorial.
        yield return ContarTiempo(4f, 11f);

        if (animadorInstructor != null)
            animadorInstructor.SetFloat("velocidad", 0f);

        // Practica: 11 segundos.
        faseActual = Fase.Tutorial;
        PosicionarCamara(puestoTutorial);

        if (globoInstructor != null)
            globoInstructor.SetActive(false);

        if (blancoPractica != null)
            blancoPractica.SetActive(true);

        ActivarControles(true);
        Debug.Log("Practica: 11 segundos");

        yield return ContarTiempo(11f);

        // Ronda 1: 20 segundos.
        yield return CambiarPuesto(puestoRonda1);

        IniciarRonda(
            Fase.Ronda1,
            "RONDA 1",
            blancosRonda1,
            aliadosRonda1
        );

        yield return ContarTiempo(20f);

        // Ronda 2: 25 segundos.
        yield return CambiarPuesto(puestoRonda2);

        IniciarRonda(
            Fase.Ronda2,
            "RONDA 2",
            blancosRonda2,
            null
        );

        yield return ContarTiempo(25f);

        // Ronda 3: 30 segundos.
        yield return CambiarPuesto(puestoRonda3);

        IniciarRonda(
            Fase.Ronda3,
            "RONDA 3",
            blancosRonda3,
            aliadosRonda3
        );

        yield return ContarTiempo(30f);

        if (!partidaTerminada)
        {
            bool victoria = puntos >= 5;

            TerminarPartida(
                victoria,
                victoria
                    ? "¡ENTRENAMIENTO COMPLETADO!"
                    : "PUNTUACION INSUFICIENTE"
            );
        }
    }

    void IniciarRonda(
        Fase nuevaFase,
        string nombre,
        GameObject[] blancos,
        GameObject[] aliados)
    {
        MostrarTodos(false);

        faseActual = nuevaFase;
        ActualizarFase(nombre);

        MostrarGrupo(blancos, true);
        MostrarGrupo(aliados, true);

        ActivarControles(true);

        Debug.Log("Comienza " + nombre);
    }

    IEnumerator CambiarPuesto(Transform destino)
    {
        faseActual = Fase.Transicion;

        ActivarControles(false);
        MostrarTodos(false);
        LimpiarProyectiles();

        ActualizarFase("TRASLADO");

        Vector3 posicionInicial = camaraJugador.position;
        Quaternion rotacionInicial = camaraJugador.rotation;

        float tiempo = 0f;

        while (tiempo < 3f && !partidaTerminada)
        {
            tiempo += Time.deltaTime;

            float progreso = Mathf.Clamp01(tiempo / 3f);

            camaraJugador.position = Vector3.Lerp(
                posicionInicial,
                destino.position,
                progreso
            );

            camaraJugador.rotation = Quaternion.Slerp(
                rotacionInicial,
                destino.rotation,
                progreso
            );

            ActualizarTiempo(Mathf.Max(0f, 3f - tiempo));

            yield return null;
        }

        if (!partidaTerminada)
            PosicionarCamara(destino);
    }

    IEnumerator ContarTiempo(float duracion, float extra = 0f)
    {
        float restante = duracion;

        while (restante > 0f && !partidaTerminada)
        {
            ActualizarTiempo(restante + extra);

            yield return null;
            restante -= Time.deltaTime;
        }

        if (!partidaTerminada)
            ActualizarTiempo(extra);
    }

    public void RegistrarImpacto(GameObject objetivo)
    {
        if (partidaTerminada || objetivo == null)
            return;

        // Practica sin puntuacion.
        if (faseActual == Fase.Tutorial)
        {
            if (objetivo == blancoPractica)
            {
                Debug.Log("Impacto de practica");
                MostrarAviso("¡BUEN DISPARO!");
            }

            return;
        }

        // Durante traslados e introduccion no hay puntos.
        if (faseActual != Fase.Ronda1 &&
            faseActual != Fase.Ronda2 &&
            faseActual != Fase.Ronda3)
        {
            return;
        }

        // Derrota inmediata por aliado de la ronda actual.
        if (objetivo.CompareTag("Aliado"))
        {
            bool aliadoValido =
                faseActual == Fase.Ronda1
                    ? Pertenece(objetivo, aliadosRonda1)
                    : faseActual == Fase.Ronda3 &&
                      Pertenece(objetivo, aliadosRonda3);

            if (aliadoValido)
            {
                Debug.Log("Aliado alcanzado: " + objetivo.name);
                TerminarPartida(false, "¡ALIADO ALCANZADO!");
            }

            return;
        }

        if (!objetivo.CompareTag("Blanco"))
            return;

        GameObject[] grupoActual = null;

        switch (faseActual)
        {
            case Fase.Ronda1:
                grupoActual = blancosRonda1;
                break;

            case Fase.Ronda2:
                grupoActual = blancosRonda2;
                break;

            case Fase.Ronda3:
                grupoActual = blancosRonda3;
                break;
        }

        // Ignora objetivos que no sean de esta ronda.
        if (!Pertenece(objetivo, grupoActual))
            return;

        // HashSet impide puntuar dos veces el mismo blanco.
        if (!blancosAlcanzados.Add(objetivo))
        {
            Debug.Log("Blanco repetido: " + objetivo.name);
            return;
        }

        puntos = Mathf.Min(7, puntos + 1);
        ActualizarPuntos();

        Debug.Log(
            "Blanco alcanzado: " + objetivo.name +
            " | Puntos: " + puntos + "/7"
        );

        MostrarAviso("¡BLANCO ALCANZADO! +1");

        // Ya no puede recibir otro impacto.
        objetivo.SetActive(false);
    }

    bool Pertenece(GameObject objetivo, GameObject[] grupo)
    {
        if (grupo == null)
            return false;

        foreach (GameObject elemento in grupo)
        {
            if (elemento == objetivo)
                return true;
        }

        return false;
    }

    void MostrarGrupo(GameObject[] grupo, bool mostrar)
    {
        if (grupo == null)
            return;

        foreach (GameObject elemento in grupo)
        {
            if (elemento != null)
            {
                bool disponible =
                    mostrar && !blancosAlcanzados.Contains(elemento);

                elemento.SetActive(disponible);
            }
        }
    }

    void MostrarTodos(bool mostrar)
    {
        if (blancoPractica != null)
            blancoPractica.SetActive(mostrar);

        MostrarGrupo(blancosRonda1, mostrar);
        MostrarGrupo(aliadosRonda1, mostrar);
        MostrarGrupo(blancosRonda2, mostrar);
        MostrarGrupo(blancosRonda3, mostrar);
        MostrarGrupo(aliadosRonda3, mostrar);
    }

    void PosicionarCamara(Transform destino)
    {
        camaraJugador.SetPositionAndRotation(
            destino.position,
            destino.rotation
        );

        if (apuntado != null)
            apuntado.SincronizarOrientacion();
    }

    void ActivarControles(bool activos)
    {
        if (apuntado != null)
            apuntado.enabled = activos;

        if (disparo != null)
            disparo.enabled = activos;
    }

    void LimpiarProyectiles()
    {
        Proyectil[] proyectiles =
            FindObjectsByType<Proyectil>(FindObjectsSortMode.None);

        foreach (Proyectil proyectil in proyectiles)
        {
            if (proyectil != null)
                Destroy(proyectil.gameObject);
        }
    }

    void ActualizarFase(string nombre)
    {
        if (textoFase != null)
            textoFase.text = nombre;

        if (textoAviso != null)
            textoAviso.text = "";
    }

    void ActualizarTiempo(float restante)
    {
        if (textoTiempo == null)
            return;

        int segundos = Mathf.CeilToInt(restante);

        textoTiempo.text = string.Format(
            "{0:00}:{1:00}",
            segundos / 60,
            segundos % 60
        );
    }

    void ActualizarPuntos()
    {
        if (textoPuntos != null)
            textoPuntos.text = "PUNTOS " + puntos + " / 7";
    }

    void MostrarAviso(string mensaje)
    {
        if (textoAviso != null)
            textoAviso.text = mensaje;
    }

    void TerminarPartida(bool victoria, string motivo)
    {
        if (partidaTerminada)
            return;

        partidaTerminada = true;
        faseActual = Fase.Final;

        StopAllCoroutines();

        ActivarControles(false);
        MostrarTodos(false);
        LimpiarProyectiles();

        ActualizarFase("RESULTADO");
        ActualizarTiempo(0f);

        if (globoInstructor != null)
            globoInstructor.SetActive(false);

        if (textoResultado != null)
        {
            textoResultado.text =
                (victoria ? "¡VICTORIA!" : "DERROTA") +
                "\n" + motivo +
                "\nPUNTOS: " + puntos + " / 7";
        }

        if (panelResultado != null)
            panelResultado.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log(
            (victoria ? "VICTORIA" : "DERROTA") +
            " | " + motivo +
            " | Puntos: " + puntos
        );
    }

    public void ReiniciarPartida()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
