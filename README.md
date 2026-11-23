# NeedMoneyForPaintball

## Descripción
Videojuego de entrenamiento de paintball en primera persona, desarrollado en Unity 6.6 como Categoría 1 — Vertical Slice de TC2008B.

El jugador debe acertar al menos 5 de 7 blancos durante tres rondas, evitando disparar a sus aliados.

## Controles
- Mouse: apuntar.
- Clic izquierdo: disparar.
- REINICIAR: comenzar una nueva partida tras el resultado.

## Mecánicas
- Una escena jugable: Main.
- Tutorial inicial de 15 segundos.
- Tres rondas de 20, 25 y 30 segundos.
- Traslados automáticos entre estaciones.
- Siete blancos válidos y tres aliados.
- Victoria con 5 o más puntos.
- Derrota con 4 o menos puntos.
- Derrota inmediata al impactar a un aliado.
- Proyectiles físicos y efectos de partículas.
- Blanco móvil e instructor animado.

## Requisitos y ejecución
- Unity 6.6 (6000.6.8f1).
- Proyecto 3D Built-In Render Pipeline.
- Abrir la carpeta del proyecto desde Unity Hub.
- Abrir la escena Main.
- Presionar Play.

No se requiere un ejecutable independiente.

## Créditos de recursos visuales

Kenney. Blocky Characters (CC0).
https://kenney.nl/assets/blocky-characters

Kenney. Nature Kit (CC0).
https://kenney.nl/assets/nature-kit

Poly Haven. Dirt Floor (CC0).
https://polyhaven.com/a/dirt_floor

Quaternius. Guard Tower (CC0), Poly Pizza.
https://poly.pizza/m/sbaM8I229r

Quaternius. Practice Dummy (CC0), Poly Pizza.
https://poly.pizza/m/1pYOHhwjXP

Quaternius. Target (CC0), Poly Pizza.
https://poly.pizza/m/Ev3BbukDFS

olgasardykova. Paintball Marker Spyder MR 100 (CC BY).
https://sketchfab.com/3d-models/paintball-marker-spyder-mr-100-f1d0ead2296b4f16b8d3f957c16ac8d2

Miracle3 Judy. Paintball Gun, distribución en Fab.
https://www.fab.com/listings/820a9d1a-e3c2-4cd5-bf34-4885e2fbf43d

La licencia aplicable a la copia distribuida en Fab debe verificarse antes de redistribuir el archivo.

## Uso de inteligencia artificial

Se utilizó ChatGPT y Codex desde Warp para consultar los materiales de las clases y laboratorios, interpretar los requisitos de la rúbrica, revisar los recursos disponibles e investigar problemas de escala y colisiones. Las consultas también abordaron la trayectoria de las bolas de pintura, la orientación de vectores y los cambios de cámara entre estaciones.

El apoyo incluyó la revisión y elaboración de bloques de lógica de mis escenas, la depuración de contactos, verificación de triggers y el ajuste de la interfaz. Además, se utilizó IA para preparar DistribuirNaturaleza.cs, distribuir árboles y rocas, y adaptar visualmente el modelo del instructor dentro de la escena. Durante la presentación inicial se consultó la configuración de los estados Idle y Walk mediante Animator Controller y el parámetro velocidad. El personaje utilizado parte del recurso Blocky Characters de Kenney.

## Referencias académicas

TC2008B — Categoría 1: Vertical Slice.
https://groups.meeplab.com/contenidos/tc2008b/half-term/vertical-slice

Clase Práctica 1 — Comenzando con Unity.
https://groups.meeplab.com/contenidos/tc2008b/clases/lab0-comenzando-unity

Clase Práctica 2 — Del prototipo al play loop.
https://groups.meeplab.com/contenidos/tc2008b/clases/lab0-1-del-prototipo-al-loop

Clase Práctica 3 — De graybox a vertical slice.
https://groups.meeplab.com/contenidos/tc2008b/clases/lab0-2-assets-y-vertical-slice

## Apoyo en la documentación del repositorio

Se utilizó ChatGPT de OpenAI como apoyo para preparar el README, organizar los archivos del repositorio y configurar .gitignore, tomando como referencia la rúbrica de Categoría 1 y el Laboratorio 4.

Referencia: OpenAI. (2026). ChatGPT. https://chatgpt.com/
