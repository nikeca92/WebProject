using System;
using System.Web;

namespace PMIS.ExceptionHandler
{
    /*IIS7: Use Classic app pool to be able to load the HttpModule*/
	public class UehHttpModule : IHttpModule 
	{ 
		public void Init(System.Web.HttpApplication Application) 
		{ 
			Application.Error += new EventHandler(OnError); 
		} 

		public void Dispose() 
		{ 
		} 

		protected virtual void OnError(object sender, EventArgs args) 
		{ 
			HttpApplication app = ((HttpApplication)(sender)); 
			Handler ueh = new Handler(); 
			ueh.HandleException(app.Server.GetLastError()); 
		} 
	}
}
