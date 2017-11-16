//  AddSceneToMultiSceneArea.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;

namespace SceneManageWindow
{
	/// <summary>
	/// シーンをマルチシーンに追加する領域
	/// </summary>
	public class AddSceneToMultiSceneArea : AreaBase
	{
		#region define

		private const string ADD_SCENE_TO_MULTI_SCENE_AREA_TITLE = "Add To Multi-Scene";

		#endregion define


		#region variables

		private int _targetMultiSceneIndex;

		private string[] _multiSceneNames;

		private Action<string> _onAddToMultiScene;

		#endregion variables


		#region methods

		public AddSceneToMultiSceneArea( string[] multiSceneNames, Action<string> onAddedToMultiScene ) : base( ADD_SCENE_TO_MULTI_SCENE_AREA_TITLE )
		{
			this._multiSceneNames = multiSceneNames;
			this._onAddToMultiScene = onAddedToMultiScene;
			this._targetMultiSceneIndex = 0;
		}

		protected override void DrawAreaDetail( GUIStyle style )
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label( "Multi-Scene", GUILayout.Width( 80f ) );
				_targetMultiSceneIndex = EditorGUILayout.Popup( _targetMultiSceneIndex, _multiSceneNames );
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if( GUILayout.Button( "Add to MultiScene" ) && _onAddToMultiScene != null ) {
					_onAddToMultiScene( _multiSceneNames[ _targetMultiSceneIndex ] );
					GUI.FocusControl( "" );
				}
			}
			GUILayout.EndHorizontal();
		}

		#endregion methods
	}
}