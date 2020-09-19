using System;
using UnityEngine;

public class Controller : MonoBehaviour
{
    new public Camera camera;
    public World world;
    int placementRotation = 0; // 0 1 2 3

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Helllo!");
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseScroll();
        HandleMouseClicks();
        ShowPlacementPreview();
    }

    private void HandleMouseScroll()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Debug.Log("Left");
            NextRotationAction();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Debug.Log("Right");
            PrevRotationAction();
        }
    }

    private void HandleMouseClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddAction();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DeleteAction();
        }
    }

    void NextRotationAction()
    {
        placementRotation = (placementRotation - 1) & 3;
    }

    void PrevRotationAction()
    {
        placementRotation = (placementRotation + 1) & 3;
    }

    void AddAction()
    {
        var position = GetPositionUnderMouse();
        world.PutBlock(position, placementRotation);
    }

    void DeleteAction()
    {
        var position = GetPositionUnderMouse();
        world.EraseBlock(position);
    }

    void ShowPlacementPreview()
    {
        var position = GetPositionUnderMouse();
        world.ShowPreviewBlock(position, placementRotation);
    }

    Vector3 GetPositionUnderMouse()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.positiveInfinity;
    }
}
