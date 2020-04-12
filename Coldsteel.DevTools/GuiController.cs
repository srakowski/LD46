// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Coldsteel.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Coldsteel.DevTools
{
	[Route("api/[controller]")]
	[ApiController]
	public class GuiController : ControllerBase
	{
		private static EditorSceneFactory editorSceneFactory = new EditorSceneFactory();

		[HttpPost]
		public ActionResult<Guid> Post([FromBody] ViewDto createView, [FromServices] Engine engine)
		{
			engine.SceneManager.SceneFactory = editorSceneFactory;
			var scene = new Scene();
			editorSceneFactory.Add(createView.Name, scene);
			engine.LoadScene(createView.Name, null);

			var view = new View();
			view.Name = createView.Name;
			var e = Entity.New.AddComponent(view);
			scene.AddEntity(e);

			return Ok(view.Id);
		}

		[HttpGet("{id}")]
		public ActionResult<View> Get(Guid id, [FromServices] Engine engine)
		{
			var view = engine.SceneManager.ActiveScene.FindComponentById(id) as View;
			if (view == null) return NotFound();
			return Ok(ViewDto.FromView(view));
		}

		public class ViewDto
		{
			public ViewDto() { }

			public Guid Id { get; set; }

			public string Name { get; set; }

			public string BackgroundColor { get; set; }

			public static ViewDto FromView(View view)
			{
				var bg = view.BackgroundColor;
				return new ViewDto
				{
					Id = view.Id,
					Name = view.Name,
					BackgroundColor = 
						$"rgba({bg.R},{bg.G},{bg.B},{(float)bg.A / byte.MaxValue})"
				};
			}
		}

		private class EditorSceneFactory : ISceneFactory
		{
			public Dictionary<string, Scene> _scene = new Dictionary<string, Scene>();

			public void Add(string name, Scene scene)
			{
				_scene[name] = scene;
			}

			public Scene Create(string sceneName, GameState gameState)
			{
				return _scene[sceneName];
			}
		}
	}
}