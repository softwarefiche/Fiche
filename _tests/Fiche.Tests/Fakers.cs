using Bogus;

using Fiche.Tests.Stubs;

namespace Fiche.Tests
{
    public static class Fakers
    {
        static Fakers()
        {
            Faker = new Faker();
            SimpleObjectFaker = new Faker<SimpleObject>()
                .RuleFor(o => o.IntProperty, f => f.Random.Int(min: 1, max: 10))
                .RuleFor(o => o.StringProperty, (f, o) => f.Random.String2(o.IntProperty));
            ComplexObjectFaker = new Faker<ComplexObject>()
                .RuleFor(o => o.LongProperty, f => f.Random.Long(min: 1, max: 10))
                .RuleFor(o => o.SimpleProperty, () => SimpleObjectFaker.Generate());
        }
        public static Faker Faker { get; }
        public static Faker<SimpleObject> SimpleObjectFaker { get; }
        public static Faker<ComplexObject> ComplexObjectFaker { get; }
    }
}
