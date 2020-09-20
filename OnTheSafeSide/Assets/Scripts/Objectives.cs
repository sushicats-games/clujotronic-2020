using System;
using System.Linq;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    public Detection detection;
    public World world;
    public Controller controller;

    int objectiveId = 0;
    Func<bool> checkCompletion = () => true;
    string objectiveMessage = "";

    // Start is called before the first frame update
    void Start()
    {
        objectiveId = 1;
        (objectiveMessage, checkCompletion) = NextObjective();
        Debug.Log(objectiveMessage);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkCompletion())
        {
            objectiveId++;
            (objectiveMessage, checkCompletion) = NextObjective();
            Debug.Log(objectiveMessage);
        }
    }

    (string, Func<bool>) NextObjective()
    {
        switch (objectiveId)
        {
            case 1:
            {
                var tool = controller.ToolPicker.GetPickedTool();
                return ("Select a wall from Tool picker!", () 
                    => controller.ToolPicker.GetPickedTool() != tool 
                    && controller.ToolPicker.GetPickedTool().Type == "wall");
            }
            case 2:
            {
                return ("Click anywhere in the world to place a the wall!", ()
                    => world.AllCells().Where(c => !c.IsEmpty()).Any());
            }
            case 3:
            {
                var viewX = controller.viewX;
                var viewZ = controller.viewZ;
                return ("Move the camera around with WASD keys!", ()
                    => controller.viewX != viewX || controller.viewZ != viewZ);
            }
            case 4:
            {
                var rx = controller.viewRotationX;
                var ry = controller.viewRotationY;
                return ("Rotate the camera by pressing middle mouse button (the scroll wheel) and moving the mouse!", ()
                    => controller.viewRotationX != rx || controller.viewRotationY != ry);
            }
            case 5:
            {
                var rot = controller.placementRotation;
                return ("Rotate the wall piece with the mouse scroll!", ()
                    => controller.placementRotation != rot);
            }
            case 6:
            {
                return ("Build a house!", ()
                    => detection.buildingStats.Count > 0);
            }
            case 7:
            {
                return ("Add an additional room to the house!", ()
                    => detection.roomStats.Count >= 2);
            }
            case 8:
            {
                return ("Complete the house by building a total of 4 rooms!", ()
                    => detection.roomStats.Count >= 4);
            }
            case 9:
            {
                var step = controller.viewDistanceStep;
                return ("Zoom out to see your creation with CTRL + scroll!", ()
                    => controller.viewDistanceStep >= step + 4 || controller.viewDistanceStep >= Controller.MaxDistanceStep - 4);
            }
            case 10:
            {
                return ("Build one more house!", ()
                    => detection.buildingStats.Count > 1);
            }
        }

        return ("Completed the game! Continue building!", () => false);
    }

}
