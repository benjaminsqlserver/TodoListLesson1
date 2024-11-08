using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ToDoList.Client.Pages
{
    public partial class AddToDoList
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public ConDataService ConDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            toDoList = new ToDoList.Server.Models.ConData.ToDoList();
        }
        protected bool errorVisible;
        protected ToDoList.Server.Models.ConData.ToDoList toDoList;

        protected IEnumerable<ToDoList.Server.Models.ConData.PriorityLevel> priorityLevelsForPriorityID;

        protected IEnumerable<ToDoList.Server.Models.ConData.Status> statusesForStatusID;


        protected int priorityLevelsForPriorityIDCount;
        protected ToDoList.Server.Models.ConData.PriorityLevel priorityLevelsForPriorityIDValue;
        protected async Task priorityLevelsForPriorityIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetPriorityLevels(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(PriorityName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                priorityLevelsForPriorityID = result.Value.AsODataEnumerable();
                priorityLevelsForPriorityIDCount = result.Count;

                if (!object.Equals(toDoList.PriorityID, null))
                {
                    var valueResult = await ConDataService.GetPriorityLevels(filter: $"PriorityID eq {toDoList.PriorityID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        priorityLevelsForPriorityIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load PriorityLevel" });
            }
        }

        protected int statusesForStatusIDCount;
        protected ToDoList.Server.Models.ConData.Status statusesForStatusIDValue;
        protected async Task statusesForStatusIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetStatuses(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(StatusName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                statusesForStatusID = result.Value.AsODataEnumerable();
                statusesForStatusIDCount = result.Count;

                if (!object.Equals(toDoList.StatusID, null))
                {
                    var valueResult = await ConDataService.GetStatuses(filter: $"StatusID eq {toDoList.StatusID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        statusesForStatusIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Status" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                toDoList.CreatedAt=DateTime.Now;
                toDoList.UpdatedAt=DateTime.Now;
                var result = await ConDataService.CreateToDoList(toDoList);
                DialogService.Close(toDoList);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;
    }
}