using System;
using System.Collections.Generic;

namespace Backend.Lobby
{
    public class LobbyModel
    {
        public event Action<string> OnMemberAdded;
        public event Action<string> OnMemberRemoved;

        public string Guid { get; }
        public string OwnerNickname { get; }

        public readonly List<string> Members = new();

        public LobbyModel(string guid, string ownerNickname)
        {
            Guid = guid;
            OwnerNickname = ownerNickname;
        }

        public void AddMember(string memberNickname)
        {
            Members.Add(memberNickname);

            OnMemberAdded?.Invoke(memberNickname);
        }

        public void RemoveMember(string memberNickname)
        {
            Members.Remove(memberNickname);

            OnMemberRemoved?.Invoke(memberNickname);
        }
    }
}
