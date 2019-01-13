using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Profiler.BeginSample("zyf");

		print("qwe");

		Profiler.EndSample();
    }

    // Update is called once per frame
    void Update()
    {
		Profiler.BeginSample("zyf");

		//print("qwe");
		print(Camera.main.transform.position);

		Profiler.EndSample();
	}
}
