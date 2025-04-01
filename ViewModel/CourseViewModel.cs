using CommunityToolkit.Maui.Storage;
using CsvHelper;
using D2LOffice.Models;
using D2LOffice.Models.D2L;
using D2LOffice.Service;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.ViewModel
{
    public partial class CourseViewModel : ReactiveObject, IDisposable
    {
        public string UrlPathSegment => "CourseViewModel";

        public Enrollment Enrollment => _enrollment.Value;
        private readonly ObservableAsPropertyHelper<Enrollment> _enrollment;
        private readonly D2LService _d2lService;
        private readonly CourseService _courseService;
        private readonly Navigation _navigation;

        //private readonly MSALAuthenticationService? _authService;
        //private readonly GraphService? _graphService;


        //public GradeDropboxesViewModel GradeDropboxesViewModel { get; private set; }

        [Reactive] private bool _isEditing = false;
        [Reactive] public bool _isSyncing = false;
        [Reactive] public int? _percentageSynced = null;
        [Reactive] public bool _syncTotalKnown = false;
        [Reactive] public string _syncStatus = "";


        public ReactiveCommand<Unit, Unit>? GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit>? PushChangesCommand { get; }
        public ReactiveCommand<Unit, Unit>? LaunchExcelWeb { get; }
        

        public ReactiveCommand<Unit, Unit>? SyncCommand { get; }
        public ReactiveCommand<Unit, Unit>? EditCategoriesCommand { get; }
        public ReactiveCommand<Unit, Unit>? AddPlanningItemCommand { get; }

        public ReactiveCommand<Unit, Unit>? ExportToCsvCommand { get; }
        public ReactiveCommand<Unit, Unit>? ImportFromCsvCommand { get; }


        public ReactiveCommand<List<string>, Unit> SetCategoryFilterCommand { get; }
        public ReactiveCommand<List<string>, Unit> SetTypeFilterCommand { get; }

        // readonly ObservableAsPropertyHelper<string> loginText;
        // public string LoginText => loginText.Value;

        public ReadOnlyObservableCollection<PlanningItem> UnfilteredPlanningItems { get; private set; }
        public ReadOnlyObservableCollection<PlanningItem> PlanningItems { get; private set; }
        public ObservableCollection<PlanningCategory> PlanningCategories { get; private set; }

        public IObservable<DateTimeOffset> MinimumObs { get; private set;}

        private BehaviorSubject<IComparer<PlanningItem>> _comparer = new BehaviorSubject<IComparer<PlanningItem>>(SortExpressionComparer<PlanningItem>.Ascending(x => x.Date));
        
        public CourseViewModel(Navigation navigation, string orgUnitId, string orgStringId)
        {
            _navigation = navigation;
            var d2LService = Locator.Current.GetService<D2LService>();
            if (d2LService == null)
            {
                throw new InvalidOperationException("D2LService is null");
            }
            _d2lService = d2LService;

            //var courseService = Locator.Current.GetService<CourseService>(orgUnitId);
            // if (courseService == null)
            // {
            //     throw new InvalidOperationException("CourseService is null");
            // }
            // _courseService = courseService;
      
            _courseService = new CourseService(int.Parse(orgUnitId), orgStringId);
            Locator.CurrentMutable.Register<CourseService>(()=>_courseService, orgUnitId );

            BehaviorSubject<Func<PlanningItem, bool>> categoryFilter = new BehaviorSubject<Func<PlanningItem, bool>>((c) => true);
            BehaviorSubject<Func<PlanningItem, bool>> typeFilter = new BehaviorSubject<Func<PlanningItem, bool>>((c) => true);

            MinimumObs = _courseService.PlanningItemsSource.Connect().ToCollection().Select(x=> x.Count > 0 ? x.Min(y=>y.Date) : DateTimeOffset.Now);

            _courseService.PlanningItemsSource.Connect()
               .ObserveOn(RxApp.MainThreadScheduler)
               .Bind(out var unfilteredPlanningItems)
               .Subscribe();
            UnfilteredPlanningItems = unfilteredPlanningItems;

            _courseService.PlanningItemsSource.Connect()
                .Filter(categoryFilter)
                .Filter(typeFilter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .SortAndBind(out var planningItems, _comparer)
                .Subscribe();
            PlanningItems = planningItems;

            PlanningCategories = _courseService.PlanningCategories;

            this.WhenAnyValue(x => x._courseService.Enrollment)
                .Where(x => x != null)
                .Select(x => x!)
                .ToProperty(this, x => x.Enrollment, out _enrollment);
           

            GoBackCommand = ReactiveCommand.Create<Unit, Unit>(_ =>
            {
                //navigationManager..Router.NavigateBack.Execute().Subscribe();
                return Unit.Default;
            });

            SyncCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            {
                IsSyncing = true;
                await _courseService.SyncAsync(PlanningItems, PlanningCategories, (x) => UpdateProgress(x));
                IsSyncing = false;
                SyncTotalKnown = false;
                SyncStatus = "";
                PercentageSynced = 0;
                return Unit.Default;
            });

            // EditCategoriesCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            // {
            //    //await _courseService.SyncAsync(PlanningItems, PlanningCategories);
            //    return Unit.Default;
            // });

            AddPlanningItemCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            {
               await _courseService.AddPlanningItemAsync();
               return Unit.Default;
            });

            ExportToCsvCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            {
                //var folder = await FolderPicker.Default.PickAsync();
                //if (folder.IsSuccessful == false)
                //{
                //    return Unit.Default;
                //}
                var stream = new MemoryStream();
                
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    await csv.WriteRecordsAsync(PlanningItems);
                    csv.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    await FileSaver.Default.SaveAsync($"PlanningItems_{orgUnitId}.csv", stream);

                }
                

                stream.Dispose();

                return Unit.Default;
            });

            ImportFromCsvCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            {
                try
                {
                    var stream = new MemoryStream();
                    var fileResult = await FilePicker.Default.PickAsync();
                    if (fileResult == null)
                    {
                        return Unit.Default;
                    }

                    using (var reader = new StreamReader(fileResult.FullPath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<PlanningItem>().ToList();
                        foreach (var record in records)
                        {
                            if (string.IsNullOrEmpty(record.Id))
                            {
                                record.Id = Guid.NewGuid().ToString();
                            }
                        }
                        _courseService.PlanningItemsSource.Clear();
                        _courseService.PlanningItemsSource.AddOrUpdate(records);

                        // check for categories that were written in but not created with the tool (i.e. imported from csv)
                        var categoryNames = records.Select(x => x.Category).Distinct().Where(x => !string.IsNullOrEmpty(x));
                        var missingCategories = categoryNames.Except(_courseService.PlanningCategories.Select(x => x.Name)).ToList();
                        foreach (var miss in missingCategories)
                        {
                            var newCat = new PlanningCategory { Name = miss, Description = "", SortOrder = _courseService.PlanningCategories.Count };
                            _courseService.PlanningCategories.Add(newCat);
                        }
                    }


                    stream.Dispose();

                    return Unit.Default;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return Unit.Default;
                }
            });

            SetCategoryFilterCommand = ReactiveCommand.Create<List<string>, Unit>(x =>
            {
                if (x.Count == 0) //MUST SELECT ONE FILTER
                {
                    categoryFilter.OnNext((c) => true);
                }
                else
                {
                    categoryFilter.OnNext((c) => c.Category!=null && x.Contains(c.Category));
                }
                return Unit.Default;
            });

            SetTypeFilterCommand = ReactiveCommand.Create<List<string>, Unit>(x =>
            {
                if (x.Count == 0) //MUST SELECT ONE FILTER
                {
                    typeFilter.OnNext((c) => true);
                }
                else
                {
                    typeFilter.OnNext((c) => c.Type != null && x.Contains(c.Type));
                }
                return Unit.Default;
            });

        }

        private void UpdateProgress(SyncProgress syncProgress)
        {
            if (syncProgress.Total == 0)
            {
                SyncTotalKnown = false;
            }
            else
            {
                SyncTotalKnown = true;
                PercentageSynced = (int)Math.Round((double)syncProgress.Completed / (double)syncProgress.Total * 100.0);
            }
            SyncStatus = syncProgress.Message;
            Debug.WriteLine(syncProgress.Message);
        }

        public void UpdatePlanningItem(PlanningItem item)
        {
            _courseService.PlanningItemsSource.AddOrUpdate(item);
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

        public void DeletePlanningItem(PlanningItem item)
        {
            _courseService.PlanningItemsSource.Remove(item);
        }

        public void Dispose()
        {
            //GradeDropboxesViewModel.Dispose();
        }
    }
}
