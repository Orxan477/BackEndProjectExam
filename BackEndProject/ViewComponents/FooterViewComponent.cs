using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
