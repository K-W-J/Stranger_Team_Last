using System.Collections.Generic;
using System.Collections;
using System.Linq;
using KWJ.Probability;
using KWJ.SO;
using Unity.AI.Navigation;
using UnityEngine.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KWJ.Map
{
    public class MapManager : MonoBehaviour
    {
        public UnityEvent CompleteMapCreate;
        
        [SerializeField] private NavMeshSurface navMeshSurface;
        [SerializeField] private MapPartSO mapPartGroupSO;
        
        [SerializeField] private int roomCount;
        [SerializeField] private float cellSize;
        [SerializeField] private float createDelay; 
        //딜레이가 없으면 복도에서 겹침을 확인할 시간이 없어서 실질적으로 길이여도 벽이 생긴다.
        
        private List<RoomManager> _mapPartManagers = new List<RoomManager>();
        private List<Exit> _cannotConnectExits = new List<Exit>();
        private List<Exit> _canConnectExits = new List<Exit>();
        
        private Queue<RoomManager> _requiredRooms = new Queue<RoomManager>();
        
        private PickRandomObjectGrade _probability;

        private bool _isRequiredPlacementPhase;
        private void Awake()
        {
            if(roomCount == 0)
                roomCount = Random.Range(3, 10);

            if (_probability == null)
            {
                _probability = new PickRandomObjectGrade(new System.Random(2));
            }
        }

        private void Start()
        {
            CreateMap();
        }

        private void ResetMap()
        {
            foreach(var room in _mapPartManagers)
            {
                Destroy(room.gameObject);
            }

            foreach (var requiredRoom in _requiredRooms)
            {
                Destroy(requiredRoom.gameObject);
            }
            
            _mapPartManagers.Clear();
            _cannotConnectExits.Clear();
            _canConnectExits.Clear();
            _requiredRooms.Clear();

            //필수 방을 미리 만들어 큐에 저장
            foreach (var importanceRoom in mapPartGroupSO.RequiredRooms)
            {
                GameObject room = Instantiate(importanceRoom, transform);
                
                RoomManager roomManager = room.GetComponentInChildren<RoomManager>();
                roomManager.gameObject.SetActive(false);
                _requiredRooms.Enqueue(roomManager);
            }
        }

        [ContextMenu("Create Rooms")]
        private void CreateMap()
        {
            ResetMap();
            StartCoroutine(CreateRooms());
        }

        private RoomManager GetRoomManager()
        {
            float rand = 0;
            
            //남은 필수 방의 개수가 현재 생성될 수 있는 방의 개수와 같다면 이 단계부터는 필수 방만 배치
            //(필수 방은 무조건적으로 맵에 있어야하기 때문)
            if (roomCount == _mapPartManagers.Count + _requiredRooms.Count)
                _isRequiredPlacementPhase = true;
            else
                rand = Random.value;
            
            if (_requiredRooms.Count > 0 && (rand > 0.8 || _isRequiredPlacementPhase))
            {
                RoomManager roomManager = _requiredRooms.Dequeue();
                roomManager.gameObject.SetActive(true);

                if (roomManager.RoomType == RoomType.BossRoom)
                {
                    if(roomCount <= _mapPartManagers.Count + 2)
                        return roomManager;
                    
                    roomManager.gameObject.SetActive(false);
                    _requiredRooms.Enqueue(roomManager);
                }
                else
                    return roomManager;
            }
            
            GameObject room = Instantiate(_probability.PickRandomObject(mapPartGroupSO), transform);
            return room.GetComponentInChildren<RoomManager>();
        }

        private IEnumerator CreateRooms()
        {
            List<Exit> exits = new List<Exit>(); 
            List<Exit> exitsTemp = new List<Exit>(); 
            
            GameObject startRoom = Instantiate(mapPartGroupSO.StartRoom, transform);
            RoomManager startRoomManager = startRoom.GetComponentInChildren<RoomManager>();
            
            startRoomManager.Initialize();
            startRoomManager.CreateExit();
            
            exits.AddRange(startRoomManager.CanConnectExits);
            _canConnectExits.AddRange(startRoomManager.CanConnectExits);
            _cannotConnectExits.AddRange(startRoomManager.CannotConnectExits);
            
            while (roomCount > _mapPartManagers.Count)
            {
                //현재 맵 생성이 가능한 포인트(exit)을 모두 찾기
                foreach (var exit in exits)
                {
                    yield return new WaitForSeconds(createDelay);
                    
                    //생성되어야하는 방의 갯수에 도달했다면 나머지 exit 모두 _cannotConnectExits에 넣기
                    if (roomCount <= _mapPartManagers.Count)
                    {
                        ChangeConnectExits(exit, false);
                        continue;
                    }
                    
                    RoomManager roomManager = GetRoomManager();

                    if (roomManager.RoomType == RoomType.BossRoom)
                    {
                        roomManager.Initialize();
                        roomManager.CreateExit();
                        roomManager.FindFirstConnectExit();
                    }
                    else
                    {
                        //각각의 방 확률에 따라 생성
                        roomManager.Initialize(exit, cellSize);
                        roomManager.CreateExit();
                    }
                    
                    if (roomManager.RoomOverlapChecker.RoomCheck())
                    {
                        print("겹침");

                        ChangeConnectExits(exit, false);
                        
                        MapPartDir mapPartDirTemp = (MapPartDir)(-(int)roomManager.FirstConnectExit.MapPartDir);
            
                        var findExits = _cannotConnectExits.FindAll(e => e.MapPartDir == mapPartDirTemp);
                        findExits.AddRange(_canConnectExits.FindAll(e => e.MapPartDir == mapPartDirTemp));
                
                        //방이 안겹치는 Exit을 찾을 때까지 foreach문을 돈다.
                        foreach (var findExit in findExits)
                        {
                            yield return new WaitForSeconds(createDelay);
                            
                            roomManager.MoveRoom(findExit, cellSize);
                
                            if (!roomManager.RoomOverlapChecker.RoomCheck())
                            {
                                print("겹침 해결");
                                StartCoroutine(CreateHallway(findExit,
                                    mapPartGroupSO.HallwayPrefab, roomManager.HallwayDistance));
                    
                                //RoomManager의 ConnectRooms 리스트에 서로 연결된 방을 추가해준다.
                                roomManager.ConnectRooms.Add(findExit.RoomManager);
                                findExit.AddRoomManager(roomManager);
                        
                                //findExit가 NotConnectExit에 있으면 ConnectExit에 넣어줌
                                ChangeConnectExits(findExit, true);
                        
                                break;
                            }
                        }

                        if (roomManager.RoomOverlapChecker.RoomCheck())
                        {
                            roomManager.MoveRoom(findExits[^1], cellSize);
                            
                            StartCoroutine(CreateHallway(findExits[^1],
                                mapPartGroupSO.HallwayPrefab, roomManager.HallwayDistance));
                    
                            //RoomManager의 ConnectRooms 리스트에 서로 연결된 방을 추가해준다.
                            roomManager.ConnectRooms.Add(findExits[^1].RoomManager);
                            findExits[^1].AddRoomManager(roomManager);
                        
                            //findExit가 NotConnectExit에 있으면 ConnectExit에 넣어줌
                            ChangeConnectExits(findExits[^1], true);
                        }
                    }
                    else
                    {
                        if(roomManager.RoomType == RoomType.BossRoom)
                            roomManager.MoveRoom(exit, cellSize);
                            
                        //RoomManager의 ConnectRooms 리스트에 서로 연결된 방을 추가해준다.
                        roomManager.ConnectRooms.Add(exit.RoomManager);
                        exit.AddRoomManager(roomManager);
                        
                        StartCoroutine(CreateHallway(exit,
                            mapPartGroupSO.HallwayPrefab, roomManager.HallwayDistance));
                    }
                    
                    List<Exit> connectExits = roomManager.CanConnectExits.FindAll(e => e != roomManager.FirstConnectExit);

                    exitsTemp.AddRange(connectExits);
                    _canConnectExits.AddRange(connectExits);
                    
                    _cannotConnectExits.AddRange(roomManager.CannotConnectExits);
                    _mapPartManagers.Add(roomManager);
                }

                if (roomCount <= _mapPartManagers.Count)
                {
                    exitsTemp.ForEach(e => ChangeConnectExits(e, false));
                }

                exits.Clear();
                exits = exitsTemp.ToList();
                exitsTemp.Clear();
            }
            
            CreateWall(mapPartGroupSO.WallPrefab);

            foreach (var mapPart in _mapPartManagers)
            {
                mapPart.AfterInitialize(mapPartGroupSO.DoorPrefab);
            }
            
            //네비 베이크
            navMeshSurface.BuildNavMesh();
            
            CompleteMapCreate?.Invoke();
        }
        
        private void ChangeConnectExits(Exit exit, bool canConnect)
        {
            if (canConnect)
            {
                _canConnectExits.Add(exit);
                _cannotConnectExits.Remove(exit);
                
                exit.RoomManager.ChangeConnectExit(exit);
            }
            else
            {
                _canConnectExits.Remove(exit);
                _cannotConnectExits.Add(exit);
                
                exit.RoomManager.ChangeNotConnectExit(exit);
            }
        }

        private void CreateWall(GameObject wallPrefab)
        {
            foreach (var wall in _cannotConnectExits)
            {
                GameObject wallObject = Instantiate(wallPrefab, wall.RoomManager.transform);
                wallObject.transform.position = wall.transform.position;
                wallObject.transform.rotation = wall.transform.rotation;
            }
        }
        
        private IEnumerator CreateHallway(Exit exit, GameObject hallwayPrefab, int hallwayDistance)
        {
            Hallway[] hallways = new Hallway[hallwayDistance];
            
            for (int i = 0; i < hallwayDistance; i++)
            {
                GameObject hallwayObject = Instantiate(hallwayPrefab, transform);
                Hallway hallway = hallwayObject.GetComponent<Hallway>();
                
                hallway.Initialize(exit.MapPartDir, mapPartGroupSO.WallPrefab);
                hallways[i] = hallway;
                
                hallway.transform.position += exit.transform.position - hallway.ConnectionExit.transform.position 
                                              + exit.transform.forward * (cellSize * i);
                
                exit.AddHallway(hallway);
            }
            
            foreach (var hallway in hallways)
            {
                yield return new WaitForSeconds(createDelay);
                hallway.AfterInitialize();
            }
        }
    }
}
