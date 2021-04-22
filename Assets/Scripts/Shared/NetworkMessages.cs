using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;

namespace DarkRiftRPG
{
    public enum Tags
    {
        JoinGameRequest,
        JoinGameResponse,

        SpawnLocalPlayerRequest,
        SpawnLocalPlayerResponse,

        PlayerMovementRequest,
        PlayerMovementUpdate,

        SpawnPlayer,
        DespawnPlayer

    }

    public struct JoinGameResponseData : IDarkRiftSerializable
    {
        public bool JoinGameRequestAccepted;

        public JoinGameResponseData(bool accepted)
        {
            JoinGameRequestAccepted = accepted;
        }
        public void Deserialize(DeserializeEvent e)
        {
            JoinGameRequestAccepted = e.Reader.ReadBoolean();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(JoinGameRequestAccepted);
        }
    }

    public struct SpawnLocalPlayerResponseData : IDarkRiftSerializable
    {
        public ushort ID;

        public SpawnLocalPlayerResponseData(ushort id)
        {
            ID = id;
        }

        public void Deserialize(DeserializeEvent e)
        {
            ID = e.Reader.ReadUInt16();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ID);
        }
    }

    public struct PlayerMovementRequestData : IDarkRiftSerializable
    {
        public Vector3 PlayerClickLocation;

        public PlayerMovementRequestData(Vector3 clickPos)
        {
            PlayerClickLocation = clickPos;
        }
        public void Deserialize(DeserializeEvent e)
        {
            PlayerClickLocation = e.Reader.ReadVector3();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.WriteVector3(PlayerClickLocation);
        }
    }

    public struct PlayerPositionInputData : IDarkRiftSerializable
    {
        public ushort ID;
        public Vector3 Pos;

        public PlayerPositionInputData(ushort id, Vector3 pos)
        {
            ID = id;
            Pos = pos;
        }

        public void Deserialize(DeserializeEvent e)
        {
            ID = e.Reader.ReadUInt16();
            Pos = e.Reader.ReadVector3();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ID);
            e.Writer.WriteVector3(Pos);
        }
    }

    public struct ProccessedPlayerMovementData : IDarkRiftSerializable
    {
        public PlayerPositionInputData[] ProccessedMovementUpdate;

        public ProccessedPlayerMovementData(PlayerPositionInputData[] newPlayerPositions)
        {
            ProccessedMovementUpdate = newPlayerPositions;
        }
        public void Deserialize(DeserializeEvent e)
        {
            ProccessedMovementUpdate = e.Reader.ReadSerializables<PlayerPositionInputData>();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ProccessedMovementUpdate);
        }
    }

    public struct PlayerSpawnData : IDarkRiftSerializable
    {
        public ushort ID;
        public Vector3 Position;

        public PlayerSpawnData(ushort id, Vector3 position)
        {
            ID = id;
            Position = position;
        }

        public void Deserialize(DeserializeEvent e)
        {
            ID = e.Reader.ReadUInt16();
            Position = e.Reader.ReadVector3();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ID);
            e.Writer.WriteVector3(Position);
        }
    }

    public struct PlayerDespawnData : IDarkRiftSerializable
    {
        public ushort ID;

        public PlayerDespawnData(ushort id)
        {
            ID = id;
        }

        public void Deserialize(DeserializeEvent e)
        {
            ID = e.Reader.ReadUInt16();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ID);
        }
    }
}
