using UnityEngine;

public class Controller : MonoBehaviour
{
    new public Camera camera;
    public World world;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Helllo!");
    }

    // Update is called once per frame
    void Update()
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

    void AddAction()
    {
        var position = GetPositionUnderMouse();
        world.PutBlock(position);
    }

    void DeleteAction()
    {
        var position = GetPositionUnderMouse();
        world.EraseBlock(position);
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
