using UnityEngine;

namespace KWJ.Map
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private MapPartDir _mapPartDir = MapPartDir.None;
        public MapPartDir MapPartDir => _mapPartDir;
        public RoomManager RoomManager { get; private set; }

        public void Initialize(RoomManager roomManager)
        {
            RoomManager = roomManager;
        }
        
        //어떤 방이랑 연결되어있는 지 알기위해 Exit를 통해 RoomManager에 ConnectRooms에 넣어준다.
        public void AddRoomManager(RoomManager connectRoom)
        {
            RoomManager.ConnectRooms.Add(connectRoom); 
        }
        
        public void AddHallway(Hallway hallway)
        {
            RoomManager.AddHallway(hallway); 
            RoomManager.MinimapMarker.AddMark(hallway.HallwayMark);
        }
    }
}