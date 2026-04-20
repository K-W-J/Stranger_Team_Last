using System.Collections.Generic;
using UnityEngine;

namespace KWJ.Probability
{
    public class PickRandomObject
    {
        protected System.Random m_Seed;
        
        public PickRandomObject(System.Random seed)
        {
            m_Seed = seed;  
        }

        public int RandomRange(int randomCount)
        {
            return m_Seed.Next(0,randomCount);
        }
        
        public GameObject PickRandomObjectInGroup(List<GameObject> gameObject)
        {
            int random = RandomRange(gameObject.Count);
            
            return gameObject[random];
        }
    }
}