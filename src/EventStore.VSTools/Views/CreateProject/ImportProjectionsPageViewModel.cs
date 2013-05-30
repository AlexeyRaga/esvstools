using System;
using System.Collections.ObjectModel;
using System.Linq;
using EventStore.VSTools.EventStore;

namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class ImportProjectionsPageViewModel : PageViewModel
    {
        private string _knownConnection;
        private readonly WizardState _state;
        private readonly Func<string, IProjectionsManager> _projectionsManagerFactory;

        public ObservableCollection<ImportProjectionInfo> ExistingProjections { get; private set; }

        public ImportProjectionsPageViewModel(WizardState state, Func<string, IProjectionsManager> projectionsManagerFactory)
        {
            _state = state;
            _projectionsManagerFactory = projectionsManagerFactory;

            ExistingProjections = new ObservableCollection<ImportProjectionInfo>();
        }

        public override void Activate()
        {
            base.Activate();

            //already activated, nothing has changed
            if (_knownConnection == _state.EventStoreConnection) return;
            _knownConnection = _state.EventStoreConnection;

            GetProjectionsListFromEventStoreAsync();
        }

        public override void Deactivate()
        {
            _state.ProjectionsToImport.Clear();
            if (ShouldImportProjections)
                _state.ProjectionsToImport.AddRange(ExistingProjections.Where(x=>x.IsSelected).Select(x=>x.Projection));

            base.Deactivate();
        }

        private async void GetProjectionsListFromEventStoreAsync()
        {
            var manager = _projectionsManagerFactory(_state.EventStoreConnection);
            var response = await manager.GetAllNonTransientAsync();

            if (!response.IsSuccessful)
                throw new EventStoreConnectionException(
                    String.Format("Cannot connect to {0} to get the projections list", _knownConnection), response.Status);

            var projections = response.Result
                                      .Where(x => !String.IsNullOrWhiteSpace(x.Name))
                                      .Where(x => !x.Name.StartsWith("$"))
                                      .Select(x => new ImportProjectionInfo(true, x))
                                      .ToList();

            ExistingProjections.Clear();
            projections.ForEach(ExistingProjections.Add);
        }

        public bool ShouldImportProjections { get; set; }

        public override string Title
        {
            get { return Resources.Wizard_DownloadProjections; }
        }

        public sealed class ImportProjectionInfo
        {
            public ProjectionStatistics Projection { get; private set; }
            public bool IsSelected { get; set; }

            public ImportProjectionInfo(bool isSelected, ProjectionStatistics projection)
            {
                IsSelected = isSelected;
                Projection = projection;
            }
        }
    }
}
