namespace EventStore.VSTools.EventStore
{
    public sealed class ProjectionConfig
    {
        public string Name { get; private set; }
        public bool IsEmitEnabled { get; private set; }
        public string Query { get; private set; }

        public ProjectionConfig(dynamic jsonData)
        {
            Name = jsonData.name;
            Query = jsonData.query;
            IsEmitEnabled = jsonData.emitEnabled;
        }
    }
}
