using ClassRoomMVC.Models;
using ClassRoomMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassRoomMVC.Controllers;
public class GroupController : Controller
{
    public readonly GroupsRepository _groupRepository;
    public GroupController(GroupsRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    [Authorize(Policy = "AddPolicy")]
    public async Task<IActionResult> AddNewGroup(Group group)
    {
        if (group.GroupName != null)
        {
            await _groupRepository.AddNewGroup(group);
            return RedirectToAction("InGroup", new { groupName = group.GroupName });
        }
        return View();
    }


    public IActionResult JoinToGroup(string groupName,string key)
    {
        var group = _groupRepository.GetGroupByName(groupName);

        if (group is not null && key == group.Key)
            return RedirectToAction("InGroup", new { groupName = group.GroupName });

        return View();
    }

    [Authorize(Policy = "AddPolicy")]
    public async Task<IActionResult> AddNewTask(TaskDto task)
    {
        var group = _groupRepository.GetGroupByName(task.GroupName!);

        if (group is null)
        {
            return NotFound("404 Error entered group was not found");
        }

        var deadline = new DateTime
            (task.Year, task.Month, task.Day).ToString("d");

        var newTask = new TaskModel
        {
            TaskName = task.TaskName,
            GroupId = group!.Id,
            DeadlineOfTask = deadline,
        };

        await _groupRepository.AddTaskToGroup(newTask);

        return RedirectToAction("InGroup", new { groupName = task.GroupName });
    }

    public IActionResult InGroup(string groupName)
    {
        var group = _groupRepository.GetGroupByName(groupName);

        return View(group);
    }

    public IActionResult DeleteTaskById(int id)
    {
        _groupRepository.DeleteTaskById(id);

        return RedirectToAction("InGroup");
    }

}
