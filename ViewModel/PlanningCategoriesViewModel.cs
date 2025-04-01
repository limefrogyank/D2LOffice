using D2LOffice.Models;
using D2LOffice.Models.D2L;
using D2LOffice.Service;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.ViewModel
{
    public class PlanningCategoriesViewModel : ReactiveObject, IDisposable
    {
        private readonly D2LService _d2lService;
        private readonly CourseService _courseService;
       
        public ReactiveCommand<Unit, Unit>? GoBackCommand { get; }
        public ReactiveCommand<PlanningCategory, Unit>? DeleteCommand { get; }

        public ObservableCollection<PlanningCategory> PlanningCategories { get; private set; }

        
        public PlanningCategoriesViewModel(NavigationManager navigationManager, string orgUnitId)
        {
            var d2LService = Locator.Current.GetService<D2LService>();
            if (d2LService == null)
            {
                throw new InvalidOperationException("D2LService is null");
            }
            _d2lService = d2LService;

            var courseService = Locator.Current.GetService<CourseService>(orgUnitId);
            if (courseService == null)
            {
                throw new InvalidOperationException("CourseService is null");
            }
            _courseService = courseService;

           
            
            PlanningCategories = _courseService.PlanningCategories;

           
            GoBackCommand = ReactiveCommand.Create<Unit, Unit>(_ =>
            {
                //navigationManager..Router.NavigateBack.Execute().Subscribe();
                return Unit.Default;
            });

            DeleteCommand = ReactiveCommand.Create<PlanningCategory, Unit>(category =>
            {
                PlanningCategories.Remove(category);
                return Unit.Default;
            });

        }

        public async void UpdatePlanningCategory()
        {            
            await _courseService.TriggerCategorySaveAsync();
        }

        public void AddPlanningCategory()
        {
            PlanningCategories.Add(new PlanningCategory(){Name = "New Category", Description = "", SortOrder = PlanningCategories.Count});
        }


    

        public void Navigate(string key)
        {
            //switch (key)
            //{
            //    case "Quiz Detail":
            //        Router.Navigate.Execute(new QuizDetailsViewModel(this, _courseService)).Subscribe();
            //        break;
            //    case "Assignments":
            //        Router.Navigate.Execute(new DropboxListViewModel(this, _courseService)).Subscribe();
            //        break;
            //    case "Calendar":
            //        Router.Navigate.Execute(new CalendarViewModel(this, _courseService)).Subscribe();
            //        break;
            //    case "GradeDropbox":
            //        Router.Navigate.Execute(new GradeDropboxesViewModel(this, _courseService)).Subscribe();
            //        break;
            //}
        }

        public void GoBack()
        {
            // if (Router.NavigationStack.Count > 1)
            // {
            //     Router.NavigateBack.Execute().Subscribe();
            // }
            // else
            // {
            //     HostScreen.Router.NavigateBack.Execute().Subscribe();
            // }
        }

        public void Dispose()
        {
            //GradeDropboxesViewModel.Dispose();
        }
    }
}
