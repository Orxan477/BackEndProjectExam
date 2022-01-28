using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Areas.admin.ViewComponents
{
    public class SidebarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
