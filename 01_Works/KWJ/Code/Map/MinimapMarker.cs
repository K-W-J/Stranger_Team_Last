using System.Collections.Generic;
using UnityEngine;

namespace KWJ.Map
{
    public class MinimapMarker : MonoBehaviour
    {
        [SerializeField] private GameObject _minimapMarkGroup;
        
        private List<GameObject> _minimapMarks = new List<GameObject>();
        
        public void Initialize()
        {
            foreach (Transform child in _minimapMarkGroup.transform)
            {
                _minimapMarks.Add(child.gameObject);
            }

            HideMark();
        }

        public void AddMark(GameObject mark)
        {
            _minimapMarks.Add(mark);
        }

        public void HideMark()
        {
            foreach (var markObject in _minimapMarks)
            {
                markObject.SetActive(false);
            }
        }
        public void DisplayMark()
        {
            foreach (var markObject in _minimapMarks)
            {
                markObject.SetActive(true);
            }
        }
        
    }
}