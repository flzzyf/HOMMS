using System.Collections;
using UnityEngine;

public class TravelCamMgr : Singleton<TravelCamMgr>
{
    public GameObject cam;

    //移动镜头到目的地
    public void MoveCamera(Vector3 _pos)
    {
        _pos.y = cam.transform.position.y;
        cam.transform.position = _pos;
    }

	public void FocusTarget(Transform _target)
	{
		StartCoroutine(Focus(_target));
	}

	public void StopFocus()
	{
		StopAllCoroutines();
	}

	IEnumerator Focus(Transform _target)
	{
		while(true)
		{
			MoveCamera(_target.position);

			yield return null;
		}
	}
}
