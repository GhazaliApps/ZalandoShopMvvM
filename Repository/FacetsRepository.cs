using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZalandoShop.Models;

namespace ZalandoShop.Repository
{
    public class FacetsRepository
    {
        private string url = "https://api.zalando.com/facets";
        private string articlesUrl= "https://api.zalando.com/articles?brand={0}&page={1}&pageSize={2}&gender={3}";
        public static Articles articles { get; set; }
        public ObservableCollection<Content> FilteredArticles;
        private const string FacetsFileName = "Facets.txt";

        public async Task<ObservableCollection<Facets>> GetFacetsList()
        {
            ObservableCollection<Facets> FacetsList = new ObservableCollection<Facets>();
            string buffer = "";
            try
            {
                //check cache first
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                if (await StorageHelper.DoesFileExistAsync((FacetsFileName), localFolder))
                {
                    //use cached version
                    StorageFile file = await localFolder.GetFileAsync(FacetsFileName);
                    buffer = await FileIO.ReadTextAsync(file);
                    FacetsList = JsonConvert.DeserializeObject<ObservableCollection<Facets>>(buffer);
                }
                else //download and store now
                {
                    HttpClient http = new HttpClient();
                    buffer = await http.GetStringAsync(url);
                    FacetsList = JsonConvert.DeserializeObject<ObservableCollection<Facets>>(buffer);

                    //store the response now
                    StorageFile storageFile = await localFolder.CreateFileAsync(FacetsFileName);
                    await FileIO.WriteTextAsync(storageFile, buffer);
                }

                return FacetsList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ObservableCollection<Content>> GetArticlesList(string facetKey, int pageNumber, int PageSize, string gender)
        {
            try {
                HttpClient http = new HttpClient();
                url = String.Format(articlesUrl, facetKey, pageNumber, PageSize, gender);
                var articlesResponse = await http.GetStringAsync(url);
                articles = JsonConvert.DeserializeObject<Articles>(articlesResponse);
                FilteredArticles = new ObservableCollection<Content>();
                foreach (Content article in articles.content)
                {
                    if (article != null)
                    {
                        FilteredArticles.Add(article);
                    }
                }
                return FilteredArticles;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }
}
