using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using WxPay.Tools;

namespace WxPay
{
    class Program
    {
        static void Main(string[] args)
        {
            new SendWorkwxRedpack().Run();

        }
    }
}
