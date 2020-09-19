using System;
using UnityEngine;

public class Controller : MonoBehaviour
{
    new public Camera camera;
    public World world;
    int placementRotation = 0; // 0 1 2 3 0 1 2 3
    bool cameraNeedsUpdate = true;
    float viewX = 0;
    float viewZ = 0;
    float viewRotationY = 0; // 0..360
    float viewRotationX = 40;
    int viewDistanceStep = 0;

    const int MinDistanceStep = -4;
    const int MaxDistanceStep = 14;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboard();
        HandleMouseScroll();
        HandleMouseClicks();
        ShowPlacementPreview();
        UpdateCamera();
    }

    private void HandleMouseScroll()
    {
        var controlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (Input.mouseScrollDelta.y > 0)
        {
            if (controlPressed)
            {
                ZoomInAction();
            }
            else
            {
                NextRotationAction();
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (controlPressed)
            {
                ZoomOutAction();
            }
            else
            {
                PrevRotationAction();
            }
        }
    }

    private void HandleMouseClicks()
    {
        if (Input.GetMouseButton(0))
        {
            AddAction();
        }
        else if (Input.GetMouseButton(1))
        {
            DeleteAction();
        }
        else if (Input.GetMouseButton(2))
        {
            RotateCamera(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        }
    }

    private void HandleKeyboard()
    {
        float forward = ToInt(Input.GetKey(KeyCode.W)) - ToInt(Input.GetKey(KeyCode.S));
        float right = ToInt(Input.GetKey(KeyCode.D)) -  ToInt(Input.GetKey(KeyCode.A));
        if (forward == 0 && right == 0)
        {
            return;
        }
        MoveCamera(forward * Time.deltaTime, right * Time.deltaTime);
    }

    private int ToInt(bool b) => b ? 1 : 0;

    private void MoveCamera(float forward, float right)
    {
        float speed = 4;
        var movement = Quaternion.Euler(0, viewRotationY, 0) * new Vector3(right, 0, forward);
        viewX += movement.x * speed;
        viewZ += movement.z * speed;
        cameraNeedsUpdate = true;
    }

    void RotateCamera(float mouseAxisX, float mouseAxisY)
    {
        var speed = 5;
        viewRotationX += mouseAxisY * speed;
        viewRotationY += mouseAxisX * speed;
        viewRotationX = Mathf.Clamp(viewRotationX, 10, 90);
        cameraNeedsUpdate = true;
    }

    private void ZoomInAction()
    {
        viewDistanceStep--;
        if (viewDistanceStep < MinDistanceStep) {
            viewDistanceStep = MinDistanceStep;
        }
        cameraNeedsUpdate = true;
    }

    private void ZoomOutAction()
    {
        viewDistanceStep++;
        if (viewDistanceStep > MaxDistanceStep){
            viewDistanceStep = MaxDistanceStep;
        }
        cameraNeedsUpdate = true;
    }
    

    void UpdateCamera()
    {
        if (!cameraNeedsUpdate)
        {
            return;
        }
        var viewDistance = Mathf.Pow(2.0f, viewDistanceStep / 4.0f) * 4.0f;
        Debug.Log($"distance: {viewDistance}");
        var rotation = Quaternion.Euler(viewRotationX, viewRotationY, 0);
        camera.transform.position = new Vector3(viewX, 0, viewZ) - rotation * Vector3.forward * viewDistance;
        camera.transform.rotation = rotation;
        cameraNeedsUpdate = false;
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
