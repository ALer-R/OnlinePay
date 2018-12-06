using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliPayTests
{
  public  class ToaccountTransfer
    {
        /// <summary>
        /// 必选	64	商户转账唯一订单号。发起转账来源方定义的转账单据ID，用于将转账回执通知给来源方。 
        /// </summary>
        /// <remarks>
        /// 不同来源方给出的ID可以重复，同一个来源方必须保证其ID的唯一性。 
        /// 只支持半角英文、数字，及“-”、“_”。	3142321423432   
        /// </remarks>
        public string out_biz_no { get; set; }
        /// <summary>
        /// 必选	20	收款方账户类型。可取值： 
        /// </summary>
        /// <remarks>
        /// 1、ALIPAY_USERID：支付宝账号对应的支付宝唯一用户号。以2088开头的16位纯数字组成。 
        /// 2、ALIPAY_LOGONID：支付宝登录号，支持邮箱和手机号格式。	ALIPAY_LOGONID
        /// </remarks>
        public string payee_type { get; set; }
        /// <summary>
        /// 必选	100	收款方账户。与payee_type配合使用。付款方和收款方不能是同一个账户。	abc @sina.com
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public string payee_account { get; set; }
        /// <summary>
        ///  必选  16	转账金额，单位：元。 
        /// </summary>
        /// <remarks>
        /// 当付款方为企业账户，且转账金额达到（大于等于）50000元，remark不能为空。收款方可见，会展示在收款用户的收支详情中。
        /// </remarks>
        public string amount { get; set; }
        /// <summary>
        /// 付款方姓名（最长支持100个英文/50个汉字）。显示在收款方的账单详情页。如果该字段不传，则默认显示付款方的支付宝认证姓名或单位名称。	上海交通卡退款
        /// </summary>
        /// <remarks>
        /// 只支持2位小数，小数点前最大支持13位，金额必须大于等于0.1元。  最大转账金额以实际签约的限额为准。	12.23
        /// </remarks>
        public string payer_show_name { get; set; }
        /// <summary>
        /// 可选	100	收款方真实姓名（最长支持100个英文/50个汉字）。 
        /// </summary>
        /// <remarks>
        /// 如果本参数不为空，则会校验该账户在支付宝登记的实名是否与收款方真实姓名一致。	张三 
        /// </remarks>
        public string payee_real_name { get; set; }
        /// <summary>
        /// 可选	200	转账备注（支持200个英文/100个汉字）。
        /// </summary>
        /// <remarks>
        /// 当付款方为企业账户，且转账金额达到（大于等于）50000元，remark不能为空。收款方可见，会展示在收款用户的收支详情中。
        /// </remarks>
        public string remark { get; set; }
        
    }
}
