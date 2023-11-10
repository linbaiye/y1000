using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp;
using Godot;
using System.Text.Json.Serialization;

namespace y1000.code.world
{
    public class ObjectLoader
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct FileHeader
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string magic;
            public int number;
        }

        public const int OBJECT_CELL_MAX = 16 * 16;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Obj3Struct
        {
            public int objectAniId;
            public int objectId;
            public byte style;
            public int mWidth;
            public int mHeigh;
            public int nBlock;
            public int startId;
            public int endId;
            public int iWidth;
            public int iHeight;
            public int ipx;
            public int ipy;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = OBJECT_CELL_MAX)]
            public byte[] mbuffer;
            public int aniDelay;
            public int dataPos;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] padding;


            /*ObjectAniId : Integer;      // 애니메이션 Object 구분자
                       ObjectId : integer;         // Object Data Index
              Style : TObjType;           // TOB_none: 일반object TOB_follow: aniObject;
              MWidth, MHeight: Integer;   // 1 블럭에 대한 Cell갯수
              nBlock : Integer;           // 블럭 갯수
              StartID : integer;          // ani 시작 ObjectID
              endID : integer;            // ani 끝 ObjectID
              IWidth, IHeight: Integer;   // 가로세로길이
              Ipx, Ipy: Integer;
              MBuffer : array[0..OBJECT_CELL_MAX - 1] of byte; // move Control buffer 16*16
              AniDelay : integer;
              DataPos : DWord;
              None : array[0..4 - 1] of integer;
              Bits : PTAns2Color;
             end;*/
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Obj2Struct
        {
            /*
             *     ObjectId: integer;
    MWidth, MHeight: Integer;
    IWidth, IHeight: Integer;
    Ipx, Ipy: Integer;
    MBuffer: array[0..OBJECT_CELL_MAX - 1] of byte;
    AniDelay: DWORD;
    None: array[0..4 - 1] of integer;
    Bits: PTAns2Color;*/
            public int objectId;
            public int mWidth;
            public int mHeigh;
            public int iWidth;
            public int iHeight;
            public int ipx;
            public int ipy;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = OBJECT_CELL_MAX)]
            public byte[] mbuffer;
            public int aniDelay;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] padding;
        }


   

        public class ObjectData
        {
            public int objectId { get; set; }
            public int mWidth{ get; set; }
            public int mHeigh{ get; set; }
            public int iWidth{ get; set; }
            public int iHeight{ get; set; }
            public int ipx{ get; set; }
            public int ipy{ get; set; }
                [JsonIgnore]
            public byte[] data{ get; set; }

            public ObjectData()
            {
                data = new byte[0];
            }

            public string AsJson()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        private ObjectData? FindData(int objectId)
        {
            foreach (ObjectData data in objectDatas)
            {
                if (data.objectId == objectId)
                { return data; }
            }
            return null;
        }


        public void Copy(SixLabors.ImageSharp.Image<Rgba32> image, int startX, int startY, int objectId, int objectNumber)
        {
            ObjectData? data = FindData(objectId);
            if (data == null)
                return;
            var offset = 0;
            //var offset = 2 * objectNumber * data.iWidth * data.iHeight;
            for (int j = 0; j < data.iHeight; j++)
            {
                for (int w = 0; w < data.iWidth; w++)
                {
                    //short color = BytesToStructure<short>(data.data.Skip(offset).ToArray());
                    short color = BitConverter.ToInt16(new byte[] {data.data[offset], data.data[offset + 1]});
                    short redMask = 0x7c00;
                    short greenMask = 0x3e0;
                    short blueMask = 0x1f;
                   short red = (short)((color & redMask) >> 10);
                    short green = (short)((color & greenMask) >> 5);
                    short blue = (short)(color & blueMask);
                        image[startX + w, startY + j] = new Rgba32(red << 3, green << 3, blue << 3);
                    /*else
                    {
                        image[startX + w, startY + j] = new Rgba32(0, 0, 0, 0);
                    }*/
                    offset += 2;
                }
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

        public static T ReadStructure<T>(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return theStructure;
        }

        private List<ObjectData> objectDatas;
        public ObjectLoader(List<ObjectData> objectDatas)
        {
            this.objectDatas = objectDatas;
        }
        private static ObjectLoader Read2Objects(BinaryReader reader, int ojbectNumber)
        {
            List<ObjectData> tileDatas = new List<ObjectData>();
            for (int i = 0; i < ojbectNumber; i++)
            {
                //list.Add(block);
                var block = ReadStructure<Obj2Struct>(reader);
                //list.Add(block);
                var len = block.iWidth * block.iHeight * 2;
                byte[] bytes = reader.ReadBytes(len);
                ObjectData data = new ObjectData();
                data.iWidth = block.iWidth;
                data.iHeight = block.iHeight;
                data.objectId = block.objectId;
                data.mHeigh = block.mHeigh;
                data.mWidth = block.mWidth;
                data.ipx = block.ipx;
                data.ipy = block.ipy;
                data.data = bytes;
                tileDatas.Add(data);
                //map.Add(block.objectId, new ObjData(block, bytes));
                //logger.Info("[{0}, {1}], width {2}, height {3}, block {4}.", block.nWCell, block.nHCell, block.iWidth, block.iHeight, block.nBlock);
            }
            return new ObjectLoader(tileDatas);
        }

        private static ObjectLoader Read3Objects(BinaryReader reader, int ojbectNumber)
        {
            List<ObjectData> tileDatas = new List<ObjectData>();
            for (int i = 0; i < ojbectNumber; i++)
            {
                //list.Add(block);
                var block = ReadStructure<Obj3Struct>(reader);
                //list.Add(block);
                var len = block.iWidth * block.iHeight * 2 * block.nBlock;
                byte[] bytes = reader.ReadBytes(len);
                ObjectData data = new ObjectData();
                data.iWidth = block.iWidth;
                data.iHeight = block.iHeight;
                data.objectId = block.objectId;
                data.mHeigh = block.mHeigh;
                data.mWidth = block.mWidth;
                data.ipx = block.ipx;
                data.ipy = block.ipy;
                data.data = bytes;
                tileDatas.Add(data);
                //map.Add(block.objectId, new ObjData(block, bytes));
                //logger.Info("[{0}, {1}], width {2}, height {3}, block {4}.", block.nWCell, block.nHCell, block.iWidth, block.iHeight, block.nBlock);
            }
            return new ObjectLoader(tileDatas);
        }

        public void Dump1(string direpath)
        {
            /*foreach (TileData tileData in mdata)
            {
                Directory.CreateDirectory(direpath + "/" + tileData.tileId);
                logger.Info("Dump tile {0}.", tileData.tileId);
                int total = tileData.nWCell * tileData.nHCell * tileData.nBlock;
                int width = total;
                int height = 1;
                if (total > 10)
                {
                    width = 10;
                    height = total / 10 + (total % width > 0 ? 1 : 0);
                }
                Bitmap bitmap = new Bitmap(32 * width, 24 * height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Copy(bitmap, j * 32, i * 24, tileData.tileId, i * 10 + j);
                        bitmap.Save(direpath + "/" + tileData.tileId + ".png", ImageFormat.Png);
                    }
                }
            }*/
            foreach (ObjectData objData in objectDatas)
            {
                Directory.CreateDirectory(direpath + "/" + objData.objectId);
                /*SixLabors.ImageSharp.Image<Rgba32> image = new SixLabors.ImageSharp.Image<Rgba32>(objData.iWidth, objData.iHeight);
                Copy(image, 0, 0, objData.objectId, 0);
                var path = direpath + "/" + objData.objectId + "/" + "image.png";
                File.Delete(path);
                using (var outputStream = new FileStream(path, FileMode.CreateNew))
                {
                    image.SaveAsPng(outputStream);
                }*/
                Object2Json json = new Object2Json();
                if (objData.objectId == 70 || objData.objectId == 71)
                {
                    GD.Print(objData.AsJson());
                }
                json.X = objData.ipx;
                json.Y = objData.ipy;
                json.Height = objData.iHeight;
                json.Width = objData.iWidth;
                /*json.YSort = objData.mHeigh;
                using (StreamWriter writer = new StreamWriter(direpath + "/" + objData.objectId + "/struct.json"))
                {
                    writer.Write(json.AsJson());
                }*/
            }
        }


        public static ObjectLoader? Unpack(string path)
        {
            FileStream? fileStream = null;
            BinaryReader? reader = null;
            try
            {
                fileStream = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read);
                reader = new BinaryReader(fileStream);
                var header = ReadStructure<FileHeader>(reader);
                //byte[,] coor = new byte[header.Height, header.Width];
                if (header.magic.StartsWith("ATZOBJ2"))
                {
                    return Read2Objects(reader, header.number);
                }
                if (header.magic.StartsWith("ATZOBJ3"))
                {
                    return Read3Objects(reader, header.number);
                }
            }
            finally
            {
                reader?.Close();
                fileStream?.Close();
            }
            return null;
        }
    }
}