//  AddSceneToSceneGroupArea.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEngine;
using System;

namespace SceneManageWindow
{
	/// <summary>
	/// シーンをシーングループに追加する領域
	/// </summary>
	public class AddSceneToSceneGroupArea : AreaBase
	{
		#region define

		private const string ADD_SCENE_TO_SCENEGROUP_AREA_TITLE = "Add To SceneGroup";

		#endregion define


		#region variables

		private int _targetSceneGroupIndex;

		private string[] _sceneGroupNames;

		private Action<string> _onAddedToSceneGroup;

		#endregion variables


		#region methods

		public AddSceneToSceneGroupArea( string[] sceneGroupNames, Action<string> onAddedToSceneGroup ) : base( ADD_SCENE_TO_SCENEGROUP_AREA_TITLE )
		{
			this._sceneGroupNames = sceneGroupNames;
			this._onAddedToSceneGroup = onAddedToSceneGroup;
			this._targetSceneGroupIndex = 0;
		}

		protected override void DrawAreaDetail( GUIStyle style )
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label( "SceneGroup", GUILayout.Width( 80f ) );
				_targetSceneGroupIndex = EditorGUILayout.Popup( _targetSceneGroupIndex, _sceneGroupNames );
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if( GUILayout.Button( "Add to SceneGroup" ) && _onAddedToSceneGroup != null ) {
					_onAddedToSceneGroup( _sceneGroupNames[ _targetSceneGroupIndex ] );
					GUI.FocusControl( "" );
				}
			}
			GUILayout.EndHorizontal();
		}

		#endregion methods
	}
}