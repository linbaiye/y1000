using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using y1000.code.player;

namespace y1000.Source.Sprite 
{
    // should not be exposed, SpriteManager should enclose this.
    public class SpriteReader
    {
        private readonly Texture2D[] textures;

        private readonly Vector2[] offsets;

        private SpriteReader(Texture2D[] textures, Vector2[] offsets)
        {
            this.textures = textures;
            this.offsets = offsets;
        }


        private Texture2D GetTexture(int nr) {
            if (nr < 0 || nr > textures.Length) {
                throw new FileNotFoundException();
            }
            return textures[nr];
        }

        private Vector2 GetOffset(int nr) {
            if (nr < 0 || nr > textures.Length) {
                throw new FileNotFoundException();
            }
            return offsets[nr];
        }


        public OffsetTexture Get(int nr)
        {
            if (nr < 0 || nr > textures.Length) {
                throw new FileNotFoundException();
            }
            return new OffsetTexture(GetTexture(nr), GetOffset(nr));
        }

        private static Vector2[] ReadOffsets(string text)
        {
            var tokens = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<Vector2> list = new List<Vector2>();
            foreach (var s in tokens){ 
                if (!s.Contains(',')) {
                    continue;
                }
                var nobrackets = s.Replace("[", "").Replace("]", "");
                var numbers = nobrackets.Split(",");
                if (numbers.Length == 2) {
                    list.Add(new Vector2(int.Parse(numbers[0].Trim()), int.Parse(numbers[1].Trim())));
                }
            }
            
            return list.ToArray();
        }

        private static readonly Dictionary<string, SpriteReader> LoadedContainers = new Dictionary<string, SpriteReader>();

        private static SpriteReader Load(string resDir, Vector2? offset = null) {
            if (!resDir.StartsWith("res://")){
                resDir = "res://" + resDir;
            }
            if (!resDir.EndsWith("/")) {
                resDir += "/";
            }
            if (LoadedContainers.TryGetValue(resDir, out var container))
            {
                return container;
            }
            if (!Godot.FileAccess.FileExists(resDir + "offset.txt")) {
                throw new FileNotFoundException();
            }
			Godot.FileAccess fileAccess = Godot.FileAccess.Open(resDir + "offset.txt", Godot.FileAccess.ModeFlags.Read);
            Vector2[] vectors = ReadOffsets(fileAccess.GetAsText());
            if (vectors.Length <= 0) {
                throw new FileNotFoundException("Empty offset file.");
            }
            Texture2D[] textures = new Texture2D[vectors.Length];
            for (int i = 0; i < vectors.Length ; i++) {
                var texture = ResourceLoader.Load(resDir + "000" + i.ToString("D3") + ".png") as Texture2D;
                textures[i] = texture;
                if (offset.HasValue)
                {
                    vectors[i] += offset.Value;
                }
            }
			fileAccess.Dispose();
            var ret = new SpriteReader(textures, vectors);
            LoadedContainers.Add(resDir, ret);
            return ret;
        }


        public static SpriteReader LoadMalePlayerSprites(string nr)
        {
            return Load("res://sprite/char/" + nr + "/");
        }


        public static SpriteReader LoadSprites(string name)
        {
            return Load("res://sprite/" + name + "/");
        }

        public static SpriteReader LoadSprites(string name, Vector2 offset)
        {
            return Load("res://sprite/" + name + "/", offset);
        }


        public static SpriteReader LoadMonsterSprites(string name)
        {
            return Load("res://sprite/monster/" + name + "/");
        }

        public static readonly SpriteReader EmptyReader = new SpriteReader(Array.Empty<Texture2D>(), Array.Empty<Vector2>());
    }
}