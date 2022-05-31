using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingHook grappleRope;// referencia al script de la cuerda 

    [Header("Layers Settings:")]
    [SerializeField] private int grappableLayerNumber = 3;

    [Header("Main Camera:")]
    public Camera m_camera; 

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]//referencias a componentes del player
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistanec = 20;

    [SerializeField] private float launchSpeed = 0.7f;//velocidad a la que se lanza el portador hacia el punto de agarre
    [SerializeField] private float activeLaunchSpeed = 2f;//fuerza a la que se lanza el portador hacia el punto de agarre

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;

    }

    private void Update()
    {
        //TODO Esto se deberia implementar como un segundo tipo de cuerda que cambie el launchspeed
        if (Input.GetKey(KeyCode.W))
            launchSpeed = activeLaunchSpeed;
        else
            launchSpeed = 0.7f;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            SetGrapplePoint();
        //replace launch speed with active launch speed if the player is holding up
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

                Debug.DrawRay(transform.position, new Vector3(mousePos.x, mousePos.y), Color.red);
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
        //disable grapple rope if it is too far
        if (hasMaxDistance)
        {
            if (Vector2.Distance(transform.position, grapplePoint) > maxDistanec)
            {
                grappleRope.enabled = false;
                m_springJoint2D.enabled = false;
                m_rigidbody.gravityScale = 1;
            }
        }
    }

    /// <summary>
    /// sets the gun's rotation variable to the angle between the lookPoint and the x axis of the player
    /// </summary>
    /// <param name="lookPoint">The current position of the mouse</param>
    void RotateGun(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    /// <summary>
    /// sets the grapple point to the intersection between the first grappable element in the scene and the mouse position
    /// </summary>
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
    /// <summary>
    /// Enables the grappling rope and sets the joint's anchor to the grapple point
    /// </summary>
    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;

        m_springJoint2D.connectedAnchor = grapplePoint;

        Vector2 distanceVector = firePoint.position - gunHolder.position;

        m_springJoint2D.distance = distanceVector.magnitude;
        m_springJoint2D.frequency = launchSpeed;
        m_springJoint2D.enabled = true;
    }
    /// <summary>
    /// Disables the grappling rope
    /// </summary>
    public void Disbable()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        m_rigidbody.gravityScale = 1;
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
