using Utils;

namespace Messages
{
    public class CompanyLevelChanged : GenericTinyMessage<int>
    {
        public CompanyLevelChanged(object sender, int content) : base(sender, content)
        {
        }
    }
}