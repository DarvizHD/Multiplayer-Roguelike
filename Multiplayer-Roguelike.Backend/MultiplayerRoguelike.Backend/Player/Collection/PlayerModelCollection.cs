using System.Linq;
using Backend.ModelCollections;
using ENet;

namespace Backend.Player.Collection
{
    public class PlayerModelCollection : ModelCollectionBase<string, PlayerModel>
    {
        public bool TryGet(Peer peer, out PlayerModel playerModel)
        {
            var player = Models.Values.FirstOrDefault(player => player.Peer.ID == peer.ID);
            if (player == null)
            {
                playerModel = null;
                return false;
            }
            
            playerModel = player;
            return true;
        }
    }
}