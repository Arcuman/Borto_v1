using GalaSoft.MvvmLight.Views;
namespace Borto_v1
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
