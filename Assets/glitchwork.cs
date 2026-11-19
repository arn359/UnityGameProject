using UnityEngine;
using System.Collections;

public class glitchwork : MonoBehaviour
{

    public GameObject glitchobj;
    public float glitchoff = 10f;
    public float glitchon = 5f; 

    void Start()
    {
        StartCoroutine(glitchloop());
        
    }



    
   
    IEnumerator glitchloop()
    {
        while (true)
        {
            if (!redroom.isroomRed)
            {
                if (glitchobj != null && glitchobj.activeSelf)
                {
                    glitchobj.SetActive(false);
                }
                yield return null;
                continue;
            
            }

            if (glitchobj != null) glitchobj.SetActive(false);
            yield return new WaitForSeconds(glitchon);
                
            if (glitchobj != null) glitchobj.SetActive(true);
            yield return new WaitForSeconds(glitchoff);
        }
    }
        
    
}
