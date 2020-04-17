using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coldsteel.ContentTool
{
	class Program
	{
		static void Main(string[] args)
		{
			var root = @"./";

			var contentFileName = Directory.GetFiles(root, "*.mgcb")
				.FirstOrDefault();

			if (contentFileName == null)
			{
				Console.WriteLine("No content file in directory");
				return;
			}

			var fileLines = File.ReadAllLines(contentFileName);

			var contentFile = new ContentFile();
			contentFile.Load(fileLines);

			var contentDirectories = Directory.GetDirectories(root)
				.Where(d => contentDirectoryNames.Contains(Path.GetFileName(d)))
				.ToArray();

			var codeBuilder = BeginContentCode();
			foreach (var directory in contentDirectories)
			{
				var groupName = Path.GetFileName(directory);
				var createEntry = ContentFileEntry.GetEntryConstructor(groupName);
				StartContentClass(codeBuilder, groupName);
				var files = Directory.GetFiles(directory);
				foreach (var file in files)
				{
					var propertyName = Path.GetFileNameWithoutExtension(file);
					var assetName = $"{groupName}/{propertyName}";

					WriteContentProperty(codeBuilder, GetPropertyName(propertyName), assetName);

					var entryFileName = $"{groupName}/{Path.GetFileName(file)}";
					var entry = contentFile.Entries.FirstOrDefault(e => e.Name == entryFileName);
					if (entry == null)
					{
						contentFile.Entries.Add(createEntry(entryFileName));
					}
					else
					{
						entry.Found = true;
					}
				}
				EndContentClass(codeBuilder);
			}
			var code = EndContentCode(codeBuilder);
			var output = Path.Combine(root, "Assets.cs");
			File.WriteAllText(output, code);

			var data = contentFile.ToString();
			File.WriteAllLines($"{contentFileName}.old", fileLines);
			File.WriteAllText($"{contentFileName}", data);
		}

		private static string[] contentDirectoryNames = new string[]
		{
			"Effect",
			"SpriteFont",
			"Texture2D",
			"Song",
			"SoundEffect"
		};

		class ContentFile
		{
			public List<string> HeaderLines { get; } = new List<string>();

			public List<ContentFileEntry> Entries { get; } = new List<ContentFileEntry>();

			internal void Load(string[] fileLines)
			{
				var lineIdx = 0;
				while (!fileLines[lineIdx].Contains("Content"))
				{
					HeaderLines.Add(fileLines[lineIdx]);
					lineIdx++;
				}
				HeaderLines.Add(fileLines[lineIdx]);
				lineIdx++;

				while (lineIdx < fileLines.Length)
				{
					while (lineIdx < fileLines.Length && string.IsNullOrWhiteSpace(fileLines[lineIdx]))
						lineIdx++;

					if (lineIdx >= fileLines.Length)
						break;

					var entry = new ContentFileEntry
					{
						Name = fileLines[lineIdx].Replace("#begin ", "").Trim(),
						Found = false,
					};
					while (lineIdx < fileLines.Length && !string.IsNullOrWhiteSpace(fileLines[lineIdx]))
					{
						entry.Lines.Add(fileLines[lineIdx]);
						lineIdx++;
					}
					Entries.Add(entry);
				}
			}

			public override string ToString()
			{
				var sb = new StringBuilder(string.Join("\r\n", HeaderLines));
				sb.AppendLine();
				sb.AppendLine();
				foreach (var entry in Entries.Where(e => e.Found))
				{
					sb.AppendLine(string.Join("\r\n", entry.Lines));
					sb.AppendLine();
				}
				return sb.ToString();
			}
		}

		class ContentFileEntry
		{
			public string Name { get; set; }

			public bool Found { get; set; } = true;

			public List<string> Lines { get; } = new List<string>();

			public static Func<string, ContentFileEntry> GetEntryConstructor(string type)
			{
				switch (type)
				{
					case "Effect": return CreateEffect;
					case "SpriteFont": return CreateSpriteFont;
					case "Texture2D": return CreateTexture2D;
					case "Song": return CreateSong;
					case "SoundEffect": return CreateSoundEffect;
					default: return null;
				}
			}

			private static ContentFileEntry CreateEffect(string name)
			{
				var entry = new ContentFileEntry { Name = name };
				entry.Lines.Add($"#begin {name}");
				entry.Lines.Add($"/importer:EffectImporter");
				entry.Lines.Add($"/processor:EffectProcessor");
				entry.Lines.Add($"/processorParam:DebugMode=Auto");
				entry.Lines.Add($"/build:{name}");
				return entry;
			}

			private static ContentFileEntry CreateSpriteFont(string name)
			{
				var entry = new ContentFileEntry { Name = name };
				entry.Lines.Add($"#begin {name}");
				entry.Lines.Add($"/importer:FontDescriptionImporter");
				entry.Lines.Add($"/processor:FontDescriptionProcessor");
				entry.Lines.Add($"/processorParam:PremultiplyAlpha=True");
				entry.Lines.Add($"/processorParam:TextureFormat=Compressed");
				entry.Lines.Add($"/build:{name}");
				return entry;
			}

			private static ContentFileEntry CreateTexture2D(string name)
			{
				var entry = new ContentFileEntry { Name = name };
				entry.Lines.Add($"#begin {name}");
				entry.Lines.Add($"/importer:TextureImporter");
				entry.Lines.Add($"/processor:TextureProcessor");
				entry.Lines.Add($"/processorParam:ColorKeyColor=255,0,255,255");
				entry.Lines.Add($"/processorParam:ColorKeyEnabled=True");
				entry.Lines.Add($"/processorParam:GenerateMipmaps=False");
				entry.Lines.Add($"/processorParam:PremultiplyAlpha=True");
				entry.Lines.Add($"/processorParam:ResizeToPowerOfTwo=False");
				entry.Lines.Add($"/processorParam:MakeSquare=False");
				entry.Lines.Add($"/processorParam:TextureFormat=Color");
				entry.Lines.Add($"/build:{name}");
				return entry;
			}

			private static ContentFileEntry CreateSong(string name)
			{
				var entry = new ContentFileEntry { Name = name };
				entry.Lines.Add($"#begin {name}");
				entry.Lines.Add($"/importer:Mp3Importer");
				entry.Lines.Add($"/processor:SongProcessor");
				entry.Lines.Add($"/processorParam:Quality=Best");
				entry.Lines.Add($"/build:{name}");
				return entry;
			}

			private static ContentFileEntry CreateSoundEffect(string name)
			{
				var entry = new ContentFileEntry { Name = name };
				entry.Lines.Add($"#begin {name}");
				entry.Lines.Add($"/importer:WavImporter");
				entry.Lines.Add($"/processor:SoundEffectProcessor");
				entry.Lines.Add($"/processorParam:Quality=Best");
				entry.Lines.Add($"/build:{name}");
				return entry;
			}
		}

		private static string GetPropertyName(string propertyName)
		{
			return new string(propertyName.Select(c => char.IsLetterOrDigit(c) || c == '_' ? c : '_').ToArray());
		}

		private static StringBuilder BeginContentCode()
		{
			var codeBuilder = new StringBuilder();
			codeBuilder.AppendLine("namespace LD46");
			codeBuilder.AppendLine("{");
			codeBuilder.AppendLine("\tpublic static class Assets");
			codeBuilder.AppendLine("\t{");
			return codeBuilder;
		}

		private static void StartContentClass(StringBuilder codeBuilder, string name)
		{
			codeBuilder.AppendLine($"\t\tpublic static class {name}");
			codeBuilder.AppendLine("\t\t{");
		}

		private static void WriteContentProperty(StringBuilder codeBuilder, string propertyName, string name)
		{
			codeBuilder.AppendLine($"\t\t\tpublic const string {propertyName} = \"{name}\";");
		}

		private static void EndContentClass(StringBuilder codeBuilder)
		{
			codeBuilder.AppendLine("\t\t}");
		}

		private static string EndContentCode(StringBuilder codeBuilder)
		{
			codeBuilder.AppendLine("\t}");
			codeBuilder.AppendLine("}");
			return codeBuilder.ToString();
		}
	}
}


//#----------------------------- Global Properties ----------------------------#

///outputDir:bin/$(Platform)
///intermediateDir:obj/$(Platform)
///platform:DesktopGL
///config:
///profile:Reach
///compress:False

//#-------------------------------- References --------------------------------#


//#---------------------------------- Content ---------------------------------#

//#begin Effect/dummy.fx
///importer:EffectImporter
///processor:EffectProcessor
///processorParam:DebugMode=Auto
///build:Effect/dummy.fx

//#begin Effect/fade.fx
///importer:EffectImporter
///processor:EffectProcessor
///processorParam:DebugMode=Auto
///build:Effect/fade.fx

//#begin Song/dummy.mp3
///importer:Mp3Importer
///processor:SongProcessor
///processorParam:Quality=Best
///build:Song/dummy.mp3

//#begin SoundEffect/dummy.wav
///importer:WavImporter
///processor:SoundEffectProcessor
///processorParam:Quality=Best
///build:SoundEffect/dummy.wav

//#begin SpriteFont/dummy.spritefont
///importer:FontDescriptionImporter
///processor:FontDescriptionProcessor
///processorParam:PremultiplyAlpha=True
///processorParam:TextureFormat=Compressed
///build:SpriteFont/dummy.spritefont

//#begin Texture2D/dummy.png
///importer:TextureImporter
///processor:TextureProcessor
///processorParam:ColorKeyColor=255,0,255,255
///processorParam:ColorKeyEnabled=True
///processorParam:GenerateMipmaps=False
///processorParam:PremultiplyAlpha=True
///processorParam:ResizeToPowerOfTwo=False
///processorParam:MakeSquare=False
///processorParam:TextureFormat=Color
///build:Texture2D/dummy.png

//#begin Texture2D/mainMenuBackground.png
///importer:TextureImporter
///processor:TextureProcessor
///processorParam:ColorKeyColor=255,0,255,255
///processorParam:ColorKeyEnabled=True
///processorParam:GenerateMipmaps=False
///processorParam:PremultiplyAlpha=True
///processorParam:ResizeToPowerOfTwo=False
///processorParam:MakeSquare=False
///processorParam:TextureFormat=Color
///build:Texture2D/mainMenuBackground.png

