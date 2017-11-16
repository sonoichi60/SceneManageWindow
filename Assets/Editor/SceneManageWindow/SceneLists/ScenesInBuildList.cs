//  ScenesInBuildList.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// ビルドに含むシーンの一覧
	/// </summary>
	public class ScenesInBuildList
	{
		#region define

		private const float INDEX_COLUMN_WIDTH = 40f;

		#endregion define


		#region variables

		private AllSceneInfo _allSceneInfo;

		private ReadOnlyCollection<int> _scenesInBuildIndexes;

		private ReorderableList _reorderableList;

		private Action _onReordered;

		#endregion variables


		#region methods

		public ScenesInBuildList( AllSceneInfo allSceneInfo, List<int> sceneInBuildIndexes, Action onReordered )
		{
			this._allSceneInfo = allSceneInfo;

			this._scenesInBuildIndexes = sceneInBuildIndexes.AsReadOnly();
			this._reorderableList = new ReorderableList( sceneInBuildIndexes, typeof( int ), true, false, false, false ) {
				drawHeaderCallback = DrawHeader,
				drawElementCallback = DrawElement,
				onReorderCallback = OnReorderElement,
			};

			this._onReordered = onReordered;
		}

		public void Draw()
		{
			GUILayout.BeginVertical();
			{
				_reorderableList.DoLayoutList();
			}
			GUILayout.EndVertical();
		}

		private void DrawHeader( Rect rect )
		{
			EditorGUILayout.BeginHorizontal();
			{
				var style = new GUIStyle( EditorStyles.label );
				style.alignment = TextAnchor.MiddleLeft;

				var nameRect = new Rect( rect ) {
					xMin = rect.xMin + 16f,
					xMax = rect.xMax - INDEX_COLUMN_WIDTH,
				};
				EditorGUI.LabelField( nameRect, "SceneName", style );

				style.alignment = TextAnchor.MiddleRight;
				var indexRect = new Rect( rect ) {
					xMin = rect.xMax - INDEX_COLUMN_WIDTH,
				};
				EditorGUI.LabelField( indexRect, "Index", style );

				var borderRect = new Rect( rect ) { 
					xMin = rect.xMax - INDEX_COLUMN_WIDTH,
					xMax = rect.xMax - INDEX_COLUMN_WIDTH + 1,
				};
				EditorGUI.DrawRect( borderRect, Color.gray );
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawElement( Rect rect, int index, bool active, bool focused )
		{
			var sceneInfo = _allSceneInfo.GetSceneInfo( _scenesInBuildIndexes[ index ] );
			if( sceneInfo == null ) {
				return;
			}

			GUILayout.BeginHorizontal();
			{
				var style = new GUIStyle( EditorStyles.label );
				style.alignment = TextAnchor.MiddleLeft;

				var nameRect = new Rect( rect ) {
					xMin = rect.xMin,
					xMax = rect.xMax - INDEX_COLUMN_WIDTH,
				};
				EditorGUI.LabelField( nameRect, sceneInfo.Name, style );

				style.alignment = TextAnchor.MiddleRight;
				var indexRect = new Rect( rect ) {
					xMin = rect.xMax - INDEX_COLUMN_WIDTH,
				};
				EditorGUI.LabelField( indexRect, index.ToString(), style );
			}
			GUILayout.EndHorizontal();
		}

		private void OnReorderElement( ReorderableList list )
		{
			if( _onReordered != null ) {
				_onReordered();
			}
		}

		#endregion methods
	}
}