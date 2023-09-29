using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Aquapunk
{
    public class StructureInfoCard : ItemInfoCard
    {
        #region Fields
        public TextMeshProUGUI infoTimeBuid;
        #endregion

        #region Methods
        #region Class methods
        public void GetStructureInfo(StructureManager structure)
        {
            Apply(structure.structure);
            infoTimeBuid.text = structure.structure.timeBuild.ToString();
            if (structure != null)
            {
                applyItemButton.gameObject.SetActive(true);
                //applyItemButton.onClick.AddListener(() => playerInfo.player.SetStructure(structure));
            }
            else
            {
                applyItemButton.gameObject.SetActive(false);
            }
        }
        #endregion
        #endregion
    }

}
