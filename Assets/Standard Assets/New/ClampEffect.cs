using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class ClampEffect : ImageEffectBase
{
	[Range(0f, 20f)]
	public float cutoff = 3f;
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Cutoff", cutoff);
		Graphics.Blit(source, destination, material);
	}
}