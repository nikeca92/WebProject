using System;
using System.Configuration; 
using System.Collections.Specialized; 
using System.IO; 

namespace PMIS.ExceptionHandler
{
    //Use this class to read the relevant settings from the web.config file.
    //The settings are places in a separate section called UnhandledException
	internal class Config 
	{ 
		private const string _strSectionName = "UnhandledException"; 
		private static NameValueCollection _nvc; 

        //Load the UnhandledException section from the web.config file
		private static void Load() 
		{ 
			if (!(_nvc == null)) 
			{ 
				return; 
			} 

			object o=null; 

			try 
			{ 
				o = ConfigurationManager.GetSection(_strSectionName); 
			} 
			catch 
			{ 
			} 

			if (o == null) 
			{ 
				_nvc = new NameValueCollection(); 
				return; 
			} 

			try 
			{ 
				_nvc = ((NameValueCollection)(o)); 
			} 
			catch (Exception ex) 
			{ 
				throw new ConfigurationErrorsException("The <" + _strSectionName + "> section is present in the .config file, but it does not appear to be a name value collection.", ex); 
			} 
		} 

        //Read an integer value
		public static int GetInteger(string Key, int Default) 
		{ 
			Load();

			string strTemp = ""; 

			if ( _nvc[Key] != null )
				strTemp = _nvc[Key]; 
			
			if (strTemp == "") 
			{ 
				return Default; 
			} 
			try 
			{ 
				return Convert.ToInt32(strTemp); 
			} 
			catch
			{ 
				return Default; 
			} 
		} 

        //Read a boolean value
		public static bool GetBoolean(string Key, bool Default) 
		{ 
			Load(); 
			string strTemp = ""; 

			if ( _nvc[Key] != null )
				strTemp = _nvc[Key]; 

			if (strTemp == "") 
			{ 
				return Default; 
			} 
			if (strTemp.ToLower() == "1" || strTemp.ToLower() == "true") 
			{ 
				return true; 
			} 
			else 
			{ 
				return false; 
			} 
		} 

        //Read a string value
		public static string GetString(string Key, string Default) 
		{ 
			Load(); 
			
			string strTemp = ""; 

			if ( _nvc[Key] != null )
				strTemp = _nvc[Key]; 

			if (strTemp == "") 
			{ 
				return Default; 
			} 
			return strTemp; 
		} 

        //Read a path value
		public static string GetPath(string Key) 
		{ 
			Load(); 
			string strPath = GetString(Key, ""); 
			if (strPath.StartsWith("~/")) 
			{ 
				strPath = strPath.Replace("~/", ""); 
			} 
			if (Path.IsPathRooted(strPath)) 
			{ 
				return strPath; 
			} 
			else 
			{ 
				return Path.Combine(AppBase, strPath); 
			} 
		} 

        //Get the app base 
		private static string AppBase 
		{ 
			get 
			{ 
				return Convert.ToString(System.AppDomain.CurrentDomain.GetData("APPBASE")); 
			} 
		} 
	}
}
