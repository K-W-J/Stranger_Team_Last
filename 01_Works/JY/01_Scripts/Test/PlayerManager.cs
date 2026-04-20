using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Players;
using UnityEngine;

namespace _01_Works.JY._01_Scripts.Test
{
    [DefaultExecutionOrder(-1)]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private EntityFinderSO playerFinder;
        
        private void Awake()
        {
            playerFinder.SetTarget(player);
        }
    }
}
