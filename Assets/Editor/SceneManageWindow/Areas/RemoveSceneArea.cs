//  RemoveSceneArea.cs
//
//  Created by Sonoichi.

using UnityEngine;
using System;

namespace SceneManageWindow
{
	/// <summary>
	/// シーンを削除する領域
	/// </summary>
	public class RemoveSceneArea : AreaBase
	{
		#region define

		private const string REMOVE_SCENE_AREA_TITLE = "Remove Scene";

		#endregion define


		#region variables

		private Action _onRemoved;

		#endregion variables


		#region methods

		public RemoveSceneArea( Action onRemoved ) : base( REMOVE_SCENE_AREA_TITLE )
		{
			this._onRemoved = onRemoved;
		}

		protected override void DrawAreaDetail( GUIStyle style )
		{
			GUILayout.BeginHorizontal();
			{
				if( GUILayout.Button( "Remove Scene" ) && _onRemoved != null ) {
					_onRemoved();
				}
			}
			GUILayout.EndHorizontal();
		}

		#endregion methods
	}
}