using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using LLYTPay;

/// <summary>
/// 用户已绑定银行卡信息查询 卡bin校验查询
/// </summary>
/// 
public partial class infoQuery : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {


		string responseJson = queryCardBin();

		Response.Write (responseJson);
    }


	/// <summary>
	/// 卡bin校验查询
	/// </summary>
	/// 
	public string queryCardBin()
	{
		SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string> ();
		sParaTemp.Add ("oid_partner", PartnerConfig.OID_PARTNER);
		sParaTemp.Add ("card_no", "6222081202888888888");
		sParaTemp.Add ("sign_type", PartnerConfig.SIGN_TYPE);
		string sign = YinTongUtil.addSign (sParaTemp, PartnerConfig.TRADER_PRI_KEY, PartnerConfig.MD5_KEY);
		sParaTemp.Add ("sign", sign);

		string reqJson = YinTongUtil.dictToJson (sParaTemp);

		Console.WriteLine ("银行卡卡bin信息查询-请求报文[" + reqJson + "]");
		string responseJSON = postJson (ServerURLConfig.QUERY_BANKCARD_URL, reqJson);
		Console.WriteLine ("银行卡卡bin信息查询-响应报文[" + responseJSON + "]");
		return responseJSON;
	}

	/// <summary>
	/// pos方法
	/// </summary>
	/// 
	public string postJson(string serverUrl , string reqJson)
	{
		var http = (HttpWebRequest)WebRequest.Create(new Uri(serverUrl));
		http.Accept = "application/json";
		http.ContentType = "application/json";
		http.Method = "POST";

		ASCIIEncoding encoding = new ASCIIEncoding();
		Byte[] bytes = encoding.GetBytes(reqJson);

		Stream newStream = http.GetRequestStream();
		newStream.Write(bytes, 0, bytes.Length);
		newStream.Close();

		var response = http.GetResponse();

		var stream = response.GetResponseStream();
		var sr = new StreamReader(stream);
		var content = sr.ReadToEnd();

		return content;
	}


}


