using System.Collections.Generic;
using UnityEngine;

namespace KWJ.Map
{
    public class RoomOverlapChecker : MonoBehaviour
    {
        [SerializeField] private GameObject _roomCheckDataGroup;
        [SerializeField] private GameObject _visual;
        
        private List<RoomOverlapCheckData> _roomCheckDatas = new List<RoomOverlapCheckData>();
        private List<RoomManager> _roomConnectList;

        private void Awake()
        {
            _roomCheckDataGroup.GetComponentsInChildren(_roomCheckDatas);
        }
        
        public bool RoomCheck()
        {
            _visual.SetActive(false);
            
            bool isOverlap = false;

            foreach (var roomCheckData in _roomCheckDatas)
            {
                if(isOverlap) break;
                
                isOverlap = Physics.CheckBox(roomCheckData.RoomCheckPos,
                    roomCheckData.BoxSize * 0.5f);
            }

            _visual.SetActive(true);
            
            return isOverlap;
        }
    }
}