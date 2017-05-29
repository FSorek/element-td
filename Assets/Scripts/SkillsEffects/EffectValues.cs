using UnityEngine;


public class EffectValues
{
	[System.Serializable]
	public class Values{
		[Tooltip("Sl||P||B||St Duration//Knockback Force Back")]
		public float ValueA;
		[Tooltip("Slow Percentage//Poison||Burn Tick Rate//Knockback Force Up")]
		public float ValueB;
		[Tooltip("Poison||Burn Damage//Knockback Duration")]
		public float ValueC;
		[Tooltip("Poison HPreg % reduction")]
		public float ValueD;
	}
}