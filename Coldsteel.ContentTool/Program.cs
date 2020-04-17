using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Coldsteel.ContentTool
{
	class Program
	{
		private static string[] contentDirectoryNames = new string[]
		{
			"Effect",
			"SpriteFont",
			"Texture2D",
			"Song",
			"SoundEffect"
		};

		static void Main(string[] args)
		{
			var root = @"C:\Users\srako\Desktop\LD46\LD46\Content";

			var contentFile = Directory.GetFiles(root, "*.mgcb")
				.FirstOrDefault();

			if (contentFile == null)
			{
				Console.WriteLine("No content file in directory");
				return;
			}

			var contentDirectories = Directory.GetDirectories(root)
				.Where(d => contentDirectoryNames.Contains(Path.GetFileName(d)))
				.ToArray();

			var codeBuilder = BeginContentCode();
			foreach (var directory in contentDirectories)
			{
				var groupName = Path.GetFileName(directory);
				StartContentClass(codeBuilder, groupName);
				var files = Directory.GetFiles(directory);
				foreach (var file in files)
				{
					var propertyName = Path.GetFileNameWithoutExtension(file);
					var assetName = $"{groupName}/{propertyName}";
					WriteContentProperty(codeBuilder, propertyName, assetName);
				}
				EndContentClass(codeBuilder);
			}
			var code = EndContentCode(codeBuilder);
			var output = Path.Combine(root, "Assets.cs");
			File.WriteAllText(output, code);
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

