
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DistribuirNaturaleza
{
    const string Ruta = "Assets/RecursosVisuales/";

    [MenuItem("Tools/Paintball/Distribuir naturaleza")]
    public static void Generar()
    {
        GameObject suelo = GameObject.Find("Suelo");
        Renderer terreno = suelo != null
            ? suelo.GetComponent<Renderer>() : null;

        if (terreno == null)
        {
            Debug.LogError("No se encontró Suelo.");
            return;
        }

        if (GameObject.Find("DecoracionGenerada") != null)
        {
            Debug.LogWarning(
                "La decoración ya existe. No se duplicó.");
            return;
        }

        GameObject[] arboles = {
            Cargar("tree_oak"),
            Cargar("tree_pineDefaultA"),
            Cargar("tree_small")
        };

        GameObject[] rocas = {
            Cargar("stone_smallA"),
            Cargar("stone_tallA")
        };

        foreach (GameObject modelo in arboles)
            if (modelo == null) return;

        foreach (GameObject modelo in rocas)
            if (modelo == null) return;

        var azar = new System.Random(2026);
        var ocupados = new List<Vector2>();

        // Respetar árboles y piedras existentes.
        foreach (Transform t in Object.FindObjectsByType<Transform>())
        {
            if (t.name.StartsWith("tree_") ||
                t.name.StartsWith("stone_"))
            {
                ocupados.Add(new Vector2(
                    t.position.x, t.position.z));
            }
        }

        var grupo = new GameObject("DecoracionGenerada");
        Undo.RegisterCreatedObjectUndo(grupo, "Crear decoración");

        Bounds limites = terreno.bounds;

        for (int i = 0; i < 10; i++)
            Colocar(arboles[i % arboles.Length], grupo,
                limites, ocupados, azar, i, true);

        for (int i = 0; i < 14; i++)
            Colocar(rocas[i % rocas.Length], grupo,
                limites, ocupados, azar, i, false);

        EditorSceneManager.MarkSceneDirty(grupo.scene);

        Debug.Log("Decoración generada. Revisar Scene y guardar.");
    }

    static GameObject Cargar(string nombre)
    {
        var modelo = AssetDatabase.LoadAssetAtPath<GameObject>(
            Ruta + nombre + ".fbx");

        if (modelo == null)
            Debug.LogError("Falta el modelo: " + nombre);

        return modelo;
    }

    static void Colocar(
        GameObject modelo, GameObject grupo, Bounds suelo,
        List<Vector2> ocupados, System.Random azar,
        int indice, bool arbol)
    {
        Vector2 punto = Vector2.zero;
        bool disponible = false;

        for (int intento = 0; intento < 60; intento++)
        {
            float margen = 1f + (float)azar.NextDouble();
            float x = RandomEntre(
                azar, suelo.min.x + 1, suelo.max.x - 1);
            float z = RandomEntre(
                azar, suelo.min.z + 1, suelo.max.z - 1);

            switch (indice % 4)
            {
                case 0: x = suelo.min.x + margen; break;
                case 1: x = suelo.max.x - margen; break;
                case 2: z = suelo.min.z + margen; break;
                default: z = suelo.max.z - margen; break;
            }

            punto = new Vector2(x, z);
            disponible = true;

            foreach (Vector2 usado in ocupados)
            {
                if (Vector2.Distance(punto, usado) < 2f)
                {
                    disponible = false;
                    break;
                }
            }

            if (disponible) break;
        }

        if (!disponible) return;

        var objeto = PrefabUtility.InstantiatePrefab(modelo)
            as GameObject;

        if (objeto == null) return;

        objeto.transform.SetParent(grupo.transform);

        float escala = arbol
            ? RandomEntre(azar, 0.8f, 1.15f)
            : RandomEntre(azar, 0.8f, 1.5f);

        objeto.transform.localScale = Vector3.one * escala;
        objeto.transform.position =
            new Vector3(punto.x, suelo.max.y, punto.y);

        objeto.transform.rotation = Quaternion.Euler(
            0, RandomEntre(azar, 0, 360), 0);

        // Corregir la altura según la geometría real.
        Renderer[] mallas =
            objeto.GetComponentsInChildren<Renderer>();

        float baseModelo = float.PositiveInfinity;

        foreach (Renderer malla in mallas)
            baseModelo = Mathf.Min(
                baseModelo, malla.bounds.min.y);

        if (!float.IsPositiveInfinity(baseModelo))
            objeto.transform.position += Vector3.up *
                (suelo.max.y - baseModelo + 0.01f);

        if (!arbol && objeto.GetComponent<Collider>() == null)
            objeto.AddComponent<BoxCollider>();

        Undo.RegisterCreatedObjectUndo(
            objeto, "Colocar decoración");

        ocupados.Add(punto);
    }

    static float RandomEntre(
        System.Random azar, float minimo, float maximo)
    {
        return minimo +
            (float)azar.NextDouble() * (maximo - minimo);
    }
}
