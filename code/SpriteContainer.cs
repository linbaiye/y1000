using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Godot;

namespace test.cide
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

        public static SpriteContainer Load(string resDir) {
            if (!resDir.StartsWith("res://")){
                resDir = "res://" + resDir;
            }
            if (!resDir.EndsWith("/")) {
                resDir += "/";
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
            return new SpriteContainer(textures, vectors);
        }

        public static readonly SpriteContainer EmptyContainer = new SpriteContainer(Array.Empty<Texture2D>(), Array.Empty<Vector2>());
    }
}