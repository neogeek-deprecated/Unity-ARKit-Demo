using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class ARKitObjectController : MonoBehaviour
{

    public GameObject parentGameObject;

    public Text textComp;

    public Camera mainCamera;
    public Light mainDirectionalLight;
    public Material clearMaterial;

    private bool anchorSet = false;

    private void Awake()
    {

        if (parentGameObject == null)
        {

            parentGameObject = gameObject;

        }

        ChildrenSetActive(false);

    }

    private void Start()
    {

        if (mainCamera == null)
        {

            mainCamera = Camera.main;

        }

        if (mainDirectionalLight == null)
        {

            mainDirectionalLight = Object.FindObjectOfType<Light>();

        }

        mainCamera.clearFlags = CameraClearFlags.Depth;
        mainCamera.nearClipPlane = 0.01f;

        if (mainCamera.gameObject.GetComponent<UnityARVideo>() == null)
        {

            UnityARVideo unityARVideo = mainCamera.gameObject.AddComponent<UnityARVideo>();
            unityARVideo.m_ClearMaterial = clearMaterial;

        }

        if (mainCamera.gameObject.GetComponent<UnityARCameraNearFar>() == null)
        {

            mainCamera.gameObject.AddComponent<UnityARCameraNearFar>();

        }

        if (mainCamera.gameObject.GetComponent<UnityARCameraManager>() == null)
        {

            UnityARCameraManager unityARCameraManager = mainCamera.gameObject.AddComponent<UnityARCameraManager>();
            unityARCameraManager.m_camera = mainCamera;

        }

        if (mainDirectionalLight.gameObject.GetComponent<UnityARAmbient>() == null)
        {

            mainDirectionalLight.gameObject.AddComponent<UnityARAmbient>();

        }

        UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;

    }

    private void Update()
    {

        if (anchorSet == false && Input.GetMouseButtonDown(0))
        {

            Vector3 screenPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            ARPoint point = new ARPoint
            {
                x = screenPosition.x,
                y = screenPosition.y
            };

            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, ARHitTestResultType.ARHitTestResultTypeHorizontalPlane);

            if (hitResults.Count > 0)
            {

                anchorSet = true;

                ChildrenSetActive(true);

                parentGameObject.transform.position = UnityARMatrixOps.GetPosition(hitResults[0].worldTransform);
                parentGameObject.transform.rotation = UnityARMatrixOps.GetRotation(hitResults[0].worldTransform);
                parentGameObject.transform.LookAt(new Vector3(mainCamera.transform.position.x, parentGameObject.transform.position.y, mainCamera.transform.position.z));

            }

        }

    }

    public void InvaidatePlane()
    {

        anchorSet = false;

        ChildrenSetActive(false);

    }

    private void ARFrameUpdated(UnityARCamera camera)
    {

        if (textComp != null)
        {

            textComp.text = string.Format("Points: {0}", camera.pointCloudData.Length);

        }

    }

    private void ChildrenSetActive(bool active)
    {

        foreach (Transform childTransform in parentGameObject.transform)
        {

            childTransform.gameObject.SetActive(active);

        }

    }

    private void Reset()
    {

#if UNITY_EDITOR

        if (clearMaterial == null)
        {

            clearMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/UnityARKitPlugin/Plugins/iOS/UnityARKit/Materials/YUVMaterial.mat", typeof(Material));

        }

#endif

    }

    private void OnDestroy()
    {

        UnityARSessionNativeInterface.ARFrameUpdatedEvent -= ARFrameUpdated;

    }

}