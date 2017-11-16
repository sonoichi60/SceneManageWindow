//  ScenesInBuildInfo.cs
//
//  Created by Sonoichi.

using UnityEditor;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// ビルドに含むすべてのシーンの情報
	/// </summary>
	public class ScenesInBuildInfo
	{
		#region variables

		private AllSceneInfo _allSceneInfo;

		public List<int> SceneInBuildIndexes;

		#endregion variables


		#region methods

		public ScenesInBuildInfo( AllSceneInfo allSceneInfo )
		{
			this._allSceneInfo = allSceneInfo;

			var scenesInBuild = EditorBuildSettings.scenes;
			this.SceneInBuildIndexes = new List<int>();
			for( int i = 0 ; i < scenesInBuild.Length ; i++ ) {
				var scene = scenesInBuild[ i ];
				for( int j = 0 ; j < allSceneInfo.SceneCount ; j++ ) {
					var sceneInfo = allSceneInfo.GetSceneInfo( j );
					if( sceneInfo != null && string.Compare( scene.path, sceneInfo.Path ) == 0 ) {
						SceneInBuildIndexes.Add( j );
						break;
					}
				}
			}
		}

		public bool AddSceneInBuildIndex( int sceneIndex )
		{
			if( sceneIndex < 0 || _allSceneInfo.SceneCount <= sceneIndex || SceneInBuildIndexes.Contains( sceneIndex ) ) {
				return false;
			}
			SceneInBuildIndexes.Add( sceneIndex );
			UpdateScenesInBuild();
			return true;
		}

		public bool RemoveSceneInBuildIndex( int sceneIndex )
		{
			if( !SceneInBuildIndexes.Remove( sceneIndex ) ) {
				return false;
			}
			UpdateScenesInBuild();
			return true;
		}

		public void UpdateScenesInBuild()
		{
			var scenes = new EditorBuildSettingsScene[SceneInBuildIndexes.Count];
			for( int i = 0 ; i < scenes.Length ; i++ ) {
				scenes[ i ] = new EditorBuildSettingsScene( _allSceneInfo.GetSceneInfo( SceneInBuildIndexes[ i ] ).Path, true );
			}
			EditorBuildSettings.scenes = scenes;
		}

		#endregion methods
	}
}