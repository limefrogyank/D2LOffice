using D2LOffice.Models;
using D2LOffice.Models.D2L;
using D2LOffice.Service;
using DynamicData;
using DynamicData.Binding;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D2LOffice.ViewModel
{
    public partial class EnrollmentsViewModel : ReactiveObject
    {
        public string UrlPathSegment => "FileViewModel";

        private readonly Navigation _navigation;
        private readonly D2LService? d2lService;
        private CancellationTokenSource _cancellationTokenSource;

        //private readonly IFileService fileService;



        [Reactive] private Enrollment? _selectedEnrollment;
        [Reactive] private string? _selectedEnrollmentOrgId;
        //[Reactive] public FullCourse FullCourse { get; set; }

        public ReadOnlyObservableCollection<Enrollment> Enrollments { get; private set; }
        SourceCache<Enrollment, int> enrollmentsSource = new SourceCache<Enrollment, int>(x => x.OrgUnit.Id);


        public EnrollmentsViewModel(Navigation navigation)
        {
            _navigation = navigation;
            d2lService = Locator.Current.GetService<D2LService>();
            if (d2lService == null)
            {
                throw new Exception("D2L Service not found");
            }
            //fileService = Locator.Current.GetService<IFileService>();
            _cancellationTokenSource = new CancellationTokenSource();

            enrollmentsSource.Connect()
                .Filter(x => x.PinDate.HasValue)
                .Filter(x=>x.Access != null)
                .ObserveOn(RxApp.MainThreadScheduler)
                .SortAndBind(out var enrollments, SortExpressionComparer<Enrollment>.Descending(x => x.Access.StartDate != null ? x.Access.StartDate : DateTime.Now))
                .Subscribe();
            Enrollments = enrollments;
            //Enrollments.CollectionChanged += Enrollments_CollectionChanged;

            
            d2lService.HasAccess.Where(x => x == true).Take(1).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async x =>
            {
                var result = await d2lService.GetEnrollmentsAsync();
                enrollmentsSource.AddOrUpdate(result);
            });

            this.WhenAnyValue(x => x.SelectedEnrollment).WhereNotNull().Subscribe(x =>
            {
                SelectedEnrollment = null;
                _navigation.NavigateTo($"course/{x.OrgUnit.Id}/{x.OrgUnit.Name}");
                //HostScreen.Router.Navigate.Execute(new CourseViewModel(HostScreen, x)).Subscribe();
            });

        }


    }
}
