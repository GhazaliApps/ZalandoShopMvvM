using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZalandoShop.Models;
using ZalandoShop.Repository;

namespace ZalandoShop.ViewModels
{
    public  class SearchViewModel: ViewModelBase
    {

        private string searchText;
        private ObservableCollection<Facets> filteredFacets;
        public ObservableCollection<Facet> results;
        FacetsRepository _facetRepInstance;
        private readonly INavigationService _navigationService;
        public RelayCommand NavigateCommand { get; private set; }
        private Facet selectedFacet;

        public Facet SelectedFacet
        {
            get { return selectedFacet; }
            set
            {
                selectedFacet = value;
                RaisePropertyChanged("SelectedFacet");
            }
        }
        public ObservableCollection<Facets> FilteredFacets
        {
            get
            {
                return filteredFacets;
            }
            set
            {
                filteredFacets = value;
                RaisePropertyChanged("FilteredFacets");
            }
        }
        public ObservableCollection<Facet> Results
        {
            get
            {
                return results;
            }
            set
            {
                results = value;
                RaisePropertyChanged("Results");
            }
        }
        public string SearchText
        {
            get { return this.searchText; }
            set
            {
                this.searchText = value;
                RaisePropertyChanged("SearchText");
                LoadContents(this.searchText);
            }
        }

        private  void LoadContents(string search_term)
       {
           results.Clear();
            if (search_term.Length >= 3)
            {
                if (filteredFacets != null)
                {
                    foreach (Facets f in filteredFacets)
                    {
                        foreach (Facet item in f.facets)
                        {
                            if (item != null && (item.displayName.ToString().ToLower().StartsWith(search_term.ToLower())))
                            {
                                results.Add(item);
                            }
                        }
                    }
                    if (results.Count <= 0)
                    {
                        searchText = "No Results";
                        results.Clear();
                    }
                }
            }
        }


        private void NavigateCommandAction()
        {
            //send Selected Facet to the Result Page via mvvmlight messenger 
           MessengerInstance.Send<Facet>(SelectedFacet);

            //Navigate to Result page 
            _navigationService.NavigateTo("ResultPage");
        }

        public async void Initialize()
        {
            FilteredFacets = await _facetRepInstance.GetFacetsList();
        }
        private void InitializeCommands()
        {
            NavigateCommand = new RelayCommand(NavigateCommandAction);
        }
        public SearchViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            results = new ObservableCollection<Facet>();
            filteredFacets = new ObservableCollection<Facets>();
            _facetRepInstance = new FacetsRepository();
            results = new ObservableCollection<Facet>();
            InitializeCommands();
            Initialize();
        }

        
    }
}
