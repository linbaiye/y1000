using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using y1000.code.player;

namespace y1000.code 
{
    public class SpriteContainer
    {
        private readonly Texture2D[] textures;

        private readonly Vector2[] offsets;

        public SpriteContainer(Texture2D[] textures, Vector2[] offsets)
        {
            this.textures = textures;
            this.offsets = offsets;
        }


        public Texture2D GetTexture(int nr) {
            if (nr < 0 || nr > textures.Length) {
                throw new FileNotFoundException();
            }
            return textures[nr];
        }

        public Vector2 GetOffset(int nr) {
            if (nr < 0 || nr > textures.Length) {
                throw new FileNotFoundException();
            }
            return offsets[nr];
        }


        public PositionedTexture Get(int nr)
        {
            if (nr < 0 || nr > textures.Length) {
                throw new FileNotFoundException();
            }
            return new PositionedTexture(GetTexture(nr), GetOffset(nr));
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

        private static Dictionary<string, SpriteContainer> LoadedContainers = new Dictionary<string, SpriteContainer>();

        public static SpriteContainer Load(string resDir) {
            if (!resDir.StartsWith("res://")){
                resDir = "res://" + resDir;
            }
            if (!resDir.EndsWith("/")) {
                resDir += "/";
            }
            if (LoadedContainers.TryGetValue(resDir, out SpriteContainer? container))
            {
                if (container != null)
                {
                    return container;
                }
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
            }
			fileAccess.Dispose();
            var ret = new SpriteContainer(textures, vectors);
            LoadedContainers.Add(resDir, ret);
            return ret;
        }


        public static SpriteContainer LoadMaleCharacterSprites(string nr)
        {
            return Load("res://sprite/char/" + nr + "/");
        }


        public static SpriteContainer LoadSprites(string name)
        {
            return Load("res://sprite/" + name + "/");
        }

        public static readonly SpriteContainer EmptyContainer = new SpriteContainer(Array.Empty<Texture2D>(), Array.Empty<Vector2>());
    }
}