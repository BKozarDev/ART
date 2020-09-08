using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ARManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _arSessionOrigin;

    [SerializeField]
    private Button button;

    [SerializeField]
    private GameObject textMesh;

    private bool switcher;
    private TMPro.TextMeshProUGUI _text;

    private ImageRecognition _arImageRecognition;
    private ARPlaceObjects _arPlane;

    private void Start()
    {
        _text = textMesh.GetComponent<TMPro.TextMeshProUGUI>();
        _arImageRecognition = _arSessionOrigin.GetComponent<ImageRecognition>();
        _arPlane = _arSessionOrigin.GetComponent<ARPlaceObjects>();

        Switcher();

        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Plane";
        switcher = true;
    }

    public void Switcher()
    {
        if(!switcher)
        {
            //Text
            _text.text = "Find some image to tracking";

            // Image Tracking 
            _arSessionOrigin.GetComponent<ARTrackedImageManager>().enabled = true;

            // Plane Detection
            _arSessionOrigin.GetComponent<ARPlaneManager>().enabled = false;
            _arSessionOrigin.GetComponent<ARRaycastManager>().enabled = false;
            _arSessionOrigin.GetComponent<ARPlaceObjects>().enabled = false;

            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Plane";
            Cleaner(switcher);
            switcher = true;

        } else
        {
            //Text
            _text.text = "Touch any plane to put object";

            // Image Tracking 
            _arSessionOrigin.GetComponent<ARTrackedImageManager>().enabled = false;

            // Plane Detection
            _arSessionOrigin.GetComponent<ARPlaneManager>().enabled = true;
            _arSessionOrigin.GetComponent<ARRaycastManager>().enabled = true;
            _arSessionOrigin.GetComponent<ARPlaceObjects>().enabled = true;

            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Track Image";
            Cleaner(switcher);
            switcher = false;
        }
    }

    private void Cleaner(bool switcher)
    {
        if(!switcher)
        {
            _arPlane.Clear();
        } else
        {
            _arImageRecognition.Clear();
        }
    }
}
