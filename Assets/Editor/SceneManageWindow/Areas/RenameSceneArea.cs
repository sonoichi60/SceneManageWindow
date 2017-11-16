//  RenameSceneArea.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;

namespace SceneManageWindow
{
	/// <summary>
	/// シーン名を変更する領域
	/// </summary>
	public class RenameSceneArea : AreaBase
	{
		#region define

		private const string RENAME_SCENE_AREA_TITLE = "Rename Scene";

		#endregion define


		#region variables

		private string _newNameOfScene;

		private Action<string> _onRenamed;

		#endregion variables


		#region methods

		public RenameSceneArea( Action<string> onRenamed ) : base( RENAME_SCENE_AREA_TITLE )
		{
			this._onRenamed = onRenamed;
			this._newNameOfScene = "";
		}

		protected override void DrawAreaDetail( GUIStyle style )
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label( "Name", GUILayout.Width( 40f ) );
				_newNameOfScene = EditorGUILayout.TextField( _newNameOfScene, EditorStyles.textField );
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				EditorGUI.BeginDisabledGroup( string.IsNullOrEmpty( _newNameOfScene ) );
				{
					if( GUILayout.Button( "Rename Scene" ) && _onRenamed != null ) {
						_onRenamed( _newNameOfScene );
					}
				}
				EditorGUI.EndDisabledGroup();
			}
			GUILayout.EndHorizontal();
		}

		#endregion methods
	}
}