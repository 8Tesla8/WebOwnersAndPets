using System.Web;
using System.Web.Mvc;

namespace WebOwnersAndPets
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
