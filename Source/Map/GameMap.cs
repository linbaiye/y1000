using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Godot;
using y1000.code.world;

namespace y1000.Source.Map
{
    public class GameMap: IRealm
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

        public const int BLOCK_SIZE = 40;


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct MapBlockData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string MapBlockIdent;
            public int MapChangedCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1600)]
            public MapCell[] MapCells;
        }

        private readonly MapCell[,] _cells;

        private readonly int _height;
        
        private readonly int _width;

        private ISet<int>? ids;

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


        public GameMap(MapCell[,] coor, int heigh, int width)
        {
            _cells = coor;
            _height = heigh;
            _width = width;
        }

        public void ForeachCell(Action<MapCell, int, int> action)
        {
            for (int y = 0; y < 60; y++)
            {
                for (int x = 0; x < 60; x++)
                {
                    action.Invoke(_cells[y, x], x, y);
                }
            }
        }
        
        public void ForeachCell(Vector2I start, Vector2I end, Action<MapCell, int, int> action)
        {
            for (var y = start.Y; y <= end.Y; y++)
            {
                if (y < 0 || y > Height)
                {
                    continue;
                }
                for (var x = start.X; x < end.X; x++)
                {
                    if (x < 0 || x > Width)
                    {
                        continue;
                    }
                    action.Invoke(_cells[y, x], x, y);
                }
            }
        }

        private bool IsInRange(int x, int y)
        {
            return x >= 0 && x < _width && y >=0 && y < _width;
        }


        public bool IsMovable(Point point)
        {
            return IsMovable(point.X, point.Y);
        }

        private bool IsMovable(int x, int y)
        {
            return IsInRange(x, y) && (_cells[y, x].BoMove & 0x1 ) == 0 && (_cells[y, x].BoMove & 0x2 ) == 0;
        }

        public bool HideRoof(Vector2I point)
        {
            if (point.X >= _width || point.X < 0 || point.Y >= _height || point.Y < 0)
            {
                return false;
            }
            return _cells[point.Y, point.X].BoMove == 4;
        }

        public int Height => _height;

        public int Width => _width;

        public static GameMap? Load(string path)
        {
            if (!Godot.FileAccess.FileExists(path))
            {
                return null;
            }
            using (Godot.FileAccess? fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read))
            {
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
                return new GameMap(coor, header.Height, header.Width);
            }
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

        public bool CanMove(Vector2I coordinate)
        {
            return IsMovable(coordinate.X, coordinate.Y);
        }
    }
}