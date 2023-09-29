using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class StructureManager : MonoBehaviour
    {
        #region Fields
        public Structure structure;
        public Player owner;

        public GameObject structureHologramModel;
        public GameObject structureObject;
        #endregion

        #region Methods
        #region Class Methods
        public void SpawnStructureHologram(GameObject plane)
        {
            BuildObject(plane, structureHologramModel);
        }

        public void BuildStructure(GameObject plane)
        {
            BuildObject(plane, structureObject);
        }

        
        private void BuildObject(GameObject plane, GameObject obj)
        {
            Transform structureTransform = Instantiate(obj, plane.transform).transform;

            //structureTransform.position = plane.transform.position;
            //structureTransform.SetParent(plane.transform);

            plane.GetComponent<PlaneLandscape>().structureTransform = structureTransform;
            if (structureTransform.GetComponent<StructureObject>())
            {
                structureTransform.GetComponent<StructureObject>().structure = structure;
                structureTransform.GetComponent<StructureObject>().Build();
            }
        }
        #endregion
        #endregion
    }
}