using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliPayTests
{
    class Program
    {
        static void Main(string[] args)
        {
            ToaccountTransfer toaccountTransfer = new ToaccountTransfer
            {
                out_biz_no = "3142321423432",
                payee_type = "ALIPAY_LOGONID",
                payee_account = "abc@sina.com",
                amount = "12.23",
                payer_show_name = "上海交通卡退款",
                payee_real_name = "张三",
                remark = "转账备注"
            }; 
            AlipayFundTransToaccountTransferRequestDemo alipayFundTransToaccountTransferRequestDemo = new AlipayFundTransToaccountTransferRequestDemo();
            alipayFundTransToaccountTransferRequestDemo.Run(toaccountTransfer);

        }
    }
}
