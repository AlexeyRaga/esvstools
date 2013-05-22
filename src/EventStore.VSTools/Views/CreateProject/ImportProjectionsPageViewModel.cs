using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using EventStore.VSTools.EventStore;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class ImportProjectionsPageViewModel : PageViewModel
    {
        private string _knownConnection;
        private readonly WizardState _state;
        private readonly IHttpClient _httpClient;

        public ObservableCollection<ProjectionInfo> ExistingProjections { get; private set; }

        public ImportProjectionsPageViewModel(WizardState state, IHttpClient httpClient)
        {
            _state = state;
            _httpClient = httpClient;

            ExistingProjections = new ObservableCollection<ProjectionInfo>();
        }

        public override void Activate()
        {
            //already activated, nothing has changed
            if (_knownConnection == _state.EventStoreConnection) return;
            _knownConnection = _state.EventStoreConnection;

            GetProjectionsListFromEventStore();
        }

        private async void GetProjectionsListFromEventStore()
        {
            var eventStoreAddress = EventStoreAddress.Get(_knownConnection);
            var allProjectionsUrl = eventStoreAddress + "/projections/all-non-transient";
            var response = await _httpClient.GetAsync(allProjectionsUrl);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new EventStoreConnectionException(
                    String.Format("Cannot connect to {0} to get the projections list", _knownConnection), response.StatusCode);

            var projections = BuildProjectionInfosFromResponse(response).ToList();

            ExistingProjections.Clear();
            projections.ForEach(ExistingProjections.Add);
        }

        private IEnumerable<ProjectionInfo> BuildProjectionInfosFromResponse(HttpResponse response)
        {
            var jsonContent = response.GetJsonContentAsDynamic();

            foreach (var projInfo in jsonContent.projections)
            {
                if (projInfo != null && !((string)projInfo.effectiveName).StartsWith("$"))
                    yield return new ProjectionInfo
                        {
                            IsSelected = true,
                            Name = projInfo.effectiveName,
                            Mode = projInfo.mode,
                            Location = projInfo.statusUrl,
                        };
            }
        }

        public override string Title
        {
            get { return Resources.Wizard_DownloadProjections; }
        }
    }

    public sealed class ProjectionInfo
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string Mode { get; set; }
        public string Location { get; set; }
    }
}
