using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjects : MonoBehaviour {

    bool floatup;
    void Start()
    {
        floatup = false;
    }
    void Update()
    {
        if (floatup)
          StartCoroutine(Floatingup());
        else if (!floatup)
          StartCoroutine(Floatingdown());
    }

    IEnumerator Floatingup()
    {
        transform.position= transform.position + new Vector3(0 , 0.3f * Time.deltaTime ,0);
        yield return new WaitForSeconds(1);
        floatup = false;
    }
    IEnumerator Floatingdown()
    {
        transform.position = transform.position - new Vector3(0, 0.3f * Time.deltaTime, 0);
        yield return new WaitForSeconds(1);
        floatup = true;
    }
}
