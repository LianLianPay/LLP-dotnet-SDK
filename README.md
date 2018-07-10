# LLP-dotnet-SDK

欢迎来到连连， 本仓库中包含使用```C#```请求连连的服务器端API时的示例工程及使用说明， 示例工程中含有收款结果查询API的请求示例。

## 主要内容

* [前置要求](#前置要求)

* [使用说明](#使用说明)

* [签名说明](#签名说明)

* [延伸阅读](#延伸阅读)


## 前置要求

暂无

## 使用说明

您的服务器可通过HTTP协议直接请求连连的服务器端API， 要求如下:

* HTTP请求的媒体类型应设置为:

```text
Content-type: application/json;charset=utf-8
```

* 请求方法应为```POST```。

* 需使用[HTTPS](https://baike.baidu.com/item/https/285356?fr=aladdin)协议。

* 在您与连连的交互中， 所有的信息须进行签名处理。

## 签名说明

首先生成签名原串， 如示例工程```App_Code```目录下```YinTongUtil.cs```的```genSignData()```方法:

```cs
// 生成签名原串
public static String genSignData(SortedDictionary<string, string> sParaTemp)
{
	StringBuilder content = new StringBuilder();
	foreach (KeyValuePair<string, string> temp in sParaTemp)
	{
		//"sign"不参与签名
		if ("sign".Equals(temp.Key))
		{
			continue;
		}
		// 空串不参与签名
		if (isnull(temp.Value))
		{
			continue;
		}
		content.Append("&" + temp.Key + "=" + temp.Value);
	}
	String signSrc = content.ToString();
	if (signSrc.StartsWith("&"))
	{
		signSrc = signSrc.Substring (1);
	}
	return signSrc;
}
```


加签时，使用示例工程```App_Code```目录下```YinTongUtil.cs```的```addSignRSA()```方法进行加签，其中```rsa_private```即为您的私钥，```sign_src```为签名原串：

```cs
//RSA签名
private static String addSignRSA(SortedDictionary<string, string> sParaTemp, String rsa_private)
{
	string oid_partner;
	sParaTemp.TryGetValue ("sign_type", out oid_partner);
	Console.WriteLine("进入商户[" + oid_partner + "]MD5加签名");

	if (sParaTemp == null)
	{
		return "";
	}
	// 生成签名原串
	String sign_src = genSignData(sParaTemp);
	Console.WriteLine("商户[" + oid_partner + "]加签原串"
		+ sign_src);
	Console.WriteLine("RSA签名key:" + rsa_private);
	try
	{
		string sign = RSAFromPkcs8.sign(sign_src,rsa_private,"utf-8");
		Console.WriteLine("商户[" + oid_partner + "]签名结果"
			+ sign);
		return sign;
	} catch (Exception e)
	{
		Console.WriteLine("商户[" + oid_partner + "]RSA加签名异常" + e.Message);
		return "";
	}
}
```

验签时，使用示例工程```App_Code```目录下```YinTongUtil.cs```的```checkSignRSA()```方法进行验签，其中```sign```连连向您发送的请求报文中的签名值，```sign_src```为签名原串， ```rsa_public```为连连向您提供的公钥：

```cs
//RSA验签
private static bool checkSignRSA(SortedDictionary<string, string> sParaTemp, String rsa_public)
{

	string oid_partner;
	sParaTemp.TryGetValue ("sign_type", out oid_partner);
	Console.WriteLine("进入商户[" + oid_partner + "]MD5签名验证");

	if (sParaTemp == null)
	{
		return false;
	}
	String sign;
	if (!sParaTemp.TryGetValue ("sign", out sign))   
	{
		return false;
	}

	// 生成签名原串
	String sign_src = genSignData(sParaTemp);

	Console.WriteLine("商户[" + oid_partner + "]待签名原串"
		+ sign_src);
	Console.WriteLine("商户[" + oid_partner + "]签名串"
		+ sign);
	try
	{
		if (RSAFromPkcs8.verify(sign_src,sign,rsa_public,"UTF-8" ))
		{
			Console.WriteLine("商户[" + oid_partner
				+ "]RSA签名验证通过");
			return true;
		} else
		{
			Console.WriteLine("商户[" + oid_partner
				+ "]RSA签名验证未通过");
			return false;
		}
	} catch (Exception e)
	{
		Console.WriteLine("商户[" + oid_partner
			+ "]RSA签名验证异常" + e.Message);
		return false;
	}
}
```

## 延伸阅读

[连连开放平台 - API文档 - 新手指南](https://openllp.lianlianpay.com/apis/get-started)

[连连开放平台 - 签名机制](https://openllp.lianlianpay.com/docs/development/signature-overview)