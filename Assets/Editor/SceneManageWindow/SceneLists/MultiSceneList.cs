//  MultiSceneList.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// マルチシーンの一覧
	/// </summary>
	public class MultiSceneList: SceneListBase
	{
		#region variables

		private string _multiSceneName;

		private Action<string> _onMultiSceneRemoved;

		private Action<string> _onMultiSceneLoaded;

		private Action<string, int> _onElementRemoved;

		private Action<string, bool> _onFoldouToggleChanged;

		#endregion variables

		#region methods

		public MultiSceneList( string headerText, string multiSceneName, List<int> subSceneIndexes, AllSceneInfo allSceneInfo, bool isFoldout ) : base( headerText, subSceneIndexes, allSceneInfo, isFoldout )
		{
			this._multiSceneName = multiSceneName;
		}

		public void RegistorCallback( Action<string> onMultiSceneRemoved, Action<string> onMultiSceneLoaded, Action<string, int> onElementRemoved, Action<string, bool> onFoldoutToggleChanged )
		{
			this._onMultiSceneRemoved = onMultiSceneRemoved;
			this._onMultiSceneLoaded = onMultiSceneLoaded;
			this._onElementRemoved = onElementRemoved;
			this._onFoldouToggleChanged = onFoldoutToggleChanged;
		}

		protected override void DrawHeaderDetail( GUIStyle style )
		{
			if( _onMultiSceneRemoved != null ) {
				DrawButton( "Remove", style, () => _onMultiSceneRemoved( _multiSceneName ) );
			}

			base.DrawHeaderDetail( style );
			if( _onMultiSceneLoaded != null ) {
				EditorGUI.BeginDisabledGroup( EditorApplication.isPlaying );
				{
					DrawButton( "Load", style, () => _onMultiSceneLoaded( _multiSceneName ) );
				}
				EditorGUI.EndDisabledGroup();
			}
		}

		protected override void DrawElementDetail( int index, SceneInfo sceneInfo, GUIStyle style )
		{
			if( _onElementRemoved != null ) {
				DrawButton( "Remove", style, () => _onElementRemoved( _multiSceneName, index ) );
			}

			base.DrawElementDetail( index, sceneInfo, style );
		}

		protected override void OnFoldoutToggleChanged( bool isOn )
		{
			if( _onFoldouToggleChanged != null ) {
				_onFoldouToggleChanged( _multiSceneName, isOn );
			}
		}

		#endregion methods
	}
}