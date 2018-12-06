using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WxPay.Tools;

namespace WxPay
{
  public  class PayWwsptrans2Pocket
    {
        public void Run()
        {
            WxPayData data = new WxPayData();
            data.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());

            data.SetValue("partner_trade_no", WxPayApi.GenerateOutTradeNo());//商户订单号
            //string openid = ConvertToOpenidByUserId(_accessToken, "13212345678");
            string openid = WxPayTools.ConvertToOpenidByUserId(WxPayTools.GetAccessoken(), "1234567890");
            var openInfo = JsonConvert.DeserializeObject<U_OpenInfo>(openid);
            data.SetValue("openid", openInfo.openid);    //商户appid下，某用户的openid
            data.SetValue("check_name", "NO_CHECK");    //校验用户姓名选项(NO_CHECK：不校验真实姓名FORCE_CHECK：强校验真实姓名)
            data.SetValue("amount", 100);               //金额，单位为分
            data.SetValue("desc", "六月份出差报销");//付款说明 
            data.SetValue("spbill_create_ip", "192.168.0.1");//Ip地址
            data.SetValue("ww_msg_type", "NORMAL_MSG");    //付款消息类型
            data.SetValue("act_name", "示例项目");   //项目名称
            data.SetValue("workwx_sign", data.MakeWorkWxSign("payment"));  //企业微信签名
            data.SetValue("sign", data.MakeSign());            //微信支付签名
            string xml = data.ToXml();
            const string postUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/paywwsptrans2pocket";
            string response = WxPayTools.PostWebRequest(postUrl, xml, Encoding.UTF8, true);
            WxPayData result = new WxPayData();
            result.FromXml(response);
        }

    }
    public class U_OpenInfo
    {
        public string openid { get; set; }
    }
}
