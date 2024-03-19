using CineTicketz.Models.DTOs;
using CineTicketz.Services;
using CineTicketz.Services.MicroServices;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CineTicketz.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ShoppingCart _shoppingCart;

        public OrdersController(IUnitOfWork unitOfWork, ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _unitOfWork.Orders.GetOrdersByUserIdAndRole(userId, userRole);

            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserData(string id)
        {
            string userId = id;

            var orders = await _unitOfWork.Orders.GetOrdersByUserId(userId);

            return View(orders);
        }

        public IActionResult ShopingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var viewModel = new ShoppingCartViewModel()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartToatal()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id) 
        { 
            var movie = await _unitOfWork.Movies.GetById(id);

            if (movie is null)
                return BadRequest();

            _shoppingCart.AddItemToCart(movie);

            return RedirectToAction(nameof(ShopingCart));
        }

        public async Task<IActionResult> AddItemToShoppingCartFromMoviesIndex(int id)
        {
            var movie = await _unitOfWork.Movies.GetById(id);

            if (movie is null)
                return BadRequest();

            _shoppingCart.AddItemToCart(movie);

            return RedirectToAction("Index", "Movies");
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var movie = await _unitOfWork.Movies.GetById(id);

            if (movie is null)
                return BadRequest();

            _shoppingCart.RemoveItemFromCart(movie);

            return RedirectToAction(nameof(ShopingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var storeOrderDto = new StoreOrderDto()
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                UserEmailAddress = User.FindFirstValue(ClaimTypes.Email),
                ShoppingCartItems = _shoppingCart.GetShoppingCartItems()
            };

            await _unitOfWork.Orders.StoreOrder(storeOrderDto);
            await _shoppingCart.ClearShoppingCart();

            return View();
        }
    }
}


