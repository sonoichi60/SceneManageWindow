//  SceneManageWindow.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// シーンの管理を行うウィンドウ
	/// </summary>
	public class SceneManageWindowMain : WindowBase
	{
		#region variables

		private MySceneManager _sceneManager;

		private List<FoldoutableViewBase> _foldoutableViews;

		#endregion variables


		#region methods

		[MenuItem( "Window/SceneManageWindow" )]
		public static void Open()
		{
			GetWindow<SceneManageWindowMain>();
		}

		protected override void Initialize()
		{
			_sceneManager = new MySceneManager();

			_foldoutableViews = new List<FoldoutableViewBase>();
			_foldoutableViews.Add( new ScenesInProjectView( _sceneManager, Initialize ) );
			_foldoutableViews.Add( new SceneGroupView( _sceneManager ) );
			_foldoutableViews.Add( new MultiScenesView( _sceneManager ) );
			_foldoutableViews.Add( new ScenesInBuildView( _sceneManager ) );

			GUI.FocusControl( "" );
		}

		protected override void DrawWindowDetail()
		{
			for( int i = 0 ; i < _foldoutableViews.Count ; i++ ) {
				_foldoutableViews[ i ].Draw();
			}
		}

		#endregion methods
	}
}