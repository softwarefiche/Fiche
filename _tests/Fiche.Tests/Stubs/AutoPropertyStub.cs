using System;

namespace Fiche.Tests.Stubs
{
    public class AutoPropertyStub
    {
        public int AutoProperty { get; set; }
        private int _bodyProperty;
        public int BodyProperty
        {
            get => this._bodyProperty; set
            {
                this._bodyProperty = value;
                BodyPropertyChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler BodyPropertyChanged;
    }
}
