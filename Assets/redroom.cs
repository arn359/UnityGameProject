using UnityEngine;

public class redroom : MonoBehaviour
{
    public static bool isroomRed = false;
    public static bool isNormal = false;   

    public static void turnRed()
    {
        isroomRed = true;
        isNormal = false;
    }

    public static void goNormal()
    {
        isNormal = true;       
        isroomRed = false;      

        Vase[] allObjects = FindObjectsOfType<Vase>();
        foreach (Vase obj in allObjects)
            obj.clickCount = 0;
    }
}