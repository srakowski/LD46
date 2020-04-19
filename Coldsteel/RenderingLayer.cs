// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Coldsteel
{
	public class RenderingLayer
	{
		private Engine _engine;

		private Scene _scene;

		public string Name;

		public int Depth;

		public bool FixToCamera;

		public SpriteSortMode SpriteSortMode;

		public BlendState BlendState;

		public SamplerState SamplerState;

		public DepthStencilState DepthStencilState;

		public RasterizerState RasterizerState;

		public Matrix? TransformMatrix;

		public Shader Shader { get; private set; }

		public RenderingLayer SetShader(Shader shader)
		{
			Shader?.Deactivate();
			if (_engine != null && _scene != null)
			{
				shader.Activate(_engine, _scene);
			}
			Shader = shader;
			return this;
		}

		public RenderingLayer() { }

		public RenderingLayer(string name)
		{
			Name = name;
		}

		public RenderingLayer(string name, int depth)
		{
			Name = name;
			Depth = depth;
		}

		public RenderingLayer AddToScene(Scene scene)
		{
			scene.AddRenderingLayer(this);
			return this;
		}
		
		internal void Activate(Engine engine, Scene scene)
		{
			_engine = engine;
			_scene = scene;
			Shader?.Activate(engine, scene);
		}

		internal void Deactivate()
		{
			Shader?.Deactivate();
			_scene = null;
			_engine = null;
		}

		internal void Draw(SpriteBatch spriteBatch, Camera camera, IEnumerable<IRenderer> sprites)
		{
			var cameraMatrix = camera != null && !FixToCamera
				? camera.TransformationMatrix
				: Matrix.Identity;

			var transformMatrix = TransformMatrix ?? Matrix.Identity;

			spriteBatch.Begin(
				SpriteSortMode,
				BlendState,
				SamplerState,
				DepthStencilState,
				RasterizerState,
				Shader?.Effect,
				transformMatrix * cameraMatrix
			);

			Shader?.ApplyParameters();

			foreach (var sprite in sprites)
				sprite.Draw(spriteBatch);

			spriteBatch.End();
		}

		public static RenderingLayer New(string name, int depth = 0) => new RenderingLayer(name, depth);
	}
}
