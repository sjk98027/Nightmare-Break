using UnityEngine;
using System.Collections;

public class SceneTest : MonoBehaviour {

	public void SceneEvent()
	{
		SceneChanger.Instance.SceneChange (SceneChanger.SceneName.InGameScene);
	}
}
