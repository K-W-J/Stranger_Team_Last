using System.Collections.Generic;
using UnityEngine;

namespace KWJ.SO
{
    [CreateAssetMenu(fileName = "MapPartSO", menuName = "SO/RoomGroupSO", order = 0)]
    public class MapPartSO : ScriptableObject
    {
        public GameObject WallPrefab;
        public GameObject DoorPrefab;
        public GameObject HallwayPrefab;

        [Header("StartRoom")] public GameObject StartRoom;
        
        [Header("RequiredRooms")] 
        public List<GameObject> RequiredRooms = new List<GameObject>();
        
        [Header("Commone")] 
        public List<GameObject> CommonObject = new List<GameObject>();
        
        [Header("Uncommon")]
        public int UncommonProbability;
        public List<GameObject> UncommonObject = new List<GameObject>();
        public bool UseUncommon;
        
        [Header("Rare")]
        public int RareProbability;
        public List<GameObject> RareObject = new List<GameObject>();
        public bool UseRare;
        
        [Header("Epic")]
        public int EpicProbability;
        public List<GameObject> EpicObject = new List<GameObject>();
        public bool UseEpic;
        
        [Header("Legendary")]
        public int LegendaryProbability;
        public List<GameObject> LegendaryObject = new List<GameObject>();
        public bool UseLegendary;

        private void OnValidate()
        {
                
            if(LegendaryProbability >= EpicProbability)
                LegendaryProbability = EpicProbability - 1;
            
            if(EpicProbability >= RareProbability)
                EpicProbability = RareProbability - 1;
            
            if(UncommonProbability < RareProbability)
                UncommonProbability = RareProbability + 1;
            
            if(RareProbability >= UncommonProbability)
                RareProbability = UncommonProbability - 1;

            if (LegendaryProbability < 0)
                LegendaryProbability = 0;
            
            if (EpicProbability < 0)
                EpicProbability = 0;
            
            if (RareProbability < 0)
                RareProbability = 0;
            
        }
    }
}