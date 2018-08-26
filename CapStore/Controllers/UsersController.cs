using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CapStore.Models;
using CapStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CapStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        UserManager<Customer> _userManager;

        public UsersController(UserManager<Customer> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer { Email = model.Email, UserName = model.Name, Address = model.Address, PhoneNumber = model.Phone, WorkPhone = model.WorkPhone, HomePhone = model.HomePhone, FirstName = model.FirstName, LastName = model.LastName, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(customer, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            Customer customer = await _userManager.FindByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = customer.Id, Email = customer.Email, Name = customer.UserName, Address = customer.Address, Phone = customer.PhoneNumber, WorkPhone = customer.WorkPhone, HomePhone = customer.HomePhone, FirstName = customer.FirstName, LastName = customer.LastName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await _userManager.FindByIdAsync(model.Id);
                if (customer != null)
                {
                    customer.Email = model.Email;
                    customer.UserName = model.Name;
                    customer.Address = model.Address;
                    customer.PhoneNumber = model.Phone;
                    customer.LastName = model.LastName;
                    customer.FirstName = model.FirstName;
                    customer.HomePhone = model.HomePhone;
                    customer.WorkPhone = model.WorkPhone;


                    var result = await _userManager.UpdateAsync(customer);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            Customer customer = await _userManager.FindByIdAsync(id);
            if (customer != null)
            {
                try
                {
                    IdentityResult result = await _userManager.DeleteAsync(customer);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)                  
                    return RedirectToAction("DelError");
                }
            }
            else
            {
                return NotFound();
            }        
        }

        public IActionResult DelError()
        {
            return View();
        }

        public async Task<ActionResult> Block(string id)
        {
            Customer customer = await _userManager.FindByIdAsync(id);
            if (customer != null)
            {
                await _userManager.SetLockoutEnabledAsync(customer, false);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Unblock(string id)
        {
            Customer customer = await _userManager.FindByIdAsync(id);
            if (customer != null)
            {
                await _userManager.SetLockoutEnabledAsync(customer, true);
            }
            return RedirectToAction("Index");
        }
    }
}
