using UnityEngine;

namespace KWJ.Map
{
    public enum MapPartDir
    {
        None = -100,
        
        North = 1,
        East = 2,
        South = -1,
        West = -2,
        
        Max,
    }
    
    public class Hallway : MonoBehaviour
    {
        [field: SerializeField] public GameObject HallwayMark { get; private set; }
        [SerializeField] private FloorChecker floorChecker;
        
        private GameObject _wallPrefab;
        private Exit[] _wallPoints = new Exit[4];
        public Exit ConnectionExit => _connectionExit;
        private Exit _connectionExit;

        public void Initialize(MapPartDir mapPartDir, GameObject wallPrefab)
        {
            _wallPoints = GetComponentsInChildren<Exit>();
            _wallPrefab = wallPrefab;

            foreach (var wallPoint in _wallPoints)
            {
                if (wallPoint.MapPartDir == (MapPartDir)(-(int)mapPartDir))
                {
                    _connectionExit = wallPoint;
                    break;
                }
            }
            
        }

        public void AfterInitialize()
        {
            foreach (var wallPoint in _wallPoints)
            {
                if (floorChecker.FloorCheck(wallPoint.transform)) continue;
                
                GameObject wall = Instantiate(_wallPrefab, wallPoint.transform, true);
                
                wall.transform.position = wallPoint.transform.position;
                wall.transform.rotation = wallPoint.transform.rotation;
            }
        }
    }
}
