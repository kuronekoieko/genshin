using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;



namespace IekoLibrary
{
	public class MenuItems : MonoBehaviour
	{
		const string path_IekoLibrary = "IekoLibrary/";
		const string path_AddNameSpace = path_IekoLibrary + "AddNameSpace/";


		[MenuItem(path_AddNameSpace + "folder")]
		static void AddNameSpaces()
		{
			AddNameSpace.Exec();
		}

		[MenuItem(path_AddNameSpace + "script")]
		static void AddNameSpacesTest()
		{
			AddNameSpace.ExecTest();
		}



	}
}
