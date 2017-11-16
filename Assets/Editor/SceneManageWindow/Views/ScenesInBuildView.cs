//  ScenesInBuildView.cs
//
//  Created by Sonoichi.

namespace SceneManageWindow
{
	/// <summary>
	/// ビルドに含むシーンのビュー
	/// </summary>
	public class ScenesInBuildView : FoldoutableViewBase
	{
		#region define

		private const string SCENES_IN_BUILD_VIEW_TITLE = "Scenes In Build";

		#endregion define


		#region variables

		private MySceneManager _sceneManager;

		private ScenesInBuildList _scenesInBuildList;

		#endregion variables


		#region methods

		public ScenesInBuildView( MySceneManager sceneManager ) : base( SCENES_IN_BUILD_VIEW_TITLE, sceneManager.OtherInfo.IsFoldoutOfScenesInBuildView )
		{
			this._sceneManager = sceneManager;
			this._scenesInBuildList = new ScenesInBuildList( _sceneManager.AllSceneInfo, _sceneManager.ScenesInBuildInfo.SceneInBuildIndexes, OnScenesInBuildListReodered );
		}

		protected override void DrawViewDetail()
		{
			_scenesInBuildList.Draw();
		}

		protected override void OnFoldoutToggleChanged( bool isOn )
		{
			_sceneManager.OtherInfo.IsFoldoutOfScenesInBuildView = isOn;
			_sceneManager.Save();
		}

		private void OnScenesInBuildListReodered()
		{
			_sceneManager.ScenesInBuildInfo.UpdateScenesInBuild();
		}

		#endregion methods
	}
}