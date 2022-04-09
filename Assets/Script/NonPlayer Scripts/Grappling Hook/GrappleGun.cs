using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingHook grappleRope;//script de la cuerda 

    [Header("Layers Settings:")]//parametros modificables en el editor para testear
    [SerializeField] private int grappableLayerNumber = 3;

    [Header("Main Camera:")]
    public Camera m_camera; 

    [Header("Transform Ref:")]
    public Transform gunHolder;//Posicion del player (portador)
    public Transform gunPivot;//punto de rotacion del grappling gun
    public Transform firePoint;//punto desde el que se dispara la cuerda

    [Header("Physics Ref:")]//referencias a componentes del player
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistanec = 20;

    [SerializeField] private float launchSpeed = 1;//velocidad a la que se lanza el portador hacia el punto 

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            SetGrapplePoint();

        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (grappleRope.enabled)
            {   
                RotateGun(grapplePoint);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos);
        }
    }


    void RotateGun(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        RaycastHit2D raycastHasHit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);

        if (raycastHasHit)
        {
            if (raycastHasHit.transform.gameObject.layer == grappableLayerNumber)
            {
                if (Vector2.Distance(raycastHasHit.point, firePoint.position) <= maxDistanec || !hasMaxDistance)
                {
                    grapplePoint = raycastHasHit.point;
                    //grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                }
            }
        }
    }
    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;

        m_springJoint2D.connectedAnchor = grapplePoint;

        Vector2 distanceVector = firePoint.position - gunHolder.position;

        m_springJoint2D.distance = distanceVector.magnitude;
        m_springJoint2D.frequency = launchSpeed;
        m_springJoint2D.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistanec);
        }
    }
}
