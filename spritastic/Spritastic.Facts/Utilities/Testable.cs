using System;
using StructureMap.AutoMocking;
using Moq;

namespace Spritastic.Facts.Utilities
{
    public class Testable<TClassUnderTest> where TClassUnderTest : class
    {
        protected MoqAutoMocker<TClassUnderTest> autoMocker = new MoqAutoMocker<TClassUnderTest>();

        public Testable()
        {

        }

        public Testable(Action<Testable<TClassUnderTest>> setup)
        {
            setup(this);
        }

        public Mock<TDependencyToMock> Mock<TDependencyToMock>() where TDependencyToMock : class
        {
            var a = autoMocker.Get<TDependencyToMock>();
            return Moq.Mock.Get(a);
        }

        public void Inject<T>(T type)
        {
            autoMocker.Inject(type);
        }

        public void InjectArray<T>(T[] types)
        {
            autoMocker.InjectArray(types);
        }

        public TClassUnderTest ClassUnderTest
        {
            get { return autoMocker.ClassUnderTest; }
        }
    }
}