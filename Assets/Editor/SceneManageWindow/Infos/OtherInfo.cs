//  OtherInfo.cs
//
//  Created by Sonoichi.

using UnityEngine;
using System;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// SceneGroup, MultiScene関連以外で保持したい情報をまとめた
	/// </summary>
	[Serializable]
	public class OtherInfo
	{
		#region define

		[Serializable]
		public class ListData
		{
			[SerializeField]
			private string _headerText;

			public bool IsFoldout;

			public string HeaderText{ get { return _headerText; } }

			public ListData( string headerText, bool isFoldout = true )
			{
				this._headerText = headerText;
				this.IsFoldout = isFoldout;
			}
		}

		#endregion define


		#region variables

		public bool IsFoldoutOfScenesInProjectView = true;

		public bool IsFoldoutOfSceneGroupsView = true;

		public bool IsFoldoutOfMutiScenesView = true;

		public bool IsFoldoutOfScenesInBuildView = true;

		public List<ListData> ScenesInProjectListDatas = null;

		#if UNITY_2017
		public string StartScenePath = "";
		#endif

		#endregion variables


		#region methods

		public OtherInfo()
		{
			this.IsFoldoutOfScenesInProjectView = true;
			this.IsFoldoutOfSceneGroupsView = true;
			this.IsFoldoutOfMutiScenesView = true;
			this.IsFoldoutOfScenesInBuildView = true;
			this.ScenesInProjectListDatas = new List<ListData>();
			#if UNITY_2017
			this.StartScenePath = "";
			#endif
		}

		public ListData GetScenesInProjectListData( string headerText )
		{
			for( int i = 0 ; i < ScenesInProjectListDatas.Count ; i++ ) {
				var listData = ScenesInProjectListDatas[ i ];
				if( string.Compare( headerText, listData.HeaderText ) == 0 ) {
					return listData;
				}
			}
			return null;
		}

		public void Init( List<string> directoryNames )
		{
			if( ScenesInProjectListDatas == null ) {
				ScenesInProjectListDatas = new List<ListData>();
			}

			var headerTexts = new List<string>();
			for( int i = ScenesInProjectListDatas.Count - 1 ; i >= 0 ; i-- ) {
				var listData = ScenesInProjectListDatas[ i ];
				if( !directoryNames.Contains( listData.HeaderText ) ) {
					ScenesInProjectListDatas.RemoveAt( i );
					continue;
				}
				headerTexts.Add( listData.HeaderText );
			}

			for( int i = 0 ; i < directoryNames.Count ; i++ ) {
				var directoryName = directoryNames[ i ];
				if( !headerTexts.Contains( directoryName ) ) {
					ScenesInProjectListDatas.Add( new ListData( directoryName ) );
				}
			}
		}

		#endregion methods
	}
}