using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using y1000.Source.Animation;

namespace y1000.Source.Sprite 
{
    public class AtzSprite
    {

        private readonly OffsetTexture[] _offsetTextures;


        public List<OffsetTexture> GetAll()
        {
            return new List<OffsetTexture>(_offsetTextures);
        }


        public AtzSprite(Texture2D[] textures, Vector2[] offsets, Vector2[]? sizes = null)
        {
            _offsetTextures = new OffsetTexture[textures.Length];
            for (int i = 0; i < textures.Length; i++)
            {
                _offsetTextures[i] = new OffsetTexture(textures[i], offsets[i], sizes?[i]);
            }
        }


        public OffsetTexture Get(int nr)
        {
            if (nr < 0 || nr >= _offsetTextures.Length) {
                throw new ArgumentOutOfRangeException("Total " + _offsetTextures.Length + ", current " + nr);
            }
            return _offsetTextures[nr];
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

        private static AtzSprite Load(string resDir, Vector2? offset = null) {
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
                if (offset.HasValue)
                {
                    vectors[i] += offset.Value;
                }
            }
			fileAccess.Dispose();
            var ret = new AtzSprite(textures, vectors);
            return ret;
        }

        public static AtzSprite LoadOffsetMalePlayerSprites(string nr)
        {
            //return Load("res://sprite/char/" + nr + "/");
            throw new Exception();
        }
        

        public static AtzSprite LoadSprites(string name)
        {
            return Load("res://sprite/" + name + "/");
        }
        
        public static AtzSprite LoadEffect(string name)
        {
            return Load("res://sprite/Effect/" + name + "/");
        }



        public static AtzSprite LoadOffsetMonsterSprites(string name)
        {
           // return Load("res://sprite/monster/" + name + "/" );
            //return Load("res://sprite/monster/" + name + "/");
            return Load("res://sprite/monster/" + name + "/", new Vector2(16, -12));
        }
        

        public static readonly AtzSprite Empty = new(Array.Empty<Texture2D>(), Array.Empty<Vector2>());
    }
}