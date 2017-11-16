//  SceneGroupsInfo.cs
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
	/// すべてのシーングループの情報
	/// </summary>
	[Serializable]
	public class SceneGroupsInfo
	{
		#region variables

		[SerializeField]
		private List<SceneGroupInfo> _sceneGroups;

		public int SceneGroupCount { get { return _sceneGroups.Count; } }

		public ReadOnlyCollection<SceneGroupInfo> SceneGroups{ get { return _sceneGroups.AsReadOnly(); } }

		#endregion variables


		#region methods

		public SceneGroupsInfo()
		{
			this._sceneGroups = new List<SceneGroupInfo>();
		}

		public SceneGroupInfo GetSceneGroupInfo( string groupName )
		{
			if( string.IsNullOrEmpty( groupName ) ) {
				return null;
			}

			for( int i = 0 ; i < _sceneGroups.Count ; i++ ) {
				if( string.Compare( groupName, _sceneGroups[ i ].Name ) == 0 ) {
					return _sceneGroups[ i ];
				}
			}

			return null;
		}

		public bool AddSceneGroup( string groupName, List<string> paths )
		{
			if( paths == null || paths.Count == 0 ) {
				return false;
			}

			if( !IsUsableGroupName( groupName ) ) {
				EditorUtility.DisplayDialog( "Error!", string.Format( "Invalid SceneGroup name. : \"{0}\"", groupName ), "OK" );
				return false;
			}

			var sceneGroupInfo = new SceneGroupInfo( groupName, paths );
			_sceneGroups.Add( sceneGroupInfo );
			return true;
		}

		public bool RemoveSceneGroup( string groupName )
		{
			var sceneGroupInfo = GetSceneGroupInfo( groupName );
			if( sceneGroupInfo == null ) {
				return false;
			}

			return _sceneGroups.Remove( sceneGroupInfo );
		}

		public void RepalcePathInAllSceneGroup( string prePath, string newPath )
		{
			for( int i = 0 ; i < _sceneGroups.Count ; i++ ) {
				_sceneGroups[ i ].ReplacePath( prePath, newPath );
			}
		}

		private bool IsUsableGroupName( string groupName )
		{
			if( string.IsNullOrEmpty( groupName ) ) {
				return false;
			}

			var invalidChars = Path.GetInvalidFileNameChars();
			if( groupName.IndexOfAny( invalidChars ) >= 0 ) {
				return false;
			}


			for( int i = 0 ; i < _sceneGroups.Count ; i++ ) {
				if( string.Compare( _sceneGroups[ i ].Name, groupName ) == 0 ) {
					return false;
				}
			}
			return true;
		}

		public void Clean( List<string> paths )
		{
			for( int i = _sceneGroups.Count - 1 ; i >= 0 ; i-- ) {
				var sceneGroup = _sceneGroups[ i ];
				for( int j = sceneGroup.Paths.Count - 1 ; j >= 0 ; j-- ) {
					var path = sceneGroup.Paths[ j ];
					if( !paths.Contains( path ) ) {
						sceneGroup.RemoveScece( path );
						if( sceneGroup.Paths.Count == 0 ) {
							_sceneGroups.Remove( sceneGroup );
						}
					}
				}
			}
		}

		public string[] GetSceneGroupNames()
		{
			return _sceneGroups.Select( sceneGroup => sceneGroup.Name ).ToArray();
		}

		#endregion methods
	}
}