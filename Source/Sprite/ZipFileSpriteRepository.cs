using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Godot;
using NLog;
using y1000.Source.Util;
using FileAccess = Godot.FileAccess;

namespace y1000.Source.Sprite;

public class ZipFileSpriteRepository : AbstractSpriteRepository, ISpriteRepository
{
	public static readonly ZipFileSpriteRepository Instance = new ();
	private ZipFileSpriteRepository() {}
	
	private const string DirPath = "/Sprites/";
	//private const string DirPath = "../sprite/";
	
	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private readonly Cache<string, AtzSprite> _cache = new();
	
	private class Cache<TKey, TValue> where TKey : notnull
	{
		private readonly Dictionary<TKey, CacheItem<TValue>> _cache = new();

		public void Store(TKey key, TValue value, TimeSpan expiresAfter)
		{
			_cache[key] = new CacheItem<TValue>(value, expiresAfter);
		}

		public TValue? Get(TKey key)
		{
			if (!_cache.TryGetValue(key, out var cached)) return default(TValue);
			if (DateTimeOffset.Now - cached.Created >= cached.ExpiresAfter)
			{
				_cache.Remove(key);
				return default;
			}
			return cached.Value;
		}
	}
	private class CacheItem<T>
	{
		public CacheItem(T value, TimeSpan expiresAfter)
		{
			Value = value;
			ExpiresAfter = expiresAfter;
		}
		public T Value { get; }
		internal DateTimeOffset Created { get; } = DateTimeOffset.Now;
		internal TimeSpan ExpiresAfter { get; }
	}
		
	
	private Vector2[] ParseVectors(IEnumerable<string> lines)
	{
		return (from line in lines where line.Contains(',') select ParseLine(line)).ToArray();
	}

	private string GetAbsPath(string name)
	{
		var path = OS.GetExecutablePath().GetBaseDir();
		return path + DirPath + name + ".zip";
	}

	public bool Exists(string name) {
		return File.Exists(GetAbsPath(name));
	}
	

	public override AtzSprite LoadByNumberAndOffset(string name, Vector2? offset = null)
	{
		var sprite = _cache.Get(name);
		if (sprite != null)
		{
			return sprite;
		}
		try
		{
			var resPath = "res://Sprites/" + name.ToLower() + ".zip";
			var fileAccess = FileAccess.Open(resPath, FileAccess.ModeFlags.Read);
			fileAccess.GetBuffer((int)fileAccess.GetLength());
			bool ret = File.Exists(fileAccess.GetPathAbsolute());
			GD.Print("Test path " + fileAccess.GetPathAbsolute() + ", result : " + ret);
			var reader = new ZipReader();
			reader.Open("res://Sprites/" + name.ToLower() + ".zip");
			if (!reader.FileExists("offset.txt"))
			{
				GD.Print(" no offset ");
				throw new FileNotFoundException("Bad atz zip file: " + name);
			}
			GD.Print("Loading atz " + name);
			var offsetEntry = reader.ReadTextFile("offset.txt");
			var sizeEntry = reader.ReadTextFile("size.txt");
			var vectors = ParseVectors(offsetEntry.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
			var sizes = ParseVectors(sizeEntry.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
			List<Texture2D> texture2Ds = new List<Texture2D>();
			for (int i = 0; i < vectors.Length; i++)
			{
				var filename = "000" + i.ToString("D3") + ".png";
				var texture = reader.ReadTextureFile(filename);
				if (texture == null)
				{
					continue;
				}
				texture2Ds.Add(texture);
				if (offset.HasValue)
				{
					vectors[i] += offset.Value;
				}
			}

			if (texture2Ds.Count != vectors.Length)
			{
				LOGGER.Error("Invalid atz {0}.", name);
			}
			LOGGER.Debug("Loaded {0}.", name);
			var atzSprite = new AtzSprite(texture2Ds.ToArray(), vectors, sizes);
			_cache.Store(name, atzSprite, TimeSpan.FromMinutes(5));
			return atzSprite;
		}
		catch (Exception e)
		{
			GD.PrintErr(e);
            throw;
        }
	}

}
