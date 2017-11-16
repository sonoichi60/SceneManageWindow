//  MultiScenesInfo.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SceneManageWindow
{
	/// <summary>
	/// すべてのマルチシーンの情報
	/// </summary>
	[Serializable]
	public class MultiScenesInfo
	{
		#region variables

		[SerializeField]
		private List<MultiSceneInfo> _multiScenes;

		public int MultiSceneCount { get { return _multiScenes.Count; } }

		public ReadOnlyCollection<MultiSceneInfo> MultiScenes{ get { return _multiScenes.AsReadOnly(); } }

		#endregion variables


		#region methods

		public MultiScenesInfo()
		{
			_multiScenes = new List<MultiSceneInfo>();
		}

		public MultiSceneInfo GetMultiScene( string multiSceneName )
		{
			if( string.IsNullOrEmpty( multiSceneName ) ) {
				return null;
			}

			for( int i = 0 ; i < MultiScenes.Count ; i++ ) {
				if( string.Compare( multiSceneName, MultiScenes[ i ].Name ) == 0 ) {
					return MultiScenes[ i ];
				}
			}

			return null;
		}

		public bool AddMultiScene( string multiSceneName, string mainScenePath, List<string>subScenePaths )
		{
			if( subScenePaths == null || subScenePaths.Count == 0 ) {
				return false;
			}

			if( !IsUsableMultiSceneName( multiSceneName ) ) {
				EditorUtility.DisplayDialog( "Error!", string.Format( "Invalid multi-scene name. : \"{0}\"", multiSceneName ), "OK" );
				return false;
			}

			var multipleSceneInfo = new MultiSceneInfo( multiSceneName, mainScenePath, subScenePaths );
			_multiScenes.Add( multipleSceneInfo );
			return true;
		}

		public bool RemoveMultiScene( string multiSceneName )
		{
			var multipleSceneInfo = GetMultiScene( multiSceneName );
			if( multipleSceneInfo == null ) {
				return false;
			}
			return _multiScenes.Remove( multipleSceneInfo );
		}

		public void ReplacePathInAllMultiScene( string prePath, string newPath )
		{
			for( int i = 0 ; i < _multiScenes.Count ; i++ ) {
				_multiScenes[ i ].ReplacePath( prePath, newPath );
			}
		}

		private bool IsUsableMultiSceneName( string multiSceneName )
		{
			if( string.IsNullOrEmpty( multiSceneName ) ) {
				return false;
			}

			var invalidChars = Path.GetInvalidFileNameChars();
			if( multiSceneName.IndexOfAny( invalidChars ) >= 0 ) {
				return false;
			}

			for( int i = 0 ; i < _multiScenes.Count ; i++ ) {
				if( string.Compare( _multiScenes[ i ].Name, multiSceneName ) == 0 ) {
					return false;
				}
			}

			return true;
		}

		public void Clean( List<string> paths )
		{
			for( int i = _multiScenes.Count - 1 ; i >= 0 ; i-- ) {
				var multiScene = _multiScenes[ i ];
				if( !paths.Contains( multiScene.MainScenePath ) ) {
					this._multiScenes.Remove( multiScene );
					continue;
				}
				for( int j = multiScene.SubScenePaths.Count - 1 ; j >= 0 ; j-- ) {
					var subScenePath = multiScene.SubScenePaths[ j ];
					if( !paths.Contains( subScenePath ) ) {
						multiScene.RemoveSubScene( subScenePath );
						if( multiScene.SubSceneCount == 0 ) {
							_multiScenes.Remove( multiScene );
						}
					}
				}
			}
		}

		public string[] GetMultiSceneNames()
		{
			return _multiScenes.Select( multipleScene => multipleScene.Name ).ToArray();
		}

		#endregion methods
	}
}