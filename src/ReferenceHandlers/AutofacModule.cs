using Autofac;
using ReferenceHandlers.Models;
using ReferenceHandlers.Types;

namespace ReferenceHandlers
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HumanInputType>();
            builder.RegisterType<HumanType>();
            builder.RegisterType<DroidType>();
            builder.RegisterType<CharacterInterface>();
            builder.RegisterType<EpisodeEnum>();
            builder.RegisterType<HumanType>();
            builder.RegisterType<HumanType>();
            builder.RegisterType<ReferenceStore>().As<IReferenceStore>();
            builder.RegisterType<StarWarsData>().SingleInstance();

            
        }
    }
}