﻿using AutoMapper;
using Shoppi.Logic.Abstract;
using Shoppi.Web.Models.CartViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class CartController : Controller
    {
        private ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        public ActionResult Index()
        {
            var cart = _cartServices.GetCart();
            var model = Mapper.Map<CartIndexViewModel>(cart);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(CartAddViewModel model)
        {
            await _cartServices.AddAsync(model.ProductId);
            return Redirect(model.ReturnUrl);
        }

        [HttpPost]
        public ActionResult DecrementQuantity(int id)
        {
            var newQuantity = _cartServices.DecrementProductQuantity(id);
            return Json(new { quantity = newQuantity });
        }
    }
}