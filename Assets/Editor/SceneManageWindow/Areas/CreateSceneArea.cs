//  CreateSceneArea.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;

namespace SceneManageWindow
{
	/// <summary>
	/// シーンを作成する領域
	/// </summary>
	public class CreateSceneArea : AreaBase
	{
		#region define

		private const string CREATE_SCENE_AREA_TITLE = "Create Scene";

		#endregion define


		#region variables

		private string[] _sceneDirectoryNames;

		private int _directoryNameIndex;

		private string _nameOfNewScene;

		private Action<string, string> _onCreated;

		#endregion variables


		#region methods

		public CreateSceneArea( string[] sceneDirectoryNames, Action<string, string> onCreated ) : base( CREATE_SCENE_AREA_TITLE )
		{
			this._sceneDirectoryNames = sceneDirectoryNames;
			this._onCreated = onCreated;
			this._directoryNameIndex = 0;
			this._nameOfNewScene = "";
		}

		protected override void DrawAreaDetail( GUIStyle style )
		{
			GUILayout.BeginHorizontal();
			{
				if( GUILayout.Button( "Directory", GUILayout.Width( 70f ) ) ) {
					var mouseGUIPos = Event.current.mousePosition;
					var popupPosLU = EditorGUIUtility.GUIToScreenPoint( mouseGUIPos );
					var popupSize = new Vector2( 200f, 100f );
					var popupRect = new Rect( popupPosLU, popupSize );
					MyPopup.Open( popupRect, _sceneDirectoryNames, OnDirectoryNameSelected );
				}
				GUILayout.Label( _sceneDirectoryNames[ _directoryNameIndex ], GUILayout.Width( EditorGUIUtility.currentViewWidth - 140f ) );

			}
			GUILayout.EndHorizontal();

			GUILayout.Space( 5f );

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label( "Name", GUILayout.Width( 40f ) );
				_nameOfNewScene = DrawTextFirld( _nameOfNewScene );
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				EditorGUI.BeginDisabledGroup( string.IsNullOrEmpty( _nameOfNewScene ) );
				{
					if( GUILayout.Button( "Create Scene" ) && _onCreated != null ) {
						_onCreated( _sceneDirectoryNames[ _directoryNameIndex ], _nameOfNewScene );
						GUI.FocusControl( "" );
						_nameOfNewScene = "";
					}
				}
				EditorGUI.EndDisabledGroup();
			}
			GUILayout.EndHorizontal();
		}

		private void OnDirectoryNameSelected( int index )
		{
			_directoryNameIndex = index;
		}

		#endregion methods
	}
}