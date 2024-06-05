
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate_Dapper_UI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int UserId
		{
			get
			{
				if (!User.Identity.IsAuthenticated)
				{
					return -1;
				}
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				return int.Parse(userId);
			}
		}
    }
}
