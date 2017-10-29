using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ZalandoShop.Models;
using ZalandoShop.Repository;

namespace ZalandoShop.ViewModels
{
   public class ResultViewModel:ViewModelBase
    {
        FacetsRepository _articlesRepInstance;
        private readonly INavigationService _navigationService;
        private string brandKey { get; set; }

        private ObservableCollection<Content> filteredArticles;
        public   RelayCommand BackToSearchPage { get; private set;}
        public ObservableCollection<Content> FilteredArticles

        {
            get { return filteredArticles; }
            set {
                    filteredArticles = value;
                    RaisePropertyChanged("FilteredArticles");
                }
        }

       private void InitializeCommands()
        {
            BackToSearchPage = new RelayCommand(BackToSearchPageAction);
        }

        private void BackToSearchPageAction()
        {
            MessengerInstance.Unregister<Facet>(this);
            FilteredArticles.Clear();
            _navigationService.NavigateTo("SearchPage");
        }

        private void IntializeMessenger()
        {
            MessengerInstance.Register<Facet>(this, async (facet) =>
            {
                FilteredArticles = await _articlesRepInstance.GetArticlesList(facet.key, 1, 10, "male");
                brandKey = facet.key;
            });
        }
        public  ResultViewModel(INavigationService navigationService)
        {
            _articlesRepInstance = new FacetsRepository();
            filteredArticles = new ObservableCollection<Content>();
            _articlesRepInstance = new FacetsRepository();
            IntializeMessenger();
         //   InitializeCommands();
            _navigationService = navigationService;
         }

    }
}
