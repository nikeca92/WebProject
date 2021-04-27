using System.IO; 
using System.Web.Services.Protocols; 

namespace PMIS.ExceptionHandler
{
	public class UehSoapExtension : SoapExtension 
	{ 
		private Stream _OldStream; 
		private Stream _NewStream; 

		public override object GetInitializer(System.Type serviceType) 
		{ 
			return null; 
		} 

		public override object GetInitializer(System.Web.Services.Protocols.LogicalMethodInfo methodInfo, System.Web.Services.Protocols.SoapExtensionAttribute attribute) 
		{ 
			return null; 
		} 

		public override void Initialize(object initializer) 
		{ 
		} 

		public override Stream ChainStream(Stream stream) 
		{ 
			_OldStream = stream; 
			_NewStream = new MemoryStream(); 
			return _NewStream; 
		} 

		private void Copy(Stream fromStream, Stream toStream) 
		{ 
			StreamReader sr = new StreamReader(fromStream); 
			StreamWriter sw = new StreamWriter(toStream); 
			sw.Write(sr.ReadToEnd()); 
			sw.Flush(); 
		} 

		public override void ProcessMessage(System.Web.Services.Protocols.SoapMessage message) 
		{ 
			if (message.Stage == SoapMessageStage.BeforeDeserialize) 
			{ 
				Copy(_OldStream, _NewStream); 
				_NewStream.Position = 0; 
			} 
			else if (message.Stage == SoapMessageStage.AfterSerialize) 
			{ 
				if (!(message.Exception == null)) 
				{ 
					Handler ueh = new Handler(); 
					string strDetailNode; 
					strDetailNode = ueh.HandleWebServiceException(message); 
					_NewStream.Position = 0; 
					TextReader tr = new StreamReader(_NewStream); 
					string s = tr.ReadToEnd(); 
					s = s.Replace("<detail />", strDetailNode); 
					_NewStream = new MemoryStream(); 
					TextWriter tw = new StreamWriter(_NewStream); 
					tw.Write(s); 
					tw.Flush(); 
				}
 
				_NewStream.Position = 0; 
				Copy(_NewStream, _OldStream); 
			} 
		} 
	}
}