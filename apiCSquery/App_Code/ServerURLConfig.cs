using System;

/**
* 请求服务地址 配置
*
*/

namespace LLYTPay
{
	public class ServerURLConfig
	{
		public static string PAY_URL = "https://yintong.com.cn/payment/authpay.htm"; // 连连支付WEB收银台支付服务地址
		public static string QUERY_USER_BANKCARD_URL = "https://yintong.com.cn/queryapi/userbankcard.htm"; // 用户已绑定银行卡列表查询
		public static string QUERY_BANKCARD_URL = "https://yintong.com.cn/queryapi/bankcardquery.htm"; //银行卡卡bin信息查询
	}
}

