using D2LOffice.Models;
using D2LOffice.Models.D2L;
using DynamicData;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using D2L = D2LOffice.Models.D2L;

namespace D2LOffice.Service
{
    public partial class CourseService : ReactiveObject
    {
        private readonly D2LService _d2lService;
        private SyncService _syncService;

        //private readonly GraphService? _graphService;
        private int _orgId;
        private string _orgStringId = string.Empty;

        //public SourceCache<ContentObject, int> ModuleSource = new SourceCache<ContentObject, int>(x => x.Id);

        // public SourceCache<Quiz, int> QuizSource = new SourceCache<Quiz, int>(x => x.QuizId);
        // public SourceCache<Quiz, int> UpdateQuizSource = new SourceCache<Quiz, int>(x => x.QuizId);

        // public SourceCache<DropboxFolder, int> DropboxFolderSource = new SourceCache<DropboxFolder, int>(x => x.Id);
        // public SourceCache<DropboxFolder, int> UpdateDropboxFolderSource = new SourceCache<DropboxFolder, int>(x => x.Id);


        public Dictionary<int, D2L.DropboxCategory>? Categories { get; private set; }

        public int OrgId => _orgId;
        [Reactive] private bool _canSave;
        [Reactive] private Enrollment? _enrollment;

        List<ContentObject>? _modules;


        public SourceCache<PlanningItem, string> PlanningItemsSource = new SourceCache<PlanningItem, string>(x => x.Id);
        public ObservableCollection<PlanningCategory> PlanningCategories = new ObservableCollection<PlanningCategory>();

        public Subject<Unit> TriggerCategoriesChangedSave = new Subject<Unit>();
        

        public CourseService(int orgId, string orgCode)
        {
            _syncService = new SyncService(orgId, orgCode);

            var d2lService = Locator.Current.GetService<D2LService>();
            if (d2lService == null)
            {
                throw new InvalidOperationException("D2LService is null");
            }
            _d2lService = d2lService;

            //var authService = Locator.Current.GetService<GoogleAuthentication>();

            _orgId = orgId;
            _orgStringId = orgCode;

            PlanningItemsSource.Connect()
                .ToCollection()
                .Subscribe(x=>{
                    SavePlanningItemsAsync(x);
                });

            PlanningCategories.CollectionChanged += (sender, args) => {
                SavePlanningCategoriesAsync(PlanningCategories);
            };

            Observable.FromAsync(async x =>
            {

                var enrollment = await _d2lService.GetEnrollmentDetailsAsync(_orgId);
                if (enrollment == null)
                {
                    throw new Exception("Can't find the enrollment details for this course.");
                }
                Enrollment = enrollment;

                var settings = Locator.Current.GetService<SettingsService>();
                //await authService.GetBaseClientServiceAsync();
                var files = await _d2lService.GetAllCourseFilesAsync(_orgId.ToString());

                _modules = await _d2lService.GetRootModulesAsync(_orgId.ToString());
                //var quizzes = await _d2lService.GetQuizzesAsync(_orgId.ToString());

                //var categories = await _d2lService.GetDropboxCategoriesAsync(_orgId.ToString());
                //Categories = categories.ToDictionary(x => x.Id);




                //await UpdateDropboxFolders();

            }).Subscribe();

            
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{_orgId.ToString()}_planningItems.json");
            try{
                var json = System.IO.File.ReadAllText(path);
                var items = JsonConvert.DeserializeObject<List<PlanningItem>>(json);
                PlanningItemsSource.AddOrUpdate(items);
            }
            catch {
                Debug.WriteLine("No planning items found");
            }
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{_orgId.ToString()}_planningCategories.json");
            try{
                var json = System.IO.File.ReadAllText(path);
                var categories = JsonConvert.DeserializeObject<List<PlanningCategory>>(json);
                foreach (var cat in categories)
                    PlanningCategories.Add(cat);
            }
            catch {
                Debug.WriteLine("No planning categories found");
            }
            
            // var test = new PlanningItem
            // {
            //     Date = DateTimeOffset.Now,
            //     Category = "Test",
            //     Title = "Test",
            //     Description = "Test",
            //     Type = PlanningItemType.Reminder
            // };
            // PlanningItemsSource.AddOrUpdate(test);
            // var test2 = new PlanningItem
            // {
            //     Date = DateTimeOffset.Now,
            //     Category = "Test",
            //     Title = "Test",
            //     Description = "Test",
            //     Type = PlanningItemType.Reminder
            // };
            // PlanningItemsSource.AddOrUpdate(test2);
            // var test3 = new PlanningItem
            // {
            //     Date = DateTimeOffset.Now,
            //     Category = "Test",
            //     Title = "Test",
            //     Description = "Test",
            //     Type = PlanningItemType.Reminder
            // };
            // PlanningItemsSource.AddOrUpdate(test3);
            // var test4 = new PlanningItem
            // {
            //     Date = DateTimeOffset.Now,
            //     Category = "Test",
            //     Title = "Test",
            //     Description = "Test",
            //     Type = PlanningItemType.Reminder
            // };
            // PlanningItemsSource.AddOrUpdate(test4);
        }

        public async Task TriggerCategorySaveAsync()
        {
            await SavePlanningCategoriesAsync(PlanningCategories);
        }

        public Task SyncAsync(IList<PlanningItem> planningItems, IList<PlanningCategory> planningCategories, Action<SyncProgress> showProgress)
        {
            return _syncService.SyncToD2LAsync(planningItems, planningCategories, showProgress);
        }

        public Task AddPlanningItemAsync()
        {
            PlanningItemsSource.AddOrUpdate(new PlanningItem(){Category = PlanningCategories[0].Name});
            return Task.CompletedTask;
        }

        private Task SavePlanningCategoriesAsync(IList<PlanningCategory> categories)
        {
            var json = JsonConvert.SerializeObject(categories);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{_orgId.ToString()}_planningCategories.json");
            System.IO.File.WriteAllText(path, json);
            return Task.CompletedTask;
        }

        private Task SavePlanningItemsAsync(IReadOnlyCollection<PlanningItem> items)
        {
            var json = JsonConvert.SerializeObject(items);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{_orgId.ToString()}_planningItems.json");
            System.IO.File.WriteAllText(path, json);
            return Task.CompletedTask;
        }
        

        //public async Task PushChangesToD2LAsync()
        //{
        //    try
        //    {
        //        var data = await _sheetsService.GetDataToPushToD2LAsync(_spreadSheet);
        //        var moduleRoot = _modules.Where(x => x.Type == ContentObjectType.Module).Cast<Module>().FirstOrDefault(x => x.Title == data.RootModule);
        //        if (moduleRoot == null)
        //        {
        //            moduleRoot = await _d2lService.CreateModuleInRootAsync(_orgId, data.RootModule);
        //            if (moduleRoot == null)
        //            {
        //                throw new Exception("Can't find or can't create the module your content will be stored in.");
        //            }
        //            _modules.Add(moduleRoot);
        //        }
        //        switch (data.WeeklyInfoType)
        //        {
        //            case Models.WeeklyInfoType.Modules:

        //                break;
        //            case Models.WeeklyInfoType.Checklists:
        //            default:

        //                break;
        //        }

        //    } 
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async Task PushChangesToD2LforChecklists(PushData data, Module moduleRoot)
        //{
        //    var checklists = data;
        //    var checklistsInModule = _modules.Where(x => x.Type == ContentObjectType.Topic).Cast<Topic>().Where(x => x.ParentModuleId == moduleRoot.Id).ToList();
        //    foreach (var checklist in checklists)
        //    {
        //        var topic = checklistsInModule.FirstOrDefault(x => x.Title == checklist.Title);
        //        if (topic == null)
        //        {
        //            topic = await _d2lService.CreateTopicAsync(_orgId, moduleRoot.Id, checklist.Title);
        //            if (topic == null)
        //            {
        //                throw new Exception("Can't find or can't create the topic your content will be stored in.");
        //            }
        //            _modules.Add(topic);
        //        }
        //        await _d2lService.UpdateTopicAsync(_orgId, topic.Id, checklist);
        //    }
        //}

        // public async Task UpdateDropboxFolders()
        // {
        //     var dropboxFolders = await _d2lService.GetDropboxFoldersAsync(_orgId.ToString());
        //     DropboxFolderSource.EditDiff(dropboxFolders, (x, y) => x.UnreadFiles == y.UnreadFiles);
        //     //DropboxFolderSource.Clear();
        //     //DropboxFolderSource.AddOrUpdate(dropboxFolders);
        // }

        // public async Task SaveUpdatedQuizzesAsync()
        // {
        //     var quizzes = UpdateQuizSource.Items.ToList();
        //     foreach (var quiz in quizzes)
        //     {
        //         var result = await _d2lService.UpdateQuizAsync(_orgId.ToString(), new D2L.QuizUpdate(quiz));
        //         if (result)
        //         {
        //             UpdateQuizSource.Remove(quiz);
        //             QuizSource.AddOrUpdate(quiz);
        //         }
        //     }
        // }
    }
}
