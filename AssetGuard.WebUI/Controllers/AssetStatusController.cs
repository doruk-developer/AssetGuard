using AssetGuard.Business.Abstract;
using AssetGuard.Entity;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class AssetStatusController : Controller
    {
        private readonly IAssetStatusService _assetStatusService;

        public AssetStatusController(IAssetStatusService assetStatusService)
        {
            _assetStatusService = assetStatusService;
        }

        public IActionResult Index()
        {
            var values = _assetStatusService.TGetAll();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AssetStatus p)
        {
            _assetStatusService.TAdd(p);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var value = _assetStatusService.TGetById(id);
            _assetStatusService.TDelete(value);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var value = _assetStatusService.TGetById(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult Edit(AssetStatus p)
        {
            _assetStatusService.TUpdate(p);
            return RedirectToAction("Index");
        }
    }
}