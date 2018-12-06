using Senparc.Weixin;
using Senparc.Weixin.CommonAPIs;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.QY.TenPayLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace WxPay
{
    public class Transfers
    {
        /// <summary> 
        /// 提现转账 
        /// </summary> 
        /// <param name="orderId"></param> 
        /// <param name="money"></param> 
        /// <param name="openId"></param> 
        /// <param name="checkName">【NO_CHECK：不校验真实姓名 FORCE_CHECK：强校验真实姓 OPTION_CHECK：针对已实名认证的用户才校验真实姓名】</param> 
        /// <param name="reUserName"></param> 
        /// <param name="spbillCreateIp"></param> 
        /// <param name="desc"></param> 
        /// <param name="nonceStr"></param> 
        public void WithdrawMoney(string orderId, decimal money, string openId, string checkName, string reUserName,
            string spbillCreateIp, string desc,
            string nonceStr = null)
        {
            //创建支付应答对象 
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化 
            packageReqHandler.Init();

            nonceStr = string.IsNullOrEmpty(nonceStr) ? TenPayUtil.GetNoncestr() : nonceStr;

            //设置package订单参数 
            packageReqHandler.SetParameter("mch_appid", "AppId"); //公众账号ID 
            packageReqHandler.SetParameter("mchid", "MchId"); //商户号 
            packageReqHandler.SetParameter("nonce_str", nonceStr); //随机字符串 
            packageReqHandler.SetParameter("desc", desc); //企业付款描述信息 
            packageReqHandler.SetParameter("check_name", checkName); //校验用户姓名选项 
            packageReqHandler.SetParameter("re_user_name", reUserName); //收款用户姓名 
            packageReqHandler.SetParameter("partner_trade_no", orderId); //商户订单号 
            packageReqHandler.SetParameter("amount", (money * 100).ToString("0")); //转账金额,以分为单位(money * 100).ToString() 
            packageReqHandler.SetParameter("spbill_create_ip", spbillCreateIp); //用户的公网ip，不是商户服务器IP 
            packageReqHandler.SetParameter("openid", openId); //用户的openId 
            string sign = packageReqHandler.CreateMd5Sign("key", "Key");
            packageReqHandler.SetParameter("sign", sign); //签名 
            string data = packageReqHandler.ParseXML();
            #region transfers() 

            //发企业支付接口地址 
            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

            //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中） 
            string cert = @"D:\apiclient_cert.p12";
            //私钥（在安装证书时设置） 
            string password = "MchId";
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(CheckValidationResult);
            //调用证书 
            X509Certificate2 cer = new X509Certificate2(cert, password,
                X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            #region 发起post请求 

            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";

            byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
            webrequest.ContentLength = postdatabyte.Length;
            Stream stream;
            stream = webrequest.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string result = streamReader.ReadToEnd();

            #endregion

            #endregion

            var res = XDocument.Parse(result);

            if (res.Element("xml") == null)
            {
                throw new Exception("转账订单接口出错");
            }

            var returnCode = res.Element("xml").Element("return_code") == null
                ? null
                : res.Element("xml").Element("return_code").Value;
            var resultCode = res.Element("xml").Element("result_code") == null
                ? null
                : res.Element("xml").Element("result_code").Value;
            if (string.IsNullOrEmpty(returnCode))
            {
                throw new Exception("转账订单接口出错，未获取到返回状态码");
            }
            if (returnCode == "FAIL" || resultCode == "FAIL")
            {
                var returnMsg = res.Element("xml").Element("return_msg").ToString();
                throw new Exception(returnMsg);
            }
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }


        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <param name="appId">公众号的唯一标识</param>
        /// <param name="redirectUrl">授权后重定向的回调链接地址，请使用urlencode对链接进行处理</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值，最多128字节</param>
        /// <param name="scope">应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）</param>
        /// <param name="responseType">返回类型，请填写code（或保留默认）</param>
        /// <param name="addConnectRedirect">加上后可以解决40029-invalid code的问题（测试中）</param>
        /// <returns></returns>
        public static string GetAuthorizeUrl(string appId, string redirectUrl, string state, string scope, string responseType = "code", bool addConnectRedirect = true)
        {
            var url =
              string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}{5}#wechat_redirect",
                      appId.AsUrlData(), redirectUrl.AsUrlData(), responseType.AsUrlData(), scope.ToString("g").AsUrlData(), state.AsUrlData(),
                      addConnectRedirect ? "&connect_redirect=1" : "");


            /* 这一步发送之后，客户会得到授权页面，无论同意或拒绝，都会返回redirectUrl页面。
             * 如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&state=STATE。这里的code用于换取access_token（和通用接口的access_token不通用）
             * 若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
             */
            return url;
        }
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId">公众号的唯一标识</param>
        /// <param name="secret">公众号的appsecret</param>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <param name="grantType">填写为authorization_code（请保持默认参数）</param>
        /// <returns></returns>
        public static OAuthAccessTokenResult GetAccessToken(string appId, string secret, string code, string grantType = "authorization_code")
        {
            var url =
              string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}",
                      appId.AsUrlData(), secret.AsUrlData(), code.AsUrlData(), grantType.AsUrlData());


            return CommonJsonSend.Send<OAuthAccessTokenResult>(null, url, null, CommonJsonSendType.GET);
        }
    }
    public class OAuthAccessTokenResult
    {
        public string openid { set; get; }
        public string Token { set; get; }
    }
}
