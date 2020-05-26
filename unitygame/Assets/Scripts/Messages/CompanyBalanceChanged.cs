using Utils;

namespace Messages
{
    public class CompanyBalanceChanged : GenericTinyMessage<float>
    {
        public CompanyBalanceChanged(object sender, float content) : base(sender, content)
        {
        }
    }
}