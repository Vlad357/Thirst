using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class PlaneLandscape : MonoBehaviour
    {
        public PlayerInfo playerInfo;
        public Transform structureTransform;
        public bool freePlane = true;

        private void ClearStructureTransform()
        {
            if (structureTransform != null)
            {
                Destroy(structureTransform.gameObject);
            }
        }

        //private void BuildStructure()
        //{
        //    freePlane = false;
        //    ClearStructureTransform();
        //    playerInfo.player.BuildStructure(gameObject);
        //}
        //private void OnMouseEnter()
        //{
        //    if (playerInfo.player != null && playerInfo.player.structureBuilding != null && playerInfo.player.buildMod && freePlane)
        //    {
        //        playerInfo.player.structureBuilding.SpawnStructureHologram(gameObject);
        //    }
        //}
        //
        //private void OnMouseExit()
        //{
        //    if (playerInfo.player != null && playerInfo.player.structureBuilding != null && playerInfo.player.buildMod && freePlane)
        //    {
        //        ClearStructureTransform();
        //    }
        //}
        //
        //private void OnMouseDown()
        //{
        //    if (playerInfo.player != null && playerInfo.player.structureBuilding != null && playerInfo.player.buildMod && freePlane)
        //    {
        //        BuildStructure();
        //    }
        //}
        //
        //private void OnMouseOver()
        //{
        //    if(structureTransform != null && !playerInfo.player.buildMod && !structureTransform.GetComponent<StructureObject>())
        //    {
        //        ClearStructureTransform();
        //    }
        //}
        //
        //private void Start()
        //{
        //    playerInfo = FindObjectOfType<PlayerInfo>();
        //}
    }
}