using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;



namespace PackageImporter
{
	public class MenuItems
	{

		const string path_Packages = "PackageImporter/";


		[MenuItem(path_Packages + "Select Package List")]
		public static void FocusOnAsset()
		{
			PackagesImporter.FocusOnAsset();
		}

		/*

		[MenuItem(path_Packages + "Import")]
		public static void ImportAllPackages()
		{
			PackageImporter.ImportAllPackages();
		}

		[MenuItem(path_Packages + "Export")]
		public static async void SetPackagesList()
		{
			await PackageExporter.SetPackagesList();
		}*/

	}
}
