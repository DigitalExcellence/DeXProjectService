using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DexProjectService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        ProjectService projectService;
        EventService eventService;
        public ProjectController(ProjectService projectService, EventService eventService)
        {
            this.projectService = projectService;
            this.eventService = eventService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            Project project = await projectService.GetProjectById(id);

            if (project == null)
            {
                return NotFound("Could not find project");
            }

            return Ok(project);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            List<Project> projects = await projectService.GetAllProjects();

            if (projects == null)
            {
                return NotFound("Could not find projects");
            }

            return Ok(projects);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewProject(Project project)
        {
            try
            {
                await projectService.AddProject(project);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Project added");
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveProject(int id)
        {
            try
            {
                await projectService.RemoveProject(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Project Removed");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProject(Project projectToUpdate)
        {
            Project project = await projectService.GetProjectById(projectToUpdate.Id);

            if (project == null)
            {
                return NotFound("Could not find project");
            }

            try
            {
                project.Name = projectToUpdate.Name;
                project.Description = projectToUpdate.Description;

                await projectService.UpdateProject(project);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Project Updated");
        }

        [HttpPost("event")]
        public async Task<IActionResult> Event()
        {
            HttpRequest req = HttpContext.Request;
            string response = string.Empty;
            BinaryData events = await BinaryData.FromStreamAsync(req.Body);

            EventGridEvent[] eventGridEvents = EventGridEvent.ParseMany(events);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                // Handle system events
                if (eventGridEvent.TryGetSystemEventData(out object eventData))
                {
                    // Handle the subscription validation event
                    if (eventData is SubscriptionValidationEventData subscriptionValidationEventData)
                    {
                        // Do any additional validation (as required) and then return back the below response

                        var responseData = new SubscriptionValidationResponse()
                        {
                            ValidationResponse = subscriptionValidationEventData.ValidationCode
                        };
                        return new OkObjectResult(responseData);
                    }
                }
                // Handle the projectUpdated event
                else if (eventGridEvent.EventType == "projectUpdated")
                {
                    //var contosoEventData = eventGridEvent.Data.ToObjectFromJson<ContosoItemReceivedEventData>();
                    Project project = new Project();
                    project.Name = "event fetched :D";
                    project.Description = "event fetched :D";

                    await projectService.AddProject(project);
                }

                // Handle the projectRemoved event
                else if (eventGridEvent.EventType == "projectRemoved")
                {
                    //var contosoEventData = eventGridEvent.Data.ToObjectFromJson<ContosoItemReceivedEventData>();
                    Project project = new Project();
                    project.Name = "Project removed event";
                    project.Description = "Project removed event";

                    await projectService.AddProject(project);
                }
            }

            return new OkObjectResult("Could not fetch event");
        }
    }
}
    
