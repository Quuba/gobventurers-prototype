using System.Collections.Generic;
using UnityEngine;

namespace Rooms.Scripts
{
    public class RoomGraphData : ScriptableObject
    {
        public RoomNode CreateExampleGraph()
        {
            //recreation of this enter the gungeon diagram: https://boristhebrave.com/content/2019/07/F1_Castle_Flow_01.svg
            RoomNode startRoom = new RoomNode(DungeonRoomType.SpawnRoom);
            //loop1:
            var normal1 = startRoom.ConnectRoom(new RoomNode(DungeonRoomType.Normal));
            var hub1 = normal1.ConnectRoom(new RoomNode(DungeonRoomType.Hub));
            var normal2 = hub1.ConnectRoom(new RoomNode(DungeonRoomType.Normal));
            var reward1 = normal2.ConnectRoom(normal1); // TODO: one way passage
            reward1.ConnectRoom(normal1);
            
            //loop2:
            var normal3 = hub1.ConnectRoom(new RoomNode(DungeonRoomType.Normal));
            var preBoss = normal3.ConnectRoom(new RoomNode(DungeonRoomType.PreBoss));
            var boss = preBoss.ConnectRoom(new RoomNode(DungeonRoomType.Boss));
            
            //loop3:
            var hub2 = normal3.ConnectRoom(new RoomNode(DungeonRoomType.Hub));
            var normal4 = hub2.ConnectRoom(new RoomNode(DungeonRoomType.Normal));
            var normal5 = normal4.ConnectRoom(new RoomNode(DungeonRoomType.Normal));
            var reward2 = normal5.ConnectRoom(new RoomNode(DungeonRoomType.Reward));
            reward2.ConnectRoom(hub2);
            
            var normal6 = hub2.ConnectRoom(new RoomNode(DungeonRoomType.Normal));
            var shop = normal6.ConnectRoom(new RoomNode(DungeonRoomType.Shop));
            
            //root node
            return startRoom;
        }
        
        //TODO: create node based A* ???

        public struct RoomNode
        {
            public List<RoomNode> childNodes;
            public DungeonRoomType roomType;

            public RoomNode(DungeonRoomType roomType)
            {
                childNodes = new List<RoomNode>();
                this.roomType = roomType;
            }
            
            public RoomNode ConnectRoom(RoomNode node)
            {
                childNodes.Add(node);
                node.ConnectRoom(this);
                //passage type (one way / two way)
                //with passages I can treat it as a binary tree, maybe trinary
                //the catch is, there can be no loops with two way passage
                //I hate loops

                return node;
            }

            //Doesn't work (yet)
            public List<RoomNode> GetChildNodes()
            {
                List<RoomNode> foundNodes = new List<RoomNode>();
                for (var i = 0; i < childNodes.Count; i++)
                {
                    var childNode = childNodes[i];
                    if (childNode.childNodes.Count > 0)
                    {
                        var nodes = childNode.GetChildNodes();
                    }
                    else
                    {
                        i++;
                    }
                    
                }

                return foundNodes;
            }

        }

        public struct Passage
        {
            public RoomNode node1;
            public RoomNode node2;
            public bool oneWay;

            public Passage(RoomNode node1, RoomNode node2, bool oneWay)
            {
                this.node1 = node1;
                this.node2 = node2;
                this.oneWay = oneWay;
            }
        }
    }
}