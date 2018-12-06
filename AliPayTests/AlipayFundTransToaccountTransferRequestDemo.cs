using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliPayTests
{
    public class AlipayFundTransToaccountTransferRequestDemo
    {
        public void Run(ToaccountTransfer toaccountTransfer)
        {

            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "1.0", "RSA2", "alipay_public_key", "GBK", false);
            AlipayFundTransToaccountTransferRequest request = new AlipayFundTransToaccountTransferRequest();
            var bizContent = "{";
            bizContent += $"\"out_biz_no\":\"{toaccountTransfer.out_biz_no}\",";
            bizContent += $"\"payee_type\":\"{(toaccountTransfer.payee_type== "ALIPAY_USERID"? "ALIPAY_USERID":"ALIPAY_LOGONID")}\",";
            bizContent += $"\"payee_account\":\"{toaccountTransfer.payee_account}\",";
            bizContent += $"\"amount\":\"{toaccountTransfer.amount}\",";
            bizContent += $"\"payer_show_name\":\"{toaccountTransfer.payer_show_name}\",";
            bizContent += $"\"payee_real_name\":\"{toaccountTransfer.payee_real_name}\",";
            bizContent += $"\"remark\":\"{toaccountTransfer.remark}\",";
            bizContent += "  }"; 

            request.BizContent = bizContent; 
            AlipayFundTransToaccountTransferResponse response = client.Execute(request);
            Console.WriteLine(response.Body);
        }
    }
}
