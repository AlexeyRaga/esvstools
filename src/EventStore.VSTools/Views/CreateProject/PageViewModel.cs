namespace EventStore.VSTools.Views.CreateProject
{
    public abstract class PageViewModel
    {
        public abstract string Title { get; }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual void Activate() {}
        public virtual void Deactivate() {}
    }
}
