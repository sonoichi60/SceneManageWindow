//  SceneInfo.cs
//
//  Created by Sonoichi.

using System;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SceneManageWindow
{
	/// <summary>
	/// シーン単体の情報
	/// </summary>
	public class SceneInfo
	{
		#region variables

		public string Name{ get { return System.IO.Path.GetFileNameWithoutExtension( Path ); } }

		public string DirectoryName{ get { return System.IO.Path.GetDirectoryName( Path ); } }

		public string Path{ get; private set; }

		#endregion variables


		#region methods

		public SceneInfo( string path )
		{
			this.Path = path;
		}

		public bool Load( OpenSceneMode mode = OpenSceneMode.Single )
		{
			if( !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() ) {
				return false;
			}

			EditorSceneManager.OpenScene( Path, mode );
			return true;
		}

		public bool Rename( string newName )
		{
			if( !string.IsNullOrEmpty( AssetDatabase.RenameAsset( Path, newName ) ) ) {
				return false;
			}

			this.Path = string.Format( "{0}/{1}.unity", DirectoryName, newName );
			return true;
		}

		#endregion methods
	}
}