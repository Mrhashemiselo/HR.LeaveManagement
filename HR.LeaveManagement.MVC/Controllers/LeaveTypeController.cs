using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.MVC.Controllers;
public class LeaveTypeController(ILeaveTypeService leaveTypeService) : Controller
{
    // GET: LeaveTypeController
    public async Task<ActionResult> Index()
    {
        var result = await leaveTypeService.GetLeaveTypes();
        return View(result);
    }

    // GET: LeaveTypeController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        var result = await leaveTypeService.GetLeaveTypeDetails(id);
        return View(result);
    }

    // GET: LeaveTypeController/Create
    public async Task<ActionResult> Create()
    {
        return View();
    }

    // POST: LeaveTypeController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateLeaveTypeVM model)
    {
        try
        {
            var response = await leaveTypeService.CreateLeaveType(model);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response.ValidationErrors);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return View(model);
    }

    // GET: LeaveTypeController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var result = await leaveTypeService.GetLeaveTypeDetails(id);
        return View(result);
    }

    // POST: LeaveTypeController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(LeaveTypeVM model)
    {
        try
        {
            var response = await leaveTypeService.UpdateLeaveType(model);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var response = await leaveTypeService.DeleteLeaveType(id);
            if (response.Success)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", response.ValidationErrors);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return BadRequest();
    }
}
