using E_commerceDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceDashboard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if(ModelState.IsValid)
            {
                var roleExist = await _roleManager.RoleExistsAsync(model.Name);
                if(roleExist)
                {
                    ModelState.AddModelError(string.Empty, "The Role Is Already Exist");
                    return RedirectToAction(nameof(Index), model);
                }
                await _roleManager.CreateAsync(new IdentityRole() { Name = model.Name.Trim() });
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction(nameof(Index) , model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if(id is not null)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return NotFound();
                return View(new RoleViewModel() { Id = role.Id , Name = role.Name});
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExist = await _roleManager.RoleExistsAsync(model.Name);
                if (roleExist)
                {
                    ModelState.AddModelError(string.Empty, "The Role Is Already Exist");
                    return View(model);
                }
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role is null) return NotFound();
                role.Name = model.Name;
                await _roleManager.UpdateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
