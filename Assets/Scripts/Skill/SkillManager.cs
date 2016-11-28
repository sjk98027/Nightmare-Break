using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour { 
 
	SkillStorage skillStorage = new SkillStorage ();
	public SkillStorage SkillStorage  { get { return skillStorage; } }
	public static SkillManager instance = null;

	void Start()
	{
		instance = this;
		skillStorage.Initialize ();
	} 

}

