using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using ZalandoShop.ViewModels;
using ZalandoShop.Views;

namespace ZalandoShop
{
   public class ViewModelLocator
    {
        public const string SearchPageKey = "SearchPage";
        public const string ResultPageKey = "ResultPage";
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Configure(SearchPageKey, typeof(SearchView));
            nav.Configure(ResultPageKey, typeof(ResultView));

            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<ResultViewModel>(true);
        }

        public SearchViewModel SearchViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SearchViewModel>();
            }
        }
        public ResultViewModel ResultViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ResultViewModel>();
            }
        }
    }
}
