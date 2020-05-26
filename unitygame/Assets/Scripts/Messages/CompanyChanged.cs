using Gameplay;
using Utils;

namespace Messages
{
    public class CompanyChanged : GenericTinyMessage<Company>
    {
        public CompanyChanged(object sender, Company content) : base(sender, content)
        {
        }
    }
}