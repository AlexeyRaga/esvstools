using System;

namespace EventStore.VSTools.Views.CreateProject
{
    public abstract class PageViewModel
    {
        public event EventHandler PageStateChanded;

        public abstract string Title { get; }

        public virtual bool IsValid() { return true; }

        public virtual void Activate() {}
        public virtual void Deactivate() {}

        protected void RaisePageStateChanged()
        {
            var evt = PageStateChanded;
            if (evt != null) evt(this, EventArgs.Empty);
        }

        private bool _canGoBack = true;
        public virtual bool CanGoBack
        {
            get { return _canGoBack; }
            protected set
            {
                if (_canGoBack == value) return;
                _canGoBack = value;
                RaisePageStateChanged();
            }
        }

        private bool _canGoNext = true;
        public virtual bool CanGoNext
        {
            get { return _canGoNext; }
            protected set
            {
                if (_canGoNext == value) return;
                _canGoNext = value;
                RaisePageStateChanged();
            }
        }
    }
}
