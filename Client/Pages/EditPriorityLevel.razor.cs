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
    public partial class EditPriorityLevel
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

        [Parameter]
        public int PriorityID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            priorityLevel = await ConDataService.GetPriorityLevelByPriorityId(priorityId:PriorityID);
        }
        protected bool errorVisible;
        protected ToDoList.Server.Models.ConData.PriorityLevel priorityLevel;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await ConDataService.UpdatePriorityLevel(priorityId:PriorityID, priorityLevel);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(priorityLevel);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            priorityLevel = await ConDataService.GetPriorityLevelByPriorityId(priorityId:PriorityID);
        }
    }
}