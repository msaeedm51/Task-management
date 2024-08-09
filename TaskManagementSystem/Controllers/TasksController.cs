using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.DTOs.RequestDTO;
using TaskManagementSystem.HelperExtensionMethods;
using TaskManagementSystem.MappingExtensionMethods;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IDocumentService _documentService;
        public TasksController(ITaskService taskService, 
                               IDocumentService documentService)
        {
            _taskService = taskService;
            _documentService = documentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAll();
                return Ok(tasks.Select(t => t.MapToTaskDTO()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting tasks from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            try
            {
                var task = await _taskService.GetById(taskId);
                if (task == null)
                {
                    return NotFound("No Task found");
                }
                return Ok(task.MapToTaskDTO());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a task with id:{0} from the database : {1} : {2}", taskId, ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpGet("{userId}/user")]
        public async Task<IActionResult> GetTaskByUserId(int userId)
        {
            try
            {
                var tasks = await _taskService.GetByUser(userId);
                if (tasks == null)
                {
                    return NotFound("No Task found");
                }
                return Ok(tasks.Select(t=> t.MapToTaskDTO()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a task with user id:{0} from the database : {1} : {2}", userId, ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] RequestCreateTasksDTO taskToCreate)
        {
            try
            {
                if (taskToCreate is null)
                {
                    return BadRequest();
                }

                var taskDB = await _taskService.AddTask(taskToCreate);
                if (taskDB is null) return BadRequest();

                if(taskToCreate.DocumentFiles is not null)
                {
                    await taskToCreate.DocumentFiles.ForEachAsync(file => _documentService.AddDocumentAsync(taskDB.Id, file));
                }

                taskDB = await _taskService.GetById(taskDB.Id);

                return CreatedAtAction(nameof(GetTaskById), new { id = taskDB!.Id }, taskDB.MapToTaskDTO());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while creating task from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] RequestUpdateTaskDTO taskToUpdate)
        {
            try
            {
                if (taskId != taskToUpdate.Id)
                {
                    return BadRequest();
                }

                var updatedTask = await _taskService.UpdateTask(taskToUpdate);
                if (updatedTask == null) return NotFound("No such task found");

                if (taskToUpdate.DocumentFiles is not null)
                {
                    await taskToUpdate.DocumentFiles.ForEachAsync(file => _documentService.AddDocumentAsync(taskId, file));
                }


                return NoContent();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Updating task from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                var success = await _taskService.DeleteTask(taskId);
                if (success == false) return NotFound("No Such task found");

                return NoContent();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Deleting Task from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }
    }
}
