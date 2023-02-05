using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Cutscene_Runner
{
	[Serializable]
	public struct CutsceneReferences
	{
		public GameObject master;
		public TMP_Text Text;
		public AudioSource AudioSource;
	}
}