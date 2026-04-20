using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using KWJ.MapObjects;
using KWJ.OverlapChecker;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace KWJ.Map
{
    public enum RoomType
    {
        None = -1,
        
        CommonRoom,
        StoreRoom,
        BossRoom, 
        
        Max,
    }
    public class RoomManager : MonoBehaviour
    {
        public UnityEvent OnRoomEnterEvent;
        public UnityEvent OnRoomClearEvent;
        
        [field:SerializeField] public RoomOverlapChecker RoomOverlapChecker { get; private set; }
        [field:SerializeField] public MinimapMarker MinimapMarker { get; private set; }
        [SerializeField] private GameObject _boxOverlapCheckerGroup;
        [field:SerializeField] public RoomType RoomType { get; private set; }
        
        [Space]
        
        [SerializeField] private int _exitCount;
        [SerializeField] private int _hallwayDistanceMax;
        [SerializeField] private int _hallwayDistanceMin;
        
        public List<RoomManager> ConnectRooms { get; set; } = new List<RoomManager>();
        public List<Exit> CanConnectExits { get; private set; } = new List<Exit>();
        public List<Exit> CannotConnectExits { get; private set; }
        
        private List<Hallway> _hallways = new List<Hallway>();
        
        private List<Door> _doors = new List<Door>();

        private BoxOverlapChecker[] _boxOverlapCheckers;

        [field:SerializeField] public Exit point { get; private set; }

        public Exit FirstConnectExit => _firstConnectExit;
        private Exit _firstConnectExit;
        
        public int HallwayDistance => _hallwayDistance;
        private int _hallwayDistance;

        private bool _isClearRoom;
        private bool _isEnterRoom;

        private bool _isComplateMapCreate;

        private void Update()
        {
            if(_isEnterRoom || _boxOverlapCheckers == null || !_isComplateMapCreate) return;

            foreach (var boxOverlapChecker in _boxOverlapCheckers)
            {
                if (boxOverlapChecker.BoxOverlapCheck())
                {
                    _isEnterRoom = true;
                    CloseDoor();
                }
            }
        }

        public void FindFirstConnectExit()
        { 
            if(point == null) return; 
            int rand = Random.Range(0, CanConnectExits.Count);
            _firstConnectExit = point;
            ChangeConnectExit(_firstConnectExit);
        }

        public void Initialize(Exit exit = null, float cellSize = 1f)
        {
            _boxOverlapCheckers = _boxOverlapCheckerGroup.GetComponentsInChildren<BoxOverlapChecker>();
            CannotConnectExits = GetComponentsInChildren<Exit>().ToList();

            foreach (var exits in CannotConnectExits)
            {
                exits.Initialize(this);
            }
            
            _hallwayDistance = Random.Range(_hallwayDistanceMin, _hallwayDistanceMax);
            
            if(exit == null) return;
            
            _firstConnectExit = CannotConnectExits.Find(e => e.MapPartDir == (MapPartDir)(-(int)exit.MapPartDir));
            MoveRoom(exit, cellSize);

            ChangeConnectExit(_firstConnectExit);
        }

        public void AfterInitialize(GameObject doorPrefab)
        {
            MinimapMarker.Initialize();

            _isComplateMapCreate = true;
            
            if (RoomType == RoomType.StoreRoom) return;

            foreach (var connectExit in CanConnectExits)
            {
                GameObject doorObject = Instantiate(doorPrefab, transform);
                Door door  = doorObject.GetComponentInChildren<Door>();
                
                doorObject.transform.position = connectExit.transform.position
                                           + -doorObject.transform.up * doorObject.transform.localScale.y;
                doorObject.transform.rotation = connectExit.transform.rotation;

                door.Particle.Stop();
                
                _doors.Add(door);
            }
        }

        public void MoveRoom(Exit exit, float cellSize = 1f)
        {
            if ((MapPartDir)(-(int)_firstConnectExit.MapPartDir) != exit.MapPartDir)
            {
                Debug.LogWarning("FindExit:값이 올바르지 않습니다.");
                return;
            }

            Vector3 targetConnExitPos = exit.transform.position + exit.transform.forward * (_hallwayDistance * cellSize);
            transform.position += targetConnExitPos - _firstConnectExit.transform.position;
        }

        //MapManager에서 수정된 Exit를 RoomManager에도 적용시켜줌
        public void ChangeConnectExit(Exit exit)
        {
            if(CanConnectExits.Contains(exit)) return;
            
            CanConnectExits.Add(exit);
            CannotConnectExits.Remove(exit);
        }
        
        public void ChangeNotConnectExit(Exit exit)
        {
            if(CannotConnectExits.Contains(exit)) return;
            
            CannotConnectExits.Add(exit);
            CanConnectExits.Remove(exit);
        }

        public void CreateExit()
        {
            int randCount = Random.Range(1, _exitCount);
            
            for (int i = 0; i < randCount; i++)
            {
                if(CannotConnectExits.Count == 0) break;
                
                int rand = Random.Range(0, CannotConnectExits.Count);
                
                Exit exit = CannotConnectExits[rand];
                CannotConnectExits.Remove(exit);
                CanConnectExits.Add(exit);
            }
        }

        public void AddHallway(Hallway hallway)
        {
            hallway.transform.SetParent(transform);
            _hallways.Add(hallway);
        }

        public void CloseDoor()
        {
            OnRoomEnterEvent?.Invoke();
            
            foreach (var door in _doors)
            {
                door.Particle.Play();
                door.Collider.position += door.Collider.up * door.Collider.localScale.y;
                door.Visual.DOLocalMoveY(door.Visual.localScale.y, 3f)
                    .OnComplete(() => door.Particle.Stop());
            }
        }

        public void OpenDoor()
        {
            OnRoomClearEvent?.Invoke();
            
            foreach (var door in _doors)
            {
                door.Particle.Play();
                door.Collider.position -= door.Collider.up * door.Collider.localScale.y;
                door.Visual.DOLocalMoveY(0, 3f)
                    .OnComplete(() => door.Particle.Stop());
            }
        }
    }
}