using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    #region referencias 
    [Header("General Refernces:")]
    public GrappleGun grapplingGun;//Script asociado a el lanzador de la cuerda
    public LineRenderer m_lineRenderer;//Un elemento lineRenderer que tendra el personaje del jugador
    #endregion
    #region propiedades fisicas de la cuerda
    [Header("General Settings:")]
    [SerializeField] private int ropeRenderPoints = 40;//el numero de puntos que toma el renderer para crear la cuerda
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 5;//Determina el tiempo que tarda la cuerda en tensarse
    #endregion
    [Header("Rope Animation Settings:")]
    #region animacion de la cuerda
    public AnimationCurve ropeAnimationCurve;//Curva de animacion de la cuerda (Aun no lo entiendo muy bien)
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;//Tamaño inicial de la onda
    float waveSize = 0;//tamaño de la onda
    #endregion
    #region progresion
    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;//Tampoco acabo de entender esto muy bien, revisar
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1;
    #endregion 
    float moveTime = 0;//determina el tiempo que la cuerda esta en movimiento

    [HideInInspector] public bool isGrappling = true;//valor bool que determina si la cuerda esta enganchada o no

    bool strightLine = true;//variable que determina si la cuerda debe tensarse

    private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.positionCount = ropeRenderPoints; 
        waveSize = StartWaveSize;
        strightLine = false;

        LinePointsToFirePoint();

        m_lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }
    /// <summary>
    /// Inicializa todos los puntos del renderer de la cuerda al punto desde el que se lanza
    /// </summary>
    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < ropeRenderPoints; i++)
        {
            m_lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
        }
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }
    /// <summary>
    /// Este metodo determina el comportamiento de la animacion de la cuerda. Esta animacion se lleva a cabo
    /// en base a los puntos del renderer de la cuerda y el estado del "hook"
    /// </summary>
    void DrawRope()
    {
        if (!strightLine)
        {
            if (m_lineRenderer.GetPosition(ropeRenderPoints - 1).x == grapplingGun.grapplePoint.x)
            {
                strightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling)
            {
                grapplingGun.Grapple();
                isGrappling = true;
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }

                DrawRopeNoWaves();
            }
        }
    }
    /// <summary>
    /// Animacion de la cuerda utilizando movidas matematicas, copiado de un tutorial, pero queda fachero
    /// </summary>
    void DrawRopeWaves()
    {
        for (int i = 0; i < ropeRenderPoints; i++)
        {
            float delta = i / (ropeRenderPoints - 1f);
            Vector2 offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            m_lineRenderer.SetPosition(i, currentPosition);
        }
    }
    /// <summary>
    /// Establece el punto inicial y final de la cuerda y dibuja una linea recta entre ambos (La cuerda esta tensa)
    /// </summary>
    void DrawRopeNoWaves()
    {
        m_lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
        m_lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
    }
}
