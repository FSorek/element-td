using UnityEngine;
using System.Collections;

public interface IUpgradeable {
	void ApplyUpgrade(UpgradeBase upgrade, int lvToApply);
}
