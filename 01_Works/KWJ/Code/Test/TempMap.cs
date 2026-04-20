namespace KWJ.Test
{
    public class TempMap
    {
        //169줄 MapManager
        
        //주변에 있는 방이랑 연결
        /*List<RoomManager> connectableRooms
            = roomManager.RoomOverlapChecker.RoomConnectCheck(roomManager.ConnectRooms);

        if (connectableRooms.Count > 0)
        {
            foreach (var connectableRoom in connectableRooms)
            {
                float distance = 0;
                Exit connectableExitTemp = null;

                foreach (var connectableExit in connectableRoom.CannotConnectExits)
                {
                    if (connectableExitTemp == null)
                        connectableExitTemp = roomManager.CannotConnectExits[0];

                    float distanceTemp = Vector3.Distance(connectableExit.transform.position,
                        connectableExitTemp.transform.position);

                    if (distance < distanceTemp)
                    {
                        distance = distanceTemp;
                        connectableExitTemp = connectableExit;
                    }
                }

                roomManager.ConnectRooms.Add(connectableExitTemp.RoomManager);
                connectableExitTemp.AddRoomManager(roomManager);

                _notConnectExits.Remove(connectableExitTemp);
                _notConnectExits.Remove(roomManager.CannotConnectExits[0]);
            }
        }*/
        
        
        // 19줄 RoomOverlapChecker
        
        //[SerializeField] private Collider _roomConnectCollider;
        
        /*public List<RoomManager> RoomConnectCheck(List<RoomManager> roomManagers)
        {
            Collider[] rooms = Physics.OverlapBox(_roomConnectCollider.transform.position,
                _roomConnectCollider.bounds.size * 0.5f);

            List<RoomManager> roomManagerList = new List<RoomManager>();

            RoomManager thisRoomManager = GetComponentInParent<RoomManager>();

            foreach (var room in rooms)
            {
                RoomManager roomManager = room.GetComponentInParent<RoomManager>();

                if(roomManager ==null || roomManagers.Contains(roomManager)
                  || roomManagerList.Contains(roomManager) || thisRoomManager == roomManager) continue;

                roomManagerList.Add(roomManager);
            }

            return roomManagerList;
        }*/
    }
}