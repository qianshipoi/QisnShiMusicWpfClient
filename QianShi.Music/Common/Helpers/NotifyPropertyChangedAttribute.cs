using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace QianShi.Music.Common.Helpers
{
    public class NotifyPropertyChangedAttribute : TypeAspect
    {
        public override void BuildAspect(IAspectBuilder<INamedType> builder)
        {
            builder.Advice.ImplementInterface(builder.Target, typeof(INotifyPropertyChanged), whenExists: OverrideStrategy.Ignore);

            var props = builder.Target.Properties.Where(p => !p.IsAbstract && p.Writeability == Writeability.All);
            foreach (var prop in props)
            {
                builder.Advice.OverrideAccessors(prop, null, nameof(this.OverridePropertySetter));
            }
        }

        [InterfaceMember]
        private event PropertyChangedEventHandler? PropertyChanged;

        [Introduce(WhenExists = OverrideStrategy.Ignore)]
        protected void OnRropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(meta.This, new PropertyChangedEventArgs(name));
        }

        [Template]
        private decimal OverridePropertySetter(dynamic value)
        {
            if (value != meta.Target.Property.Value)
            {
                meta.Proceed();
                this.OnRropertyChanged(meta.Target.Property.Name);
            }
            return value;
        }
    }
}
