using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using Godot;

namespace y1000.code.world
{
    public class MapManager
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Header
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string Magic;
            public int BlockSize;
            public int Width;
            public int Height;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MapCell
        {
            /*TileId : word;
    TileNumber: byte;
    TileOverId: word;
    TileOverNumber: byte;
    ObjectId: word;
    ObjectNumber: byte;
    RoofId: word;
    boMove: byte;*/
            public short TileId;
            public byte TileNumber;
            public short TileOverId;
            public byte TileOverNumber;
            public short ObjectId;
            public byte ObjectNumber;
            public short RoofId;
            public byte BoMove;
        }

        public const int OFFSET = 28 + 20;
        public const int BLOCK_SIZE = 40;
        public const int MAPCELL_SIZE = 12;


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct MapBlockData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string MapBlockIdent;
            public int MapChangedCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1600)]
            public MapCell[] MapCells;
        }

        public readonly string _mapFilePath;

        private readonly MapCell[,] _cells;


        private readonly int _height;
        private readonly int _width;

        private ISet<int>? ids;

        private ISet<int>? objectIds;
        public IEnumerable<int> ObjectIds
        {
            get
            {
                if (objectIds != null)
                {
                    return objectIds;
                }
                objectIds = new HashSet<int>();
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        if (!objectIds.Contains(_cells[j, i].ObjectId))
                        {
                            objectIds.Add(_cells[j, i].ObjectId);
                        }
                    }
                }
                return objectIds;
            }
        }

        public IEnumerable<int> TileIds
        {
            get
            {
                if (ids != null)
                {
                    return ids;
                }
                ids = new HashSet<int>();
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        if (!ids.Contains(_cells[j, i].TileId))
                        {
                            ids.Add(_cells[j, i].TileId);
                        }
                        if (!ids.Contains(_cells[j, i].TileOverId))
                        {
                            ids.Add(_cells[j, i].TileOverId);
                        }
                    }
                }
                return ids;
            }
        }

        //private readonly ISet<Point> _blockedCoordinates = new HashSet<Point>();

        private readonly IDictionary<Point, byte> _blocked = new ConcurrentDictionary<Point, byte>();

        private readonly ISet<Point> _forbidenCoordinates = new HashSet<Point>();
        // Any coordinate left to or above this point is blocked.
        private Point? _blockedTopLeft;

        private Point? _blockedBottomRight;

        public MapManager(string path, MapCell[,] coor, int heigh, int width)
        {
            _mapFilePath = path;
            _cells = coor;
            _height = heigh;
            _width = width;
        }

        public MapCell? GetCell(int x, int y)
        {
            if (x >= Width || x < 0 || y >= Height || y < 0)
            {
                return null;
            }
            return _cells[y, x];
        }


        private static readonly List<Point> NEIGHBORS_OFFSET = new() {
            new Point(-1, -1), // Up left
            new Point(-1, 0),  // Up
            new Point(-1, 1),  // Up right
            new Point(0, 1),   // Right
            new Point(1, 1),   // Down right
            new Point(1, 0),   // Down
            new Point(1, -1),  // Down left
            new Point(0, -1)   // Left
        };


        public void BlockRange(Point? topLeft, Point? bottomRight)
        {
            _blockedTopLeft = topLeft;
            _blockedBottomRight = bottomRight;
        }


        public string MapDirectory
        {
            get
            {
                var path = Path.GetDirectoryName(_mapFilePath);
                return path ?? "";
            }
        }

        public void UnblockRange()
        {
            _blockedTopLeft = null;
            _blockedBottomRight = null;
        }

        public string Name => Path.GetFileName(_mapFilePath);


        public bool IsMovable(Point point)
        {
            if (_blockedTopLeft.HasValue &&
                (_blockedTopLeft.Value.X >= point.X || _blockedTopLeft.Value.Y >= point.Y))
            {
                return false;
            }
            if (_blockedBottomRight.HasValue &&
                (_blockedBottomRight.Value.X <= point.X || _blockedBottomRight.Value.Y <= point.Y))
            {
                return false;
            }
            if (_blocked.ContainsKey(point) || _forbidenCoordinates.Contains(point))
            {
                return false;
            }
            if (point.X >= _width || point.X < 0 || point.Y >= _height || point.Y < 0)
            {
                return false;
            }
            return true;
        }

        public void BlockCoordinates(IEnumerable<Point> blockedPoints)
        {
            if (blockedPoints != null)
            {
                foreach (var p in blockedPoints)
                {
                    _blocked.TryAdd(p, (byte)1);
                }
            }
        }

        /**
         * To make path smoother, some coordinates are easily got blocked by monsters, so just forbit them.
         */
        public void Forbit(IEnumerable<Point> blockedPoints)
        {
            if (blockedPoints != null)
            {
                foreach (var p in blockedPoints)
                    _forbidenCoordinates.Add(p);
            }
        }

        public int Height => _height;
        public int Width => _width;

        public bool IsBlocked(Point point)
        {
            return _blocked.ContainsKey(point);
        }

        public void UnblockCoordinates(IEnumerable<Point> blockedPoints)
        {
            if (blockedPoints != null)
            {
                foreach (var p in blockedPoints)
                {
                    _blocked.Remove(p);
                }
            }
        }


        public static MapManager? Load(string path)
        {
            Godot.FileAccess? fileAccess = null;
            try
            {
                fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
                if (fileAccess == null)
                {
                    return null;
                }
                int len = Marshal.SizeOf(typeof(Header));
                var bytes = fileAccess.GetBuffer(len);
                var header = BytesToStructure<Header>(bytes);
                MapCell[,] coor = new MapCell[header.Height, header.Width];
                if (!header.Magic.StartsWith("ATZMAP2"))
                {
                    return null;
                }
                for (int h = 0; h < header.Height / BLOCK_SIZE; h++)
                {
                    for (int w = 0; w < header.Width / BLOCK_SIZE; w++)
                    {
                        bytes = fileAccess.GetBuffer(Marshal.SizeOf<MapBlockData>());
                        var block = BytesToStructure<MapBlockData>(bytes);
                        for (int by = 0; by < BLOCK_SIZE; by++)
                        {
                            for (int bx = 0; bx < BLOCK_SIZE; bx++)
                            {
                                coor[h * BLOCK_SIZE + by, w * BLOCK_SIZE + bx] = block.MapCells[by * BLOCK_SIZE + bx];
                            }
                        }
                    }
                }
                return new MapManager(path, coor, header.Height, header.Width);
            }
            catch (Exception e)
            {
                GD.Print(e);
            }
            finally
            {
                fileAccess?.Close();
            }
            return null;
        }

        public static T BytesToStructure<T>(byte[] bytes)
        {
            int size = Marshal.SizeOf(typeof(T));
            if (bytes.Length < size)
                size = bytes.Length;
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}