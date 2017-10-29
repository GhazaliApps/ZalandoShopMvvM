using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using ZalandoShop.Repository;

namespace ZalandoShop.Utility
{   
    public class PaginatedCollection<T> : ObservableCollection<T>,ISupportIncrementalLoading
    {
        private Func<uint, Task<IEnumerable<T>>> load;
        public bool HasMoreItems { get; protected set; }
        private FacetsRepository repInstance = new FacetsRepository();
        int pageNumber = 0;

        public PaginatedCollection(Func<uint, Task<IEnumerable<T>>> load)
        {
            HasMoreItems = true;
            this.load = load;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run(async c =>
            {
                var data = await repInstance.GetArticlesList("NA5",++pageNumber,10,"male");
                HasMoreItems = true;

                return new LoadMoreItemsResult()
                {
                    Count = (uint)data.Count()
                };
            });
        }
    }

}

