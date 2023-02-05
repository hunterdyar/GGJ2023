using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Cutscene_Runner
{
	[Serializable]
	public struct CutsceneReferences
	{
		public GameObject master;
		public TMP_Text Text;
		public Image image;
		public AudioSource AudioSource;
	}
}