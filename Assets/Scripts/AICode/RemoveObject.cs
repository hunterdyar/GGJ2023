using System.Collections;
using UnityEngine;

public class RemoveObject : MonoBehaviour
{
	public AnimationCurve ease;
	[SerializeField] private float animationDuration = 2f;
	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	public void Remove()
	{
		StartCoroutine(AnimateOut());
	}
	
	IEnumerator AnimateOut()
	{
		_spriteRenderer.sortingOrder = 10;//jump to top.
		_spriteRenderer.color = Color.white;
		Vector3 scale = transform.localScale;
		Vector3 scaleEnd = scale * 2;
		Color col = _spriteRenderer.color;
		Color colEnd = new Color(col.r, col.g, col.b, 0); 
		float t = 0;
		while (t < 1)
		{
			//opacity
			_spriteRenderer.color = Color.Lerp(col, colEnd, ease.Evaluate(t));
			transform.localScale = Vector3.Lerp(scale, scaleEnd, ease.Evaluate(t));
			t += Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject);
	}


}