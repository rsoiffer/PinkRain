using System.Collections.Generic;
using System.Linq;
using PinkRain.Utility;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PinkRain.Component
{
    public class Procgen : MonoBehaviour
    {
        public Tile? wall;
        public Tile? grass;

        public Tilemap? tilemap;

        public GameObject? enemyPrefab;

        void Start()
        {
            Requires.NotNull(enemyPrefab, nameof(enemyPrefab));

            var baseRoom = new Room
            {
                lowerLeft = new Vector2Int(0, 0),
                upperRight = new Vector2Int(100, 100),
            };
            var allRooms = Subdivide(baseRoom.Expand(-3)).ToList();
            allRooms = allRooms.Where(r => r.hallway || Random.value < .9f).ToList();

            var doors = new List<Room>();
            for (int i = 0; i < allRooms.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (AddDoor(allRooms[i], allRooms[j], doors))
                    {
                        allRooms[i].neighbors.Add(allRooms[j]);
                        allRooms[j].neighbors.Add(allRooms[i]);
                    }
                }
            }

            allRooms = Component(allRooms.Find(r => r.hallway)).ToList();

            FillRoom(baseRoom, wall);

            foreach (var r in allRooms)
            {
                FillRoom(r, null);

                if (!r.hallway && Random.value < .5f)
                {
                    var numEnemies = Random.Range(1, 8);
                    for (int i = 0; i < numEnemies; i++)
                    {
                        var pos = new Vector2(
                            Random.Range(r.lowerLeft.x, r.upperRight.x + 1),
                            Random.Range(r.lowerLeft.y, r.upperRight.y + 1));
                        var enemy = Instantiate(enemyPrefab);
                        enemy.transform.position = pos + 0.5f * Vector2.one;
                    }
                }
            }

            foreach (var door in doors)
            {
                FillRoom(door, null);
            }
        }

        public HashSet<Room> Component(Room r)
        {
            var component = new HashSet<Room>();

            var toCheck = new Queue<Room>();
            toCheck.Enqueue(r);
            while (toCheck.Count > 0)
            {
                var c = toCheck.Dequeue();
                if (!component.Add(c))
                {
                    continue;
                }

                foreach (var n in c.neighbors)
                {
                    toCheck.Enqueue(n);
                }
            }

            return component;
        }

        public bool AddDoor(Room r1, Room r2, List<Room> doors)
        {
            var inter = r1.Expand(1).Intersect(r2.Expand(1));
            if (inter.area == 0)
            {
                return false;
            }

            var horWall = inter.size.x > inter.size.y;
            inter = inter.Expand(horWall ? new Vector2Int(-1, 0) : new Vector2Int(0, -1));
            if (r1.hallway && r2.hallway)
            {
                doors.Add(inter);
                return true;
            }

            if (inter.area < 4 || !(r1.hallway || r2.hallway || Random.value < .3f))
            {
                return false;
            }

            if (horWall)
            {
                var doorPos = Random.Range(inter.lowerLeft.x + 1, inter.upperRight.x - 1);
                doors.Add(inter.SliceX(doorPos, doorPos + 1));
            }
            else
            {
                var doorPos = Random.Range(inter.lowerLeft.y + 1, inter.upperRight.y - 1);
                doors.Add(inter.SliceY(doorPos, doorPos + 1));
            }

            return true;
        }

        public void FillRoom(Room r, Tile? t)
        {
            Requires.NotNull(tilemap, nameof(tilemap));

            for (int x = r.lowerLeft.x; x <= r.upperRight.x; x++)
            {
                for (int y = r.lowerLeft.y; y <= r.upperRight.y; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), t);
                }
            }
        }

        public IEnumerable<Room> Subdivide(Room r)
        {
            if (r.area < 50 + 200 * Random.value) yield return r;
            else
            {
                var splitH = r.size.x > r.size.y;
                if (splitH)
                {
                    if (r.size.x > 40)
                    {
                        var splitPos = Random.Range(r.lowerLeft.x + 12, r.upperRight.x - 11);
                        var rLeft = r.SliceX(r.lowerLeft.x, splitPos - 3);
                        var rCenter = r.SliceX(splitPos - 1, splitPos + 1);
                        rCenter.hallway = true;
                        var rRight = r.SliceX(splitPos + 3, r.upperRight.x);

                        foreach (var r2 in Subdivide(rLeft)) yield return r2;
                        yield return rCenter;
                        foreach (var r2 in Subdivide(rRight)) yield return r2;
                    }
                    else
                    {
                        var splitPos = Random.Range(r.lowerLeft.x + 5, r.upperRight.x - 4);
                        var rLeft = r.SliceX(r.lowerLeft.x, splitPos - 1);
                        var rRight = r.SliceX(splitPos + 1, r.upperRight.x);

                        foreach (var r2 in Subdivide(rLeft)) yield return r2;
                        foreach (var r2 in Subdivide(rRight)) yield return r2;
                    }
                }
                else
                {
                    if (r.size.y > 40)
                    {
                        var splitPos = Random.Range(r.lowerLeft.y + 12, r.upperRight.y - 11);
                        var rLeft = r.SliceY(r.lowerLeft.y, splitPos - 3);
                        var rCenter = r.SliceY(splitPos - 1, splitPos + 1);
                        rCenter.hallway = true;
                        var rRight = r.SliceY(splitPos + 3, r.upperRight.y);

                        foreach (var r2 in Subdivide(rLeft)) yield return r2;
                        yield return rCenter;
                        foreach (var r2 in Subdivide(rRight)) yield return r2;
                    }
                    else
                    {
                        var splitPos = Random.Range(r.lowerLeft.y + 5, r.upperRight.y - 4);
                        var rLeft = r.SliceY(r.lowerLeft.y, splitPos - 1);
                        var rRight = r.SliceY(splitPos + 1, r.upperRight.y);

                        foreach (var r2 in Subdivide(rLeft)) yield return r2;
                        foreach (var r2 in Subdivide(rRight)) yield return r2;
                    }
                }
            }
        }

        public class Room
        {
            public Vector2Int lowerLeft, upperRight;
            public bool hallway;
            public List<Room> neighbors = new List<Room>();

            public Room Expand(int amt)
            {
                return Expand(new Vector2Int(amt, amt));
            }

            public Room Expand(Vector2Int amt)
            {
                return new Room
                {
                    lowerLeft = lowerLeft - amt,
                    upperRight = upperRight + amt,
                };
            }

            public Room Intersect(Room other)
            {
                return new Room
                {
                    lowerLeft = Vector2Int.Max(lowerLeft, other.lowerLeft),
                    upperRight = Vector2Int.Min(upperRight, other.upperRight),
                };
            }

            public Room SliceX(int min, int max)
            {
                return new Room
                {
                    lowerLeft = new Vector2Int(min, lowerLeft.y),
                    upperRight = new Vector2Int(max, upperRight.y),
                };
            }

            public Room SliceY(int min, int max)
            {
                return new Room
                {
                    lowerLeft = new Vector2Int(lowerLeft.x, min),
                    upperRight = new Vector2Int(upperRight.x, max),
                };
            }

            public Vector2Int size => Vector2Int.Max(upperRight - lowerLeft + new Vector2Int(1, 1), Vector2Int.zero);
            public int area => size.x * size.y;
        }
    }
}