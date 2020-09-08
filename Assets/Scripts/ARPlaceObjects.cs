using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlaceObjects : MonoBehaviour
{

    public GameObject gameObjectToInstantiate;

    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    private MenuController mc;

    private void Start()
    {
        placed = false;
        state = Object.Moving;
    }

    enum Object
    {
        Moving,
        Scale,
        Rotate
    }

    Object state;

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    bool placed;
    Pose hitPose;
    void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            hitPose = hits[0].pose;

            if(spawnedObject == null)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
            } else
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if(state == Object.Moving)
                        spawnedObject.transform.position = hitPose.position;
                }
            }

            placed = true;
        }

        if(placed)
        {
            mc.ObjectMenu_On();
        }

        if(state == Object.Rotate)
        {
            Rotate();
        }

        if(state == Object.Scale)
        {
            Scale();
        }
    }

    private void Moving(Pose hitPose)
    {
        spawnedObject.transform.position = hitPose.position;
    }

    public void ChangeToMoving()
    {
        state = Object.Moving;
    }

    public void ChangeToRotate()
    {
        state = Object.Rotate;
    }

    public void ChangeToScale()
    {
        state = Object.Scale;
    }

    private Touch touch;
    private Quaternion rotationY;
    private float rotationSpeed = 5f;
    public void Rotate()
    {
        if(Input.touchCount == 1)
        {
            float rotateSpeed = .5f;
            Touch touch = Input.GetTouch(0);
        
            spawnedObject.transform.Rotate(0, -touch.deltaPosition.x * rotateSpeed, -touch.deltaPosition.y * rotateSpeed, Space.Self);
        }
    }

    private float initialFingersDistance;
    private Vector3 initialScale;
    public void Scale()
    {
        if(Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if(t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(t1.position, t2.position);
                initialScale = spawnedObject.transform.localScale;
            } else if(t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                var currentFingersDistance = Vector2.Distance(t1.position, t2.position);
                var scaleFactor = currentFingersDistance / initialFingersDistance;
                spawnedObject.transform.localScale = initialScale * scaleFactor;
            }
        }
    }

    public void Clear()
    {
        Destroy(spawnedObject);
        placed = false;

        mc.ObjectMenu_Off();
    }
}
