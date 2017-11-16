//  DuplicateSceneArea.cs
//
//  Created by Sonoichi.

using System;
using UnityEngine;
using UnityEditor;

namespace SceneManageWindow
{
	/// <summary>
	/// シーンを複製する領域
	/// </summary>
	public class DuplicateSceneArea : AreaBase
	{
		#region define

		private const string DUPLICATE_SCENE_AREA_TITLE = "Duplicate Scene";

		#endregion define


		#region variables

		private string[] _sceneDirectoryNames;

		private int _directoryNameIndex;

		private string _nameOfNewScene;

		private Action<string, string> _onDuplicated;

		#endregion variables


		#region methods

		public DuplicateSceneArea( string[] sceneDirectoryNames, Action<string, string> onDuplicated ) : base( DUPLICATE_SCENE_AREA_TITLE )
		{
			this._sceneDirectoryNames = sceneDirectoryNames;
			this._onDuplicated = onDuplicated;
			this._directoryNameIndex = 0;
			this._nameOfNewScene = "";
		}

		protected override void DrawAreaDetail( UnityEngine.GUIStyle style )
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
					if( GUILayout.Button( "Duplicate Scene" ) && _onDuplicated != null ) {
						_onDuplicated( _sceneDirectoryNames[ _directoryNameIndex ], _nameOfNewScene );
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